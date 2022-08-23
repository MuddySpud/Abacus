// Copyright (C) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.ApplicationInspector.Common;
using Microsoft.ApplicationInspector.RulesEngine;
using Microsoft.CST.OAT;
using Microsoft.CST.RecursiveExtractor;
using MuddySpud.Metrics.Engine;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MuddySpud.RulesEngine.Commands
{
    /// <summary>
    /// Heart of RulesEngine. Parses code applies rules
    /// </summary>
    public class MtRuleProcessor
    {
        private readonly int MAX_TEXT_SAMPLE_LENGTH = 200;//char bytes

        private Confidence ConfidenceLevelFilter { get; }
        private readonly Logger? _logger;
        private readonly bool _treatEverythingAsCode;
        private readonly Analyzer analyzer;
        private readonly RuleSet _ruleset;
        private readonly ConcurrentDictionary<string, IEnumerable<ConvertedOatRule>> _fileRulesCache = new();
        private readonly ConcurrentDictionary<string, IEnumerable<ConvertedOatRule>> _languageRulesCache = new();
        private IEnumerable<ConvertedOatRule>? _universalRulesCache;

        /// <summary>
        /// Sets severity levels for analysis
        /// </summary>
        private Severity SeverityLevel { get; }

        /// <summary>
        /// Enables caching of rules queries if multiple reuses per instance
        /// </summary>
        private bool EnableCache { get; }

        /// <summary>
        /// Creates instance of RuleProcessor
        /// </summary>
        public MtRuleProcessor(RuleSet rules, RuleProcessorOptions opts)
        {
            _ruleset = rules;
            EnableCache = true;
            _logger = opts.logger;
            _treatEverythingAsCode = opts.treatEverythingAsCode;
            ConfidenceLevelFilter = opts.confidenceFilter;
            SeverityLevel = Severity.Critical | Severity.Important | Severity.Moderate | Severity.BestPractice; //finds all; arg not currently supported

            analyzer = new Analyzer(new AnalyzerOptions(false, opts.Parallel));
            analyzer.SetOperation(new MtWithinOperation(analyzer));
            analyzer.SetOperation(new MtOATRegexWithIndexOperation(analyzer));
            analyzer.SetOperation(new MtOatSubstringIndexOperation(analyzer));
        }

        private static string ExtractDependency(
            BlockTextContainer? blockTextContainer,
            int startIndex,
            string? pattern,
            string? language)
        {
            if (blockTextContainer is null
                || string.IsNullOrEmpty(blockTextContainer.RawContent)
                || string.IsNullOrEmpty(language)
                || string.IsNullOrEmpty(pattern))
            {
                return string.Empty;
            }

            string rawResult = string.Empty;

            int endIndex = blockTextContainer.RawContent.IndexOfAny(
                new char[] { '\n', '\r' },
                startIndex
            );

            if (-1 != startIndex
                && -1 != endIndex)
            {
                rawResult = blockTextContainer.RawContent[startIndex..endIndex].Trim();
                Regex regex = new(pattern ?? string.Empty);
                MatchCollection matches = regex.Matches(rawResult);

                //remove surrounding import or trailing comments
                if (matches?.Any() == true)
                {
                    foreach (Match? match in matches)
                    {
                        if (match?.Groups.Count == 1)//handles cases like "using Newtonsoft.Json"
                        {
                            string[] parseValues = match.Groups[0].Value.Split(' ');

                            if (parseValues.Length == 1)
                            {
                                rawResult = parseValues[0].Trim();
                            }
                            else if (parseValues.Length > 1)
                            {
                                rawResult = parseValues[1].Trim(); //should be value; time will tell if fullproof
                            }
                        }
                        else if (match?.Groups.Count > 1)//handles cases like include <stdio.h>
                        {
                            rawResult = match.Groups[1].Value.Trim();
                        }
                        //else if > 2 too hard to match; do nothing

                        break;//only designed to expect one match per line i.e. not include value include value
                    }
                }

                string finalResult = rawResult.Replace(";", "");

                return System.Net.WebUtility.HtmlEncode(finalResult);
            }

            return rawResult;
        }

        public MetricsRecord AnalyzeFile(
            string contents,
            FileEntry fileEntry,
            LanguageInfo languageInfo,
            IEnumerable<string>? tagsToIgnore = null,
            int numLinesContext = 3)
        {
            var rules = GetRulesForFile(
                languageInfo,
                fileEntry,
                tagsToIgnore
            );

            MetricsRecord metricsRecord = new();

            BlockTextContainer blockTextContainer = new(
                contents,
                languageInfo.Name
            );

            var caps = analyzer.GetCaptures(
                rules,
                blockTextContainer)
                    .ToList();

            foreach (var ruleCapture in caps)
            {
                foreach (var cap in ruleCapture.Captures)
                {
                    metricsRecord.Matches.AddRange(ProcessBoundary(cap));
                }

                List<MtMatchRecord> ProcessBoundary(ClauseCapture cap)
                {
                    List<MtMatchRecord> newMatches = new();//matches for this rule clause only

                    if (cap is TypedClauseCapture<List<(int, Boundary)>> tcc)
                    {
                        if (ruleCapture.Rule is ConvertedOatRule oatRule)
                        {
                            if (tcc?.Result is List<(int, Boundary)> captureResults)
                            {
                                foreach (var match in captureResults)
                                {
                                    var patternIndex = match.Item1;
                                    var boundary = match.Item2;

                                    //restrict adds from build files to tags with "metadata" only to avoid false feature positives that are not part of executable code
                                    if (!_treatEverythingAsCode
                                        && languageInfo.Type == LanguageInfo.LangFileType.Build
                                        && (oatRule.AppInspectorRule.Tags?.Any(v => !v.Contains("Metadata")) ?? false))
                                    {
                                        continue;
                                    }

                                    if (!ConfidenceLevelFilter.HasFlag(oatRule.AppInspectorRule.Patterns[patternIndex].Confidence))
                                    {
                                        continue;
                                    }

                                    int startIndex = blockTextContainer.GetFullIndexFromCodeIndex(boundary.Index);
                                    int endIndex = blockTextContainer.GetFullIndexFromCodeIndex(boundary.Index + boundary.Length);
                                    Location startLocation = blockTextContainer.GetLocation(startIndex);
                                    Location endLocation = blockTextContainer.GetLocation(endIndex);

                                    MtMatchRecord newMatch = new(oatRule.AppInspectorRule)
                                    {
                                        FileName = fileEntry.FullPath,
                                        BlockTextContainer = blockTextContainer,
                                        LanguageInfo = languageInfo,
                                        Boundary = boundary,
                                        StartLocationLine = startLocation.Line,
                                        StartLocationColumn = startLocation.Column,
                                        EndLocationLine = endLocation.Line != 0 ? endLocation.Line : startLocation.Line + 1, //match is on last line
                                        EndLocationColumn = endLocation.Column,
                                        MatchingPattern = oatRule.AppInspectorRule.Patterns[patternIndex],
                                        Scope = MtPatternScope.Code, // Need to handle the rest too...
                                        StartIndex = startIndex,
                                        EndIndex = endIndex,

                                        Excerpt = numLinesContext > 0 
                                            ? ExtractExcerpt(
                                                blockTextContainer, 
                                                startLocation.Line, 
                                                numLinesContext
                                            ) 
                                            : string.Empty,

                                        Sample = numLinesContext > -1 
                                            ? ExtractTextSample(
                                                blockTextContainer.RawContent, 
                                                boundary.Index, 
                                                boundary.Length
                                            ) 
                                            : string.Empty
                                    };

                                    if (oatRule.AppInspectorRule.Tags?.Contains("Dependency.SourceInclude") ?? false)
                                    {
                                        newMatch.Sample = ExtractDependency(
                                            newMatch.BlockTextContainer, 
                                            newMatch.Boundary.Index, 
                                            newMatch.Pattern, 
                                            newMatch.LanguageInfo.Name);
                                    }

                                    newMatches.Add(newMatch);
                                }
                            }
                        }
                    }

                    return newMatches;
                }
            }

            List<MtMatchRecord> removes = new();

            foreach (MtMatchRecord m in metricsRecord.Matches.Where(x => x.Rule.Overrides?.Length > 0))
            {
                foreach (string ovrd in m.Rule?.Overrides ?? Array.Empty<string>())
                {
                    // Find all overriden rules and mark them for removal from issues list
                    foreach (MtMatchRecord om in metricsRecord.Matches.FindAll(x => x.Rule.Id == ovrd))
                    {
                        if (om.Boundary?.Index >= m.Boundary?.Index &&
                            om.Boundary?.Index <= m.Boundary?.Index + m.Boundary?.Length)
                        {
                            removes.Add(om);
                        }
                    }
                }
            }

            // Remove overriden rules
            metricsRecord.Matches.RemoveAll(x => removes.Contains(x));

            MetricsProcessor.Aggregate(
                metricsRecord,
                blockTextContainer
            );

            return metricsRecord;
        }

        private IEnumerable<ConvertedOatRule> GetRulesForFile(
            LanguageInfo languageInfo, 
            FileEntry fileEntry, 
            IEnumerable<string>? tagsToIgnore)
        {
            var rulesByLanguage = GetRulesByLanguage(languageInfo.Name).Where(x => !x.AppInspectorRule.Disabled && SeverityLevel.HasFlag(x.AppInspectorRule.Severity));
            var rules = rulesByLanguage.Union(GetRulesByFileName(fileEntry.FullPath).Where(x => !x.AppInspectorRule.Disabled && SeverityLevel.HasFlag(x.AppInspectorRule.Severity)));
            rules = rules.Union(GetUniversalRules());

            if (tagsToIgnore?.Any() == true)
            {
                rules = rules.Where(x => x.AppInspectorRule?.Tags?.Any(y => !tagsToIgnore.Contains(y)) ?? false);
            }

            return rules;
        }

        public MetricsRecord AnalyzeFile(
            FileEntry fileEntry, 
            LanguageInfo languageInfo, 
            IEnumerable<string>? tagsToIgnore = null, 
            int numLinesContext = 3)
        {
            using var sr = new StreamReader(fileEntry.Content);
            var contents = string.Empty;

            try
            {
                contents = sr.ReadToEnd();
            }
            catch (Exception e)
            {
                WriteOnce.SafeLog($"Failed to analyze file {fileEntry.FullPath}. {e.GetType()}:{e.Message}. ({e.StackTrace})", LogLevel.Debug);
            }

            if (!string.IsNullOrEmpty(contents))
            {
                return AnalyzeFile(
                    contents, 
                    fileEntry, 
                    languageInfo, 
                    tagsToIgnore, 
                    numLinesContext
                );
            }

            return new MetricsRecord();
        }

        public async Task<MetricsRecord> AnalyzeFileAsync(
            FileEntry fileEntry, 
            LanguageInfo languageInfo, 
            CancellationToken cancellationToken, 
            IEnumerable<string>? tagsToIgnore = null, 
            int numLinesContext = 3)
        {
            var rules = GetRulesForFile(languageInfo, fileEntry, tagsToIgnore);

            MetricsRecord metricsRecord = new();
            using var sr = new StreamReader(fileEntry.Content);

            BlockTextContainer blockTextContainer = new(
                await sr.ReadToEndAsync().ConfigureAwait(false), 
                languageInfo.Name
            );

            foreach (var ruleCapture in analyzer.GetCaptures(rules, blockTextContainer))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return metricsRecord;
                }

                foreach (var cap in ruleCapture.Captures)
                {
                    metricsRecord.Matches.AddRange(ProcessBoundary(cap));
                }

                List<MtMatchRecord> ProcessBoundary(ClauseCapture cap)
                {
                    List<MtMatchRecord> newMatches = new();//matches for this rule clause only

                    if (cap is TypedClauseCapture<List<(int, Boundary)>> tcc)
                    {
                        if (ruleCapture.Rule is ConvertedOatRule oatRule)
                        {
                            if (tcc?.Result is List<(int, Boundary)> captureResults)
                            {
                                foreach (var match in captureResults)
                                {
                                    var patternIndex = match.Item1;
                                    var boundary = match.Item2;

                                    //restrict adds from build files to tags with "metadata" only to avoid false feature positives that are not part of executable code
                                    if (!_treatEverythingAsCode && languageInfo.Type == LanguageInfo.LangFileType.Build 
                                        && (oatRule.AppInspectorRule.Tags?.Any(v => !v.Contains("Metadata")) ?? false))
                                    {
                                        continue;
                                    }

                                    if (patternIndex < 0 || patternIndex > oatRule.AppInspectorRule.Patterns.Length)
                                    {
                                        _logger?.Error("Index out of range for patterns for rule: " + oatRule.AppInspectorRule.Name);
                                        continue;
                                    }

                                    if (!ConfidenceLevelFilter.HasFlag(oatRule.AppInspectorRule.Patterns[patternIndex].Confidence))
                                    {
                                        continue;
                                    }

                                    int startIndex = blockTextContainer.GetFullIndexFromCodeIndex(boundary.Index);
                                    int endIndex = blockTextContainer.GetFullIndexFromCodeIndex(boundary.Index + boundary.Length);
                                    Location startLocation = blockTextContainer.GetLocation(startIndex);
                                    Location endLocation = blockTextContainer.GetLocation(endIndex);

                                    MtMatchRecord newMatch = new(oatRule.AppInspectorRule)
                                    {
                                        FileName = fileEntry.FullPath,
                                        BlockTextContainer = blockTextContainer,
                                        LanguageInfo = languageInfo,
                                        Boundary = boundary,
                                        StartLocationLine = startLocation.Line,
                                        EndLocationLine = endLocation.Line != 0 ? endLocation.Line : startLocation.Line + 1, //match is on last line
                                        MatchingPattern = oatRule.AppInspectorRule.Patterns[patternIndex],
                                        Scope = MtPatternScope.Code, // Need to handle the rest too...
                                        StartIndex = startIndex,
                                        EndIndex = endIndex,

                                        Excerpt = numLinesContext > 0 
                                            ? ExtractExcerpt(
                                                blockTextContainer, 
                                                startLocation.Line, 
                                                numLinesContext
                                            ) 
                                            : string.Empty,

                                        Sample = numLinesContext > -1 
                                            ? ExtractTextSample(
                                                blockTextContainer.RawContent, 
                                                boundary.Index, 
                                                boundary.Length
                                            ) 
                                            : string.Empty
                                    };

                                    if (oatRule.AppInspectorRule.Tags?.Contains("Dependency.SourceInclude") ?? false)
                                    {
                                        newMatch.Sample = ExtractDependency(
                                            newMatch.BlockTextContainer, 
                                            newMatch.Boundary.Index, 
                                            newMatch.Pattern, 
                                            newMatch.LanguageInfo.Name
                                        );
                                    }

                                    newMatches.Add(newMatch);
                                }
                            }
                        }
                    }
                    return newMatches;
                }
            }

            List<MtMatchRecord> removes = new();

            foreach (MtMatchRecord m in metricsRecord.Matches.Where(x => x.Rule.Overrides?.Length > 0))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return metricsRecord;
                }

                foreach (string ovrd in m.Rule?.Overrides ?? Array.Empty<string>())
                {
                    // Find all overriden rules and mark them for removal from issues list
                    foreach (MtMatchRecord om in metricsRecord.Matches.FindAll(x => x.Rule.Id == ovrd))
                    {
                        if (om.Boundary?.Index >= m.Boundary?.Index &&
                            om.Boundary?.Index <= m.Boundary?.Index + m.Boundary?.Length)
                        {
                            removes.Add(om);
                        }
                    }
                }
            }

            // Remove overriden rules
            metricsRecord.Matches.RemoveAll(x => removes.Contains(x));

            MetricsProcessor.Aggregate(
                metricsRecord,
                blockTextContainer
            );

            return metricsRecord;
        }

        #region Private Support Methods

        /// <summary>
        ///     Filters the rules for those matching the content type. Resolves all the overrides
        /// </summary>
        /// <param name="languages"> Languages to filter rules for </param>
        /// <returns> List of rules </returns>
        private IEnumerable<ConvertedOatRule> GetRulesByLanguage(string input)
        {
            if (EnableCache)
            {
                if (_languageRulesCache.ContainsKey(input))
                {
                    return _languageRulesCache[input];
                }
            }

            IEnumerable<ConvertedOatRule> filteredRules = _ruleset.ByLanguage(input);

            if (EnableCache)
            {
                _languageRulesCache.TryAdd(input, filteredRules);
            }

            return filteredRules;
        }

        /// <summary>
        ///     Filters the rules for those matching the content type. Resolves all the overrides
        /// </summary>
        /// <param name="languages"> Languages to filter rules for </param>
        /// <returns> List of rules </returns>
        private IEnumerable<ConvertedOatRule> GetUniversalRules()
        {
            if (_universalRulesCache is null)
            {
                if (EnableCache)
                {
                    _universalRulesCache = _ruleset.GetUniversalRules();
                }
                else
                {
                    return _ruleset.GetUniversalRules();
                }
            }

            return _universalRulesCache;
        }

        /// <summary>
        ///     Filters the rules for those matching the content type. Resolves all the overrides
        /// </summary>
        /// <param name="languages"> Languages to filter rules for </param>
        /// <returns> List of rules </returns>
        private IEnumerable<ConvertedOatRule> GetRulesByFileName(string input)
        {
            if (EnableCache)
            {
                if (_fileRulesCache.ContainsKey(input))
                {
                    return _fileRulesCache[input];
                }
            }

            IEnumerable<ConvertedOatRule> filteredRules = _ruleset.ByFilename(input);

            if (EnableCache)
            {
                _fileRulesCache.TryAdd(input, filteredRules);
            }

            return filteredRules;
        }

        /// <summary>
        /// Simple wrapper but keeps calling code consistent
        /// Do not html code result which is accomplished later before out put to report
        /// </summary>
        private string ExtractTextSample(
            string fileText,
            int index,
            int length)
        {
            if (index < 0 || length < 0) { return fileText; }

            length = Math.Min(
                Math.Min(
                    length,
                    MAX_TEXT_SAMPLE_LENGTH
                ),
                fileText.Length - index
            );

            if (length == 0)
            {
                return string.Empty;
            }

            return fileText[index..(index + length)].Trim();
        }

        /// <summary>
        /// Located here to include during Match creation to avoid a call later or putting in constructor
        /// Needed in match ensuring value exists at time of report writing rather than expecting a callback
        /// from the template
        /// </summary>
        /// <returns></returns>
        private static string ExtractExcerpt(
            BlockTextContainer blockTextContainer,
            int startLineNumber,
            int context = 3)
        {
            if (context == 0)
            {
                return string.Empty;
            }
            if (startLineNumber < 0)
            {
                startLineNumber = 0;
            }

            if (startLineNumber >= blockTextContainer.LineEnds.Count)
            {
                startLineNumber = blockTextContainer.LineEnds.Count - 1;
            }

            var excerptStartLine = Math.Max(
                0, 
                startLineNumber - context
            );

            var excerptEndLine = Math.Min(
                blockTextContainer.LineEnds.Count - 1, 
                startLineNumber + context
            );

            var startIndex = blockTextContainer.LineStarts[excerptStartLine];
            var endIndex = blockTextContainer.LineEnds[excerptEndLine] + 1;
            var maxCharacterContext = context * 100;

            // Only gather 100*lines context characters to avoid gathering super long lines
            if (blockTextContainer.LineStarts[startLineNumber] - startIndex > maxCharacterContext)
            {
                startIndex = Math.Max(
                    0, 
                    startIndex - maxCharacterContext
                );
            }
            if (endIndex - blockTextContainer.LineEnds[startLineNumber] > maxCharacterContext)
            {
                endIndex = Math.Min(
                    blockTextContainer.RawContent.Length - 1, 
                    endIndex + maxCharacterContext
                );
            }

            return blockTextContainer.RawContent[startIndex..endIndex];
        }

        #endregion Private Methods
    }
}
