// Copyright (C) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using GlobExpressions;
using Microsoft.ApplicationInspector.Commands;
using Microsoft.ApplicationInspector.Common;
using Microsoft.ApplicationInspector.RulesEngine;
using Microsoft.CST.RecursiveExtractor;
using MuddySpud.Metrics.Engine;
using Newtonsoft.Json;
using NLog;
using ShellProgressBar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MuddySpud.RulesEngine.Commands
{
    /// <summary>
    /// Analyze operation for setup and processing of results from Rulesengine
    /// </summary>
    public class MtAnalyzeCommand
    {
        private readonly List<string> _srcfileList = new();
        private MtMetaDataHelper? _metaDataHelper; //wrapper containing MetaData object to be assigned to result
        private MtRuleProcessor? _rulesProcessor;
        private const int _sleepDelay = 100;
        private DateTime DateScanned { get; }

        private readonly List<Glob> _fileExclusionList = new();
        private Confidence _confidence;
        private readonly MtAnalyzeOptions _options; //copy of incoming caller options

        /// <summary>
        /// Constructor for AnalyzeCommand.
        /// </summary>
        /// <param name="opt"></param>
        public MtAnalyzeCommand(MtAnalyzeOptions opt)
        {
            _options = opt;

            if (opt.FilePathExclusions.Any(x => !x.Equals("none")))
            {
                _fileExclusionList = opt.FilePathExclusions.Select(x => new Glob(x)).ToList();
            }
            else
            {
                _fileExclusionList = new List<Glob>();
            }

            DateScanned = DateTime.Now;

            try
            {
                _options.Log ??= Utils.SetupLogging(_options);
                WriteOnce.Log ??= _options.Log;

                ConfigureConsoleOutput();
                ConfigSourcetoScan();
                ConfigConfidenceFilters();
                ConfigRules();
            }
            catch (OpException e) //group error handling
            {
                WriteOnce.Error(e.Message);
                throw;
            }
        }

        #region configureMethods

        /// <summary>
        /// Establish console verbosity
        /// For NuGet DLL use, console is automatically muted overriding any arguments sent
        /// </summary>
        private void ConfigureConsoleOutput()
        {
            WriteOnce.SafeLog(
                "AnalyzeCommand::ConfigureConsoleOutput",
                LogLevel.Trace
            );

            //Set console verbosity based on run context (none for DLL use) and caller arguments
            if (!Utils.CLIExecutionContext)
            {
                WriteOnce.Verbosity = WriteOnce.ConsoleVerbosity.None;
            }
            else
            {
                if (!Enum.TryParse(
                        _options.ConsoleVerbosityLevel,
                        true,
                        out WriteOnce.ConsoleVerbosity verbosity))
                {
                    throw new OpException(
                        MsgHelp.FormatString(
                            MsgHelp.ID.CMD_INVALID_ARG_VALUE,
                            "-x"
                        )
                    );
                }
                else
                {
                    WriteOnce.Verbosity = verbosity;
                }
            }
        }

        /// <summary>
        /// Expects user to supply all that apply impacting which rule pattern matches are returned
        /// </summary>
        private void ConfigConfidenceFilters()
        {
            WriteOnce.SafeLog(
                "AnalyzeCommand::ConfigConfidenceFilters",
                LogLevel.Trace
            );

            //parse and verify confidence values
            if (string.IsNullOrEmpty(_options.ConfidenceFilters))
            {
                _confidence = Confidence.High | Confidence.Medium; //excludes low by default
            }
            else
            {
                string[] confidences = _options.ConfidenceFilters.Split(',');

                foreach (string confidence in confidences)
                {
                    Confidence single;

                    if (Enum.TryParse(confidence, true, out single))
                    {
                        _confidence |= single;
                    }
                    else
                    {
                        throw new OpException(
                            MsgHelp.FormatString(
                                MsgHelp.ID.CMD_INVALID_ARG_VALUE,
                                "x"
                            )
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Simple validation on source path provided for scanning and preparation
        /// </summary>
        private void ConfigSourcetoScan()
        {
            WriteOnce.SafeLog(
                "AnalyzeCommand::ConfigSourcetoScan",
            LogLevel.Trace);

            if (!_options.SourcePath.Any())
            {
                throw new OpException(
                    MsgHelp.FormatString(
                        MsgHelp.ID.CMD_REQUIRED_ARG_MISSING,
                        "SourcePath"
                    )
                );
            }

            foreach (var entry in _options.SourcePath)
            {
                if (Directory.Exists(entry))
                {
                    try
                    {
                        _srcfileList.AddRange(
                            Directory.EnumerateFiles(
                                entry,
                                "*.*",
                                SearchOption.AllDirectories
                            )
                        );
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else if (File.Exists(entry))
                {
                    _srcfileList.Add(entry);
                }
                else
                {
                    throw new OpException(
                        MsgHelp.FormatString(
                            MsgHelp.ID.CMD_INVALID_FILE_OR_DIR,
                            string.Join(
                                ',',
                                _options.SourcePath
                            )
                        )
                    );
                }
            }
            if (_srcfileList.Count == 0)
            {
                throw new OpException(
                    MsgHelp.FormatString(
                        MsgHelp.ID.CMD_INVALID_FILE_OR_DIR,
                        string.Join(
                            ',',
                            _options.SourcePath
                        )
                    )
                );
            }
        }

        /// <summary>
        /// Add default and/or custom rules paths
        /// Iterate paths and add to ruleset
        /// </summary>
        private void ConfigRules()
        {
            WriteOnce.SafeLog(
                "AnalyzeCommand::ConfigRules",
                LogLevel.Trace
            );

            RuleSet? rulesSet = null;

            if (!_options.IgnoreDefaultRules)
            {
                rulesSet = RuleSetUtils.GetDefaultRuleSet(_options.Log);
            }

            if (!string.IsNullOrEmpty(_options.CustomRulesPath))
            {
                if (rulesSet == null)
                {
                    rulesSet = new RuleSet(_options.Log);
                }

                if (Directory.Exists(_options.CustomRulesPath))
                {
                    rulesSet.AddDirectory(_options.CustomRulesPath);
                }
                else if (File.Exists(_options.CustomRulesPath)) //verify custom rules before use
                {
                    RulesVerifier verifier = new(
                        _options.CustomRulesPath,
                        _options.Log
                    );

                    verifier.Verify();

                    if (!verifier.IsVerified)
                    {
                        throw new OpException(MsgHelp.FormatString(MsgHelp.ID.VERIFY_RULE_LOADFILE_FAILED, _options.CustomRulesPath));
                    }

                    rulesSet.AddRange(verifier.CompiledRuleset);
                }
                else
                {
                    throw new OpException(MsgHelp.FormatString(MsgHelp.ID.CMD_INVALID_RULE_PATH, _options.CustomRulesPath));
                }
            }

            //error check based on ruleset not path enumeration
            if (rulesSet == null || !rulesSet.Any())
            {
                throw new OpException(MsgHelp.GetString(MsgHelp.ID.CMD_NORULES_SPECIFIED));
            }

            //instantiate a RuleProcessor with the added rules and exception for dependency
            var rpo = new RuleProcessorOptions()
            {
                logger = _options.Log,
                treatEverythingAsCode = _options.TreatEverythingAsCode,
                confidenceFilter = _confidence,
                Parallel = !_options.SingleThread
            };

            _rulesProcessor = new MtRuleProcessor(
                rulesSet,
                rpo
            );

            //create metadata helper to wrap and help populate metadata from scan
            _metaDataHelper = new MtMetaDataHelper(
                string.Join(
                    ',',
                    _options.SourcePath
                )
            );
        }

        #endregion configureMethods

        /// <summary>
        /// Populate the MetaDataHelper with the data from the FileEntries.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="opts"></param>
        /// <param name="populatedEntries"></param>
        public MtAnalyzeResult.ExitCode PopulateRecords(
            CancellationToken cancellationToken,
            MtAnalyzeOptions opts,
            IEnumerable<FileEntry> populatedEntries)
        {
            WriteOnce.SafeLog(
                "AnalyzeCommand::PopulateRecords",
                LogLevel.Trace
            );

            if (_metaDataHelper is null)
            {
                WriteOnce.Error("MetadataHelper is null");

                throw new ArgumentNullException("_metaDataHelper");
            }

            if (_rulesProcessor is null
                || populatedEntries is null)
            {
                return MtAnalyzeResult.ExitCode.CriticalError;
            }

            if (opts.SingleThread)
            {
                foreach (var entry in populatedEntries)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return MtAnalyzeResult.ExitCode.Canceled;
                    }

                    ProcessAndAddToMetadata(entry);
                }
            }
            else
            {
                try
                {
                    Parallel.ForEach(
                        populatedEntries,
                        new ParallelOptions()
                        {
                            CancellationToken = cancellationToken,
                            MaxDegreeOfParallelism = Environment.ProcessorCount * 3 / 2
                        },
                        entry =>
                        {
                            try
                            {
                                ProcessAndAddToMetadata(entry);
                            }
                            catch (Exception e)
                            {
                                throw;
                            }
                        }
                    );
                }
                catch (OperationCanceledException)
                {
                    return MtAnalyzeResult.ExitCode.Canceled;
                }
            }

            return MtAnalyzeResult.ExitCode.Success;

            void ProcessAndAddToMetadata(FileEntry file)
            {
                var fileRecord = new FileRecord()
                {
                    FileName = file.FullPath,
                    ModifyTime = file.ModifyTime,
                    CreateTime = file.CreateTime,
                    AccessTime = file.AccessTime
                };

                AbacusRecord abacusRecord = new(fileRecord);
                var sw = new Stopwatch();
                sw.Start();

                if (_fileExclusionList.Any(x => x.IsMatch(file.FullPath)))
                {
                    WriteOnce.SafeLog(
                        MsgHelp.FormatString(
                            MsgHelp.ID.ANALYZE_EXCLUDED_TYPE_SKIPPED,
                            fileRecord.FileName
                        ),
                        LogLevel.Debug
                    );

                    fileRecord.Status = ScanState.Skipped;
                }
                else
                {
                    if (IsBinary(file.Content))
                    {
                        WriteOnce.SafeLog(
                            MsgHelp.FormatString(
                                MsgHelp.ID.ANALYZE_EXCLUDED_BINARY,
                                fileRecord.FileName
                            ),
                            LogLevel.Debug
                        );

                        fileRecord.Status = ScanState.Skipped;
                    }
                    else
                    {
                        _ = _metaDataHelper.FileExtensions.TryAdd(
                            Path
                                .GetExtension(file.FullPath)
                                .Replace('.', ' ')
                                .TrimStart(),
                            0
                        );

                        LanguageInfo languageInfo = new LanguageInfo();

                        if (Language.FromFileName(file.FullPath, ref languageInfo))
                        {
                            _metaDataHelper.AddLanguage(languageInfo.Name);
                        }
                        else
                        {
                            _metaDataHelper.AddLanguage("Unknown");

                            languageInfo = new LanguageInfo()
                            {
                                Extensions = new string[]
                                {
                                    Path.GetExtension(file.FullPath)
                                },
                                Name = "Unknown"
                            };

                            if (!_options.ScanUnknownTypes)
                            {
                                fileRecord.Status = ScanState.Skipped;
                            }
                        }

                        if (fileRecord.Status != ScanState.Skipped)
                        {
                            MetricsRecord? metricsRecord = null;

                            if (opts.FileTimeOut > 0)
                            {
                                using var cts = new CancellationTokenSource();

                                var t = Task.Run(() =>
                                    {
                                        if (opts.TagsOnly)
                                        {
                                            metricsRecord = _rulesProcessor.AnalyzeFile(
                                                file,
                                                languageInfo,
                                                _metaDataHelper.UniqueTags.Keys,
                                                -1
                                            );
                                        }
                                        else
                                        {
                                            metricsRecord = _rulesProcessor.AnalyzeFile(
                                                file,
                                                languageInfo,
                                                null,
                                                opts.ContextLines
                                            );
                                        }
                                    },
                                    cts.Token
                                );

                                try
                                {
                                    if (!t.Wait(new TimeSpan(0, 0, 0, 0, opts.FileTimeOut)))
                                    {
                                        WriteOnce.Error($"{file.FullPath} timed out.");
                                        fileRecord.Status = ScanState.TimedOut;
                                        cts.Cancel();
                                    }
                                    else
                                    {
                                        fileRecord.Status = ScanState.Analyzed;
                                    }
                                }
                                catch (Exception e)
                                {
                                    WriteOnce.SafeLog(
                                        $"Failed to analyze file {file.FullPath}. {e.GetType()}:{e.Message}. ({e.StackTrace})",
                                        LogLevel.Debug
                                    );

                                    fileRecord.Status = ScanState.Error;
                                }
                            }
                            else
                            {
                                if (opts.TagsOnly)
                                {
                                    metricsRecord = _rulesProcessor.AnalyzeFile(
                                        file,
                                        languageInfo,
                                        _metaDataHelper.UniqueTags.Keys,
                                        -1
                                    );
                                }
                                else
                                {
                                    metricsRecord = _rulesProcessor.AnalyzeFile(
                                        file,
                                        languageInfo,
                                        null,
                                        opts.ContextLines
                                    );
                                }

                                fileRecord.Status = ScanState.Analyzed;
                            }

                            if (metricsRecord is { })
                            {
                                if (metricsRecord.Matches.Any())
                                {
                                    fileRecord.Status = ScanState.Affected;
                                    fileRecord.NumFindings = metricsRecord.Matches.Count;
                                }

                                foreach (var matchRecord in metricsRecord.Matches)
                                {
                                    if (opts.TagsOnly)
                                    {
                                        _metaDataHelper.AddTagsFromMatchRecord(matchRecord);
                                    }
                                    else
                                    {
                                        _metaDataHelper.AddMatchRecord(matchRecord);
                                    }
                                }

                                abacusRecord.Metrics = metricsRecord;
                            }
                        }
                    }
                }

                sw.Stop();
                fileRecord.ScanTime = sw.Elapsed;
                _metaDataHelper.AbacusRecords.Add(abacusRecord);
            }
        }

        /// <summary>
        /// Populate the records in the metadata asynchronously.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Result code.</returns>
        public async Task<MtAnalyzeResult.ExitCode> PopulateRecordsAsync(CancellationToken cancellationToken)
        {
            WriteOnce.SafeLog(
                "AnalyzeCommand::PopulateRecordsAsync",
                LogLevel.Trace
            );

            if (_metaDataHelper is null)
            {
                WriteOnce.Error("MetadataHelper is null");

                throw new ArgumentNullException("_metaDataHelper");
            }

            if (_rulesProcessor is null)
            {
                return MtAnalyzeResult.ExitCode.CriticalError;
            }

            await foreach (var entry in GetFileEntriesAsync())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return MtAnalyzeResult.ExitCode.Canceled;
                }

                await ProcessAndAddToMetadata(
                    entry,
                    cancellationToken
                );
            }

            return MtAnalyzeResult.ExitCode.Success;

            async Task ProcessAndAddToMetadata(
                FileEntry file,
                CancellationToken cancellationToken)
            {
                var fileRecord = new FileRecord()
                {
                    FileName = file.FullPath,
                    ModifyTime = file.ModifyTime,
                    CreateTime = file.CreateTime,
                    AccessTime = file.AccessTime
                };

                AbacusRecord abacusRecord = new(fileRecord);
                var sw = new Stopwatch();
                sw.Start();

                if (_fileExclusionList.Any(x => x.IsMatch(file.FullPath)))
                {
                    WriteOnce.SafeLog(
                        MsgHelp.FormatString(
                            MsgHelp.ID.ANALYZE_EXCLUDED_TYPE_SKIPPED,
                            fileRecord.FileName
                        ),
                        LogLevel.Debug
                    );

                    fileRecord.Status = ScanState.Skipped;
                }
                else
                {
                    if (IsBinary(file.Content))
                    {
                        WriteOnce.SafeLog(
                            MsgHelp.FormatString(
                                MsgHelp.ID.ANALYZE_EXCLUDED_BINARY,
                                fileRecord.FileName
                            ),
                            LogLevel.Debug
                        );

                        fileRecord.Status = ScanState.Skipped;
                    }
                    else
                    {
                        _ = _metaDataHelper.FileExtensions.TryAdd(
                            Path
                                .GetExtension(file.FullPath)
                                .Replace('.', ' ')
                                .TrimStart(),
                            0
                        );

                        LanguageInfo languageInfo = new LanguageInfo();

                        if (Language.FromFileName(file.FullPath, ref languageInfo))
                        {
                            _metaDataHelper.AddLanguage(languageInfo.Name);
                        }
                        else
                        {
                            _metaDataHelper.AddLanguage("Unknown");

                            languageInfo = new LanguageInfo()
                            {
                                Extensions = new string[] { Path.GetExtension(file.FullPath) },
                                Name = "Unknown"
                            };

                            if (!_options.ScanUnknownTypes)
                            {
                                fileRecord.Status = ScanState.Skipped;
                            }
                        }

                        if (fileRecord.Status != ScanState.Skipped)
                        {
                            abacusRecord.Metrics = _options.TagsOnly
                                ? await _rulesProcessor.AnalyzeFileAsync(file, languageInfo, cancellationToken, _metaDataHelper.UniqueTags.Keys, -1)
                                : await _rulesProcessor.AnalyzeFileAsync(file, languageInfo, cancellationToken, null, _options.ContextLines);

                            fileRecord.Status = ScanState.Analyzed;

                            if (abacusRecord.Metrics.Matches.Any())
                            {
                                fileRecord.Status = ScanState.Affected;
                                fileRecord.NumFindings = abacusRecord.Metrics.Matches.Count;
                            }

                            foreach (var matchRecord in abacusRecord.Metrics.Matches)
                            {
                                if (_options.TagsOnly)
                                {
                                    _metaDataHelper.AddTagsFromMatchRecord(matchRecord);
                                }
                                else
                                {
                                    _metaDataHelper.AddMatchRecord(matchRecord);
                                }
                            }
                        }
                    }
                }

                sw.Stop();
                fileRecord.ScanTime = sw.Elapsed;
                _metaDataHelper.AbacusRecords.Add(abacusRecord);
            }
        }

        /// <summary>
        /// Gets the FileEntries synchronously.
        /// </summary>
        /// <returns>An Enumerable of FileEntries.</returns>
        private IEnumerable<FileEntry> GetFileEntries()
        {
            WriteOnce.SafeLog(
                "AnalyzeCommand::GetFileEntries",
                LogLevel.Trace
            );

            Extractor extractor = new();

            // For every file, if the file isn't excluded return it, and if it is track the exclusion in the metadata
            foreach (var srcFile in _srcfileList)
            {
                if (!_fileExclusionList.Any(x => x.IsMatch(srcFile)))
                {
                    foreach (var entry in extractor.Extract(
                        srcFile,
                        new ExtractorOptions()
                        {
                            Parallel = false,
                            DenyFilters = _options.FilePathExclusions,
                            AllowFilters = _options.WhiteListExtensions,
                            MemoryStreamCutoff = 1
                        })
                    )
                    {
                        yield return entry;
                    }
                }
                else
                {
                    _metaDataHelper?.Metadata.AbacusRecords.Add(
                        new AbacusRecord(
                            new FileRecord()
                            {
                                FileName = srcFile,
                                Status = ScanState.Skipped
                            }
                        )
                    );
                }
            }
        }

        /// <summary>
        /// Gets the FileEntries asynchronously.
        /// </summary>
        /// <returns>An enumeration of FileEntries</returns>
        private async IAsyncEnumerable<FileEntry> GetFileEntriesAsync()
        {
            WriteOnce.SafeLog("AnalyzeCommand::GetFileEntriesAsync", LogLevel.Trace);

            Extractor extractor = new();

            foreach (var srcFile in _srcfileList ?? new List<string>())
            {
                if (!_fileExclusionList.Any(x => x.IsMatch(srcFile)))
                {
                    await foreach (var entry in extractor.ExtractAsync(
                        srcFile,
                        new ExtractorOptions()
                        {
                            Parallel = false,
                            DenyFilters = _options.FilePathExclusions,
                            AllowFilters = _options.WhiteListExtensions,
                            MemoryStreamCutoff = 1
                        })
                    )
                    {
                        yield return entry;
                    }
                }
                else
                {
                    _metaDataHelper?.Metadata.AbacusRecords.Add(
                        new AbacusRecord(
                            new FileRecord()
                            {
                                FileName = srcFile,
                                Status = ScanState.Skipped
                            }
                        )
                    );
                }
            }
        }


        // Follows Perl's model, if there are NULs or too many non printable characters, this is probably a binary file
        private bool IsBinary(Stream fileContents)
        {
            var numRead = 1;
            var span = new Span<byte>(new byte[8192]);
            var controlsEncountered = 0;
            var maxControlsEncountered = (int)(0.3 * fileContents.Length);

            while (numRead > 0)
            {
                numRead = fileContents.Read(span);

                for (var i = 0; i < numRead; i++)
                {
                    var ch = (char)span[i];

                    if (ch == '\0')
                    {
                        fileContents.Position = 0;

                        return true;
                    }
                    else if (char.IsControl(ch)
                        && !char.IsWhiteSpace(ch))
                    {
                        if (++controlsEncountered > maxControlsEncountered)
                        {
                            fileContents.Position = 0;

                            return true;
                        }
                    }
                }

            }

            fileContents.Position = 0;

            return false;
        }

        /// <summary>
        /// Perform Analysis and get the result Asynchronously
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to stop analysis and return results found so far.</param>
        /// <returns></returns>
        public async Task<MtAnalyzeResult> GetResultAsync(CancellationToken cancellationToken)
        {
            WriteOnce.SafeLog("AnalyzeCommand::GetResultAsync", LogLevel.Trace);
            WriteOnce.Operation(MsgHelp.FormatString(MsgHelp.ID.CMD_RUNNING, "Analyze"));

            if (_metaDataHelper is null)
            {
                WriteOnce.Error("MetadataHelper is null");

                throw new ArgumentNullException("_metaDataHelper");
            }

            MtAnalyzeResult analyzeResult = new()
            {
                AppVersion = Utils.GetVersionString()
            };

            var exitCode = await PopulateRecordsAsync(cancellationToken);

            //wrapup result status
            if (!_options.NoFileMetadata
                && _metaDataHelper.AbacusRecords.All(x => x.File.Status == ScanState.Skipped))
            {
                WriteOnce.Error(MsgHelp.GetString(MsgHelp.ID.ANALYZE_NOSUPPORTED_FILETYPES));

                analyzeResult.ResultCode = MtAnalyzeResult.ExitCode.NoMatches;
            }
            else if (_metaDataHelper.UniqueTagsCount == 0)
            {
                WriteOnce.Error(MsgHelp.GetString(MsgHelp.ID.ANALYZE_NOPATTERNS));
                analyzeResult.ResultCode = MtAnalyzeResult.ExitCode.NoMatches;
            }
            else if (_metaDataHelper != null && _metaDataHelper.Metadata != null)
            {
                _metaDataHelper.Metadata.DateScanned = DateScanned.ToString();
                _metaDataHelper.PrepareReport();
                analyzeResult.Metadata = _metaDataHelper.Metadata; //replace instance with metadatahelper processed one
                analyzeResult.ResultCode = MtAnalyzeResult.ExitCode.Success;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                analyzeResult.ResultCode = MtAnalyzeResult.ExitCode.Canceled;
            }

            return analyzeResult;
        }

        /// <summary>
        /// Main entry point to start analysis from CLI; handles setting up rules, directory enumeration
        /// file type detection and handoff
        /// Pre: All Configure Methods have been called already and we are ready to SCAN
        /// </summary>
        /// <returns></returns>
        public MtAnalyzeResult GetResult()
        {
            WriteOnce.SafeLog(
                "AnalyzeCommand::GetResult",
                LogLevel.Trace
            );

            WriteOnce.Operation(
                MsgHelp.FormatString(
                    MsgHelp.ID.CMD_RUNNING,
                    "Analyze"
                )
            );

            if (_metaDataHelper is null)
            {
                WriteOnce.Error("MetadataHelper is null");

                throw new ArgumentNullException("_metaDataHelper");
            }

            MtAnalyzeResult analyzeResult = new()
            {
                AppVersion = Utils.GetVersionString()
            };

            var timedOut = false;

            if (!_options.NoShowProgress)
            {
                var done = false;
                ConcurrentBag<FileEntry> fileQueue = new();

                _ = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        foreach (var entry in GetFileEntries())
                        {
                            fileQueue.Add(entry);
                        }
                    }
                    catch (OverflowException e)
                    {
                        WriteOnce.Error($"Overflowed while extracting file entries. Check the input for quines or zip bombs. {e.Message}");
                    }
                    done = true;
                });

                var options = new ProgressBarOptions
                {
                    ForegroundColor = ConsoleColor.Yellow,
                    ForegroundColorDone = ConsoleColor.DarkGreen,
                    BackgroundColor = ConsoleColor.DarkGray,
                    BackgroundCharacter = '\u2593',
                    DisableBottomPercentage = true
                };

                using (var pbar = new IndeterminateProgressBar("Enumerating Files.", options))
                {
                    while (!done)
                    {
                        Thread.Sleep(_sleepDelay);
                        pbar.Message = $"Enumerating Files. {fileQueue.Count} Discovered.";
                    }

                    pbar.Message = $"Enumerating Files. {fileQueue.Count} Discovered.";
                    pbar.Finished();
                }

                Console.WriteLine();
                done = false;

                var options2 = new ProgressBarOptions
                {
                    ForegroundColor = ConsoleColor.Yellow,
                    ForegroundColorDone = ConsoleColor.DarkGreen,
                    BackgroundColor = ConsoleColor.DarkGray,
                    BackgroundCharacter = '\u2593',
                    DisableBottomPercentage = false,
                    ShowEstimatedDuration = true
                };

                using (var progressBar = new ProgressBar(fileQueue.Count, $"Analyzing Files.", options2))
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    _ = Task.Factory.StartNew(() =>
                    {
                        DoProcessing(fileQueue);
                        done = true;
                    });

                    while (!done)
                    {
                        Thread.Sleep(_sleepDelay);
                        var current = _metaDataHelper.AbacusRecords.Count;
                        var timePerRecord = sw.Elapsed.TotalMilliseconds / current;
                        var millisExpected = (int)(timePerRecord * (fileQueue.Count - current));
                        var timeExpected = new TimeSpan(0, 0, 0, 0, millisExpected);
                        progressBar.Tick(_metaDataHelper.AbacusRecords.Count, timeExpected, $"Analyzing Files. {_metaDataHelper.AbacusRecords.SelectMany(x => x.Metrics.Matches).Count()} Matches. {_metaDataHelper.AbacusRecords.Count(x => x.File.Status == ScanState.Skipped)} Files Skipped. {_metaDataHelper.AbacusRecords.Count(x => x.File.Status == ScanState.TimedOut)} Timed Out. {_metaDataHelper.AbacusRecords.Count(x => x.File.Status == ScanState.Affected)} Affected. {_metaDataHelper.AbacusRecords.Count(x => x.File.Status == ScanState.Analyzed)} Not Affected.");
                    }

                    progressBar.Message = $"{_metaDataHelper.AbacusRecords.SelectMany(x => x.Metrics.Matches).Count()} Matches. {_metaDataHelper.AbacusRecords.Count(x => x.File.Status == ScanState.Skipped)} Files Skipped. {_metaDataHelper.AbacusRecords.Count(x => x.File.Status == ScanState.TimedOut)} Timed Out. {_metaDataHelper.AbacusRecords.Count(x => x.File.Status == ScanState.Affected)} Affected. {_metaDataHelper.AbacusRecords.Count(x => x.File.Status == ScanState.Analyzed)} Not Affected.";
                    progressBar.Tick(progressBar.MaxTicks);
                }

                Console.WriteLine();
            }
            else
            {
                DoProcessing(GetFileEntries());
            }

            //wrapup result status
            if (!_options.NoFileMetadata
                && _metaDataHelper.AbacusRecords.All(x => x.File.Status == ScanState.Skipped))
            {
                WriteOnce.Error(MsgHelp.GetString(MsgHelp.ID.ANALYZE_NOSUPPORTED_FILETYPES));
                analyzeResult.ResultCode = MtAnalyzeResult.ExitCode.NoMatches;
            }
            else if (_metaDataHelper.UniqueTagsCount == 0)
            {
                WriteOnce.Error(MsgHelp.GetString(MsgHelp.ID.ANALYZE_NOPATTERNS));
                analyzeResult.ResultCode = MtAnalyzeResult.ExitCode.NoMatches;
            }
            else if (_metaDataHelper != null && _metaDataHelper.Metadata != null)
            {
                _metaDataHelper.Metadata.DateScanned = DateScanned.ToString();
                _metaDataHelper.PrepareReport();
                analyzeResult.Metadata = _metaDataHelper.Metadata; //replace instance with metadatahelper processed one
                analyzeResult.ResultCode = MtAnalyzeResult.ExitCode.Success;
            }

            if (timedOut)
            {
                analyzeResult.ResultCode = MtAnalyzeResult.ExitCode.TimedOut;
            }

            return analyzeResult;

            void DoProcessing(IEnumerable<FileEntry> fileEntries)
            {
                if (_options.ProcessingTimeOut > 0)
                {
                    using var cts = new CancellationTokenSource();

                    var t = Task.Run(
                        () => PopulateRecords(
                            cts.Token,
                            _options,
                            fileEntries
                        ),
                        cts.Token
                    );

                    if (!t.Wait(new TimeSpan(0, 0, 0, 0, _options.ProcessingTimeOut)))
                    {
                        timedOut = true;
                        WriteOnce.Error($"Processing timed out.");
                        cts.Cancel();

                        if (_metaDataHelper is not null)
                        {
                            // Populate skips for all the entries we didn't process
                            foreach (var entry in fileEntries.Where(x => !_metaDataHelper.AbacusRecords.Any(y => x.FullPath == y.File.FileName)))
                            {
                                _metaDataHelper.AbacusRecords.Add(
                                    new AbacusRecord(
                                        new FileRecord()
                                        {
                                            AccessTime = entry.AccessTime,
                                            CreateTime = entry.CreateTime,
                                            ModifyTime = entry.ModifyTime,
                                            FileName = entry.FullPath,
                                            Status = ScanState.TimeOutSkipped
                                        }
                                    )
                                );
                            }
                        }
                    }
                }
                else
                {
                    PopulateRecords(
                        new CancellationToken(),
                        _options,
                        fileEntries
                    );
                }
            }
        }
    }

}