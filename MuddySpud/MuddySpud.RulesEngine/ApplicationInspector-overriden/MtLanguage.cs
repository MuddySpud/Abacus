// Copyright (C) Microsoft. All rights reserved. Licensed under the MIT License.

using Microsoft.ApplicationInspector.RulesEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MuddySpud.RulesEngine.Commands
{
    /// <summary>
    /// Helper class for language based commenting
    /// </summary>
    public sealed class MtLanguage
    {
        private static MtLanguage? _instance;
        private readonly List<Comment> _comments;
        private readonly List<LanguageInfo> _languages;
        private readonly List<CodeBlock> _codeBlocks;
        private readonly List<PreProcessor> _preProcessors;
        private readonly List<FileStructure> _fileStructures;
        private readonly List<StringQuote> _stringQuotes;

        private MtLanguage()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Load comments
            using (Stream? resource = assembly.GetManifestResourceStream("MuddySpud.RulesEngine.Resources.comments.json"))
            using (StreamReader file = new(resource ?? new MemoryStream()))
            {
                _comments = JsonConvert.DeserializeObject<List<Comment>>(file.ReadToEnd()) ?? new List<Comment>();
            }

            // Load languages
            using (Stream? resource = assembly.GetManifestResourceStream("MuddySpud.RulesEngine.Resources.languages.json"))
            using (StreamReader file = new(resource ?? new MemoryStream()))
            {
                _languages = JsonConvert.DeserializeObject<List<LanguageInfo>>(file.ReadToEnd()) ?? new List<LanguageInfo>();
            }

            // Load codeBlocks
            using (Stream? resource = assembly.GetManifestResourceStream("MuddySpud.RulesEngine.Resources.codeBlocks.json"))
            using (StreamReader file = new(resource ?? new MemoryStream()))
            {
                string contents = file.ReadToEnd();
                List<CodeBlock>? codeBlocks = JsonConvert.DeserializeObject<List<CodeBlock>>(contents);
                _codeBlocks = codeBlocks ?? new List<CodeBlock>();
            }

            // Load preProcessors
            using (Stream? resource = assembly.GetManifestResourceStream("MuddySpud.RulesEngine.Resources.preProcessors.json"))
            using (StreamReader file = new(resource ?? new MemoryStream()))
            {
                string contents = file.ReadToEnd();
                List<PreProcessor>? preProcessors = JsonConvert.DeserializeObject<List<PreProcessor>>(contents);
                _preProcessors = preProcessors ?? new List<PreProcessor>();
            }

            // Load structure
            using (Stream? resource = assembly.GetManifestResourceStream("MuddySpud.RulesEngine.Resources.structure.json"))
            using (StreamReader file = new(resource ?? new MemoryStream()))
            {
                string contents = file.ReadToEnd();
                List<FileStructure>? fileStructures = JsonConvert.DeserializeObject<List<FileStructure>>(contents);
                _fileStructures = fileStructures ?? new List<FileStructure>();
            }

            // Load stringQuotes
            using (Stream? resource = assembly.GetManifestResourceStream("MuddySpud.RulesEngine.Resources.stringQuotes.json"))
            using (StreamReader file = new(resource ?? new MemoryStream()))
            {
                string contents = file.ReadToEnd();
                List<StringQuote>? stringQuotes = JsonConvert.DeserializeObject<List<StringQuote>>(contents);
                _stringQuotes = stringQuotes ?? new List<StringQuote>();
            }
        }

        /// <summary>
        /// Returns language for given file name
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>MtLanguage</returns>
        public static bool FromFileName(string fileName, ref LanguageInfo info)
        {
            if (fileName == null)
            {
                return false;
            }

            string file = Path.GetFileName(fileName).ToLower(CultureInfo.CurrentCulture);
            string ext = Path.GetExtension(file);

            // Look for whole filename first
            foreach (LanguageInfo item in Instance._languages)
            {
                if (item.Name == file)
                {
                    info = item;

                    return true;
                }
            }

            // Look for extension only ext is defined
            if (!string.IsNullOrEmpty(ext))
            {
                foreach (LanguageInfo item in Instance._languages)
                {
                    if (item.Name == "typescript-config")//special case where extension used for exact match to a single type
                    {
                        if (item.Extensions?.Any(x => x.ToLower().Equals(file)) ?? false)
                        {
                            info = item;
                            return true;
                        }
                    }
                    else if (Array.Exists(item.Extensions ?? Array.Empty<string>(), x => x.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        info = item;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets comment inline for given language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Commented string</returns>
        public static string GetCommentInline(string language)
        {
            string result = string.Empty;

            if (language != null)
            {
                foreach (Comment comment in Instance._comments)
                {
                    if (Array.Exists(comment.Languages ?? new string[] { "" }, x => x.Equals(language, StringComparison.InvariantCultureIgnoreCase)) && comment.Inline is { })
                        return comment.Inline;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets comment preffix for given language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Commented string</returns>
        public static string GetCommentPrefix(string language)
        {
            string result = string.Empty;

            if (language != null)
            {
                foreach (Comment comment in Instance._comments)
                {
                    if ((comment.Languages?.Contains(language.ToLower(CultureInfo.InvariantCulture)) ?? false) && comment.Prefix is { })
                        return comment.Prefix;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets comment suffix for given language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Commented string</returns>
        public static string GetCommentSuffix(string language)
        {
            string result = string.Empty;

            if (language != null)
            {
                foreach (Comment comment in Instance._comments)
                {
                    if (Array.Exists(comment.Languages ?? new string[] { "" }, x => x.Equals(language, StringComparison.InvariantCultureIgnoreCase)) && comment.Suffix is { })
                        return comment.Suffix;
                }
            }

            return result;
        }

        /// <summary>
        /// Get names of all known languages
        /// </summary>
        /// <returns>Returns list of names</returns>
        public static string[] GetNames()
        {
            var names = from x in Instance._languages
                        select x.Name;

            return names.ToArray();
        }

        private static MtLanguage Instance
        {
            get
            {
                return _instance ??= new MtLanguage();
            }
        }

        #region Codemologie 

        public static StringSettings GetStringSettings(string language)
        {
            char quote = Char.MinValue;
            char phrasePrefix = Char.MinValue;
            StringEscape? characterEscape = null;
            StringEscape? phraseCharacterEscape = null;

            char formatPhrasePrefix = Char.MinValue;
            char formatPrefix = Char.MinValue;
            StringEscape? formatPrefixEscape = null;

            char formatSufffix = Char.MinValue;
            StringEscape? formatSufffixEscape = null;
            bool useFormatPrefixEscapeAsDefault = false;

            if (language != null)
            {
                foreach (StringQuote stringQuote in Instance._stringQuotes)
                {
                    bool exists = LanguageExists(
                        stringQuote.Languages ?? new string[] { "" },
                        language);

                    if (exists)
                    {
                        if (stringQuote.Quote.HasValue)
                        {
                            quote = stringQuote.Quote.Value;
                        }

                        if (stringQuote.PhrasePrefix.HasValue)
                        {
                            phrasePrefix = stringQuote.PhrasePrefix.Value;
                        }

                        if (stringQuote.CharacterEscape is { }
                            && stringQuote.CharacterEscape.Escape.HasValue
                            && !String.IsNullOrWhiteSpace(stringQuote.CharacterEscape.Location))
                        {
                            characterEscape = new StringEscape(
                                stringQuote.CharacterEscape.Escape.Value,
                                stringQuote.CharacterEscape.Location);
                        }

                        if (stringQuote.PhraseCharacterEscape is { }
                            && stringQuote.PhraseCharacterEscape.Escape.HasValue
                            && !String.IsNullOrWhiteSpace(stringQuote.PhraseCharacterEscape.Location))
                        {
                            phraseCharacterEscape = new StringEscape(
                                stringQuote.PhraseCharacterEscape.Escape.Value,
                                stringQuote.PhraseCharacterEscape.Location);
                        }

                        if (stringQuote.FormatPhrasePrefix.HasValue)
                        {
                            formatPhrasePrefix = stringQuote.FormatPhrasePrefix.Value;
                        }

                        if (stringQuote.FormatPrefix.HasValue)
                        {
                            formatPrefix = stringQuote.FormatPrefix.Value;
                        }

                        if (stringQuote.FormatPrefixEscape is { }
                            && stringQuote.FormatPrefixEscape.Escape.HasValue
                            && !String.IsNullOrWhiteSpace(stringQuote.FormatPrefixEscape.Location))
                        {
                            formatPrefixEscape = new StringEscape(
                                stringQuote.FormatPrefixEscape.Escape.Value,
                                stringQuote.FormatPrefixEscape.Location);
                        }

                        if (stringQuote.FormatSufffix.HasValue)
                        {
                            formatSufffix = stringQuote.FormatSufffix.Value;
                        }

                        if (stringQuote.FormatSufffixEscape is { }
                            && stringQuote.FormatSufffixEscape.Escape.HasValue
                            && !String.IsNullOrWhiteSpace(stringQuote.FormatSufffixEscape.Location))
                        {
                            formatSufffixEscape = new StringEscape(
                                stringQuote.FormatSufffixEscape.Escape.Value,
                                stringQuote.FormatSufffixEscape.Location);
                        }

                        if (stringQuote.UseFormatPrefixEscapeAsDefault.HasValue)
                        {
                            useFormatPrefixEscapeAsDefault = stringQuote.UseFormatPrefixEscapeAsDefault.Value;
                        }
                    }
                }
            }

            if (characterEscape is { }
                && phraseCharacterEscape is { }
                && formatPrefixEscape is { }
                && formatSufffixEscape is { })
            {
                return new StringSettings(
                    quote,
                    phrasePrefix,
                    characterEscape,
                    phraseCharacterEscape,
                    formatPhrasePrefix,
                    formatPrefix,
                    formatPrefixEscape,
                    formatSufffix,
                    formatSufffixEscape,
                    useFormatPrefixEscapeAsDefault
                );
            }

            throw new Exception("Could not create StringSettings...");
        }

        public static CodeBlockSettings GetCodeBlockSettings(string language)
        {
            CodeBlockSettings codeBlockSettings = new();
            HashSet<string> diffChecker = new HashSet<string>();
            bool isDifferent;

            if (language != null)
            {
                foreach (CodeBlock codeBlock in Instance._codeBlocks)
                {
                    bool exists = LanguageExists(
                        codeBlock.Languages ?? new string[] { "" },
                        language);

                    if (exists)
                    {
                        if (codeBlock.Boundaries is { })
                        {
                            BlockBoundarySettings blockBoundarySettings;

                            foreach (BlockBoundary blockBoundary in codeBlock.Boundaries)
                            {
                                if (blockBoundary.Open is { }
                                    && blockBoundary.Close is { }
                                    && blockBoundary.Type is { }
                                    && blockBoundary.Name is { }
                                    && blockBoundary.SearchType is { }
                                    && blockBoundary.ExplicitClose.HasValue)
                                {
                                    isDifferent = diffChecker.Add(blockBoundary.Open);

                                    if (isDifferent)
                                    {
                                        blockBoundarySettings = new(
                                            blockBoundary.Name,
                                            blockBoundary.Type,
                                            blockBoundary.Open,
                                            blockBoundary.Close,
                                            blockBoundary.SearchType,
                                            blockBoundary.ExplicitClose.Value
                                        );
                                    }
                                    else
                                    {
                                        throw new NotImplementedException($"BlockBoundary.Open: {blockBoundary.Open} as already been added, it must be unique.");
                                    }

                                    codeBlockSettings.AllBoundaries.Add(blockBoundarySettings);

                                    if (codeBlock.enabledNames is { }
                                        && codeBlock.enabledNames.Contains(blockBoundary.Name))
                                    {
                                        codeBlockSettings.Boundaries.Add(blockBoundarySettings);
                                    }
                                }
                            }
                        }

                        if (codeBlock.Statements is { })
                        {
                            StatementBoundarySettings statementSettings;

                            foreach (Statement statement in codeBlock.Statements)
                            {
                                if (statement.Enabled != true)
                                {
                                    continue;
                                }

                                if (statement.Close is { }
                                    && statement.Type is { }
                                    && statement.Name is { }
                                    && statement.SearchType is { })
                                {
                                    statementSettings = new(
                                        statement.Name,
                                        statement.Type,
                                        statement.Close,
                                        statement.SearchType);

                                    codeBlockSettings.Statements.Add(statementSettings);
                                }
                            }
                        }

                        if (codeBlock.Mask is { }
                            && codeBlock.Mask.Enabled == true)
                        {
                            MaskSettings maskSettings;

                            List<FacetSettings> facets = BuildFacets(codeBlock.Mask.Facets);
                            List<string> boundaries = BuildBoundaries(codeBlock.Mask.Boundaries);

                            if (facets.Count > 0
                                && boundaries.Count > 0)
                            {
                                maskSettings = new(
                                    boundaries,
                                    facets);

                                codeBlockSettings.Mask = maskSettings;
                            }
                        }
                    }
                }
            }

            return codeBlockSettings;
        }

        public static PreProcessorSettings GetPreProcessorSettings(string language)
        {
            PreProcessorSettings preProcessorSettings = new();

            if (language != null)
            {
                foreach (PreProcessor preProcessor in Instance._preProcessors)
                {
                    bool exists = LanguageExists(
                        preProcessor.Languages ?? new string[] { "" },
                        language);

                    if (exists)
                    {
                        if (preProcessor.Preprocessors is { })
                        {
                            foreach (string preprocessor in preProcessor.Preprocessors)
                            {
                                if (String.IsNullOrWhiteSpace(preprocessor))
                                {
                                    continue;
                                }

                                if (!preProcessorSettings.Preprocessors.Contains(preprocessor))
                                {
                                    preProcessorSettings.Preprocessors.Add(preprocessor);
                                }
                            }
                        }
                    }
                }
            }

            return preProcessorSettings;
        }

        public static FileStructureSettings GetStructureSettings(string language)
        {
            if (language is null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            FileStructureSettings? structureSettings = null;

            foreach (FileStructure fileStructure in Instance._fileStructures)
            {
                bool exists = LanguageExists(
                    fileStructure.Languages ?? new string[] { "" },
                    language
                );

                if (exists)
                {
                    if (fileStructure.Type is null)
                    {
                        throw new ArgumentNullException(nameof(fileStructure.Type));
                    }

                    string joiner = String.IsNullOrWhiteSpace(fileStructure.NameJoiner) ? String.Empty : fileStructure.NameJoiner;

                    structureSettings = new(
                        language,
                        fileStructure.Type,
                        joiner
                    );

                    LoadComponents(
                        fileStructure.Components,
                        structureSettings
                    );

                    LoadGroups(
                        fileStructure.Groups,
                        structureSettings
                    );

                    LoadUnMasking(
                        fileStructure.UnMasking,
                        structureSettings
                    );

                    LoadUnitOfWork(
                        fileStructure.UnitsOfWork,
                        structureSettings
                    );

                    break;
                }
            }

            if (structureSettings is null)
            {
                throw new ArgumentNullException(nameof(structureSettings));
            }

            return structureSettings;
        }

        private static void LoadUnitOfWork(
            UnitsOfWork unitsOfWork,
            FileStructureSettings structureSettings)
        {
            if (unitsOfWork.Enabled != true)
            {
                return;
            }

            if (unitsOfWork.NonBlockPatterns?.Count > 0)
            {
                LoadPatterns(
                    unitsOfWork.NonBlockPatterns,
                    structureSettings.UnitsOfWork.NonBlockPatterns
                );
            }
        }

        private static void LoadUnMasking(
            List<UnMask> unMasking,
            FileStructureSettings structureSettings)
        {
            UnMaskSettings? unMaskSettings;

            foreach (UnMask unMask in unMasking)
            {
                if (!String.IsNullOrWhiteSpace(unMask?.GroupName)
                    && unMask.Enabled == true
                    && !String.IsNullOrWhiteSpace(unMask.ActionName))
                {
                    unMaskSettings = new(
                        unMask.GroupName,
                        unMask.ActionName
                    );

                    structureSettings.UnMasking.Add(unMaskSettings);
                }
            }
        }

        private static void LoadGroups(
            List<FileGroup> groups,
            FileStructureSettings structureSettings)
        {
            GroupSettings? groupSettings;

            foreach (FileGroup fileGroup in groups)
            {
                if (!String.IsNullOrWhiteSpace(fileGroup?.Name)
                    && fileGroup.Enabled == true
                    && !String.IsNullOrWhiteSpace(fileGroup.ID)
                    && !String.IsNullOrWhiteSpace(fileGroup.Type)
                    && !String.IsNullOrWhiteSpace(fileGroup.Keyword))
                {
                    groupSettings = new(
                        fileGroup.ID,
                        fileGroup.Name,
                        fileGroup.Type,
                        fileGroup.Keyword
                    );

                    LoadBlocks(
                        fileGroup,
                        groupSettings
                    );

                    LoadSubBlocks(
                        fileGroup,
                        groupSettings
                    );

                    LoadGroupGang(
                        fileGroup.Gang,
                        groupSettings
                    );

                    LoadGroupUnits(
                        fileGroup,
                        groupSettings
                    );

                    structureSettings.Groups.Add(groupSettings);
                }
            }
        }

        private static void LoadGroupUnits(
            FileGroup fileGroup,
            GroupSettings groupSettings)
        {
            if (fileGroup.Units is { })
            {
                GroupUnitSettings? groupUnitSettings;

                foreach (GroupUnit unit in fileGroup.Units)
                {
                    if (!String.IsNullOrWhiteSpace(unit?.Name)
                        && !String.IsNullOrWhiteSpace(unit.Type)
                        && !String.IsNullOrWhiteSpace(unit.SearchType))
                    {
                        groupUnitSettings = new(
                            unit.Name,
                            unit.Type,
                            unit.SearchType
                        );

                        if (unit.Patterns?.Count > 0)
                        {
                            LoadPatterns(
                                unit.Patterns,
                                groupUnitSettings.Patterns
                            );
                        }

                        groupSettings.Units.Add(groupUnitSettings);
                    }
                }
            }
        }

        private static void LoadComponents(
            List<FileComponent> components,
            FileStructureSettings structureSettings)
        {
            ComponentSettings? componentSettings;

            foreach (FileComponent fileComponent in components)
            {
                if (!String.IsNullOrWhiteSpace(fileComponent?.Name)
                    && fileComponent.Enabled == true
                    && !String.IsNullOrWhiteSpace(fileComponent.ID)
                    && !String.IsNullOrWhiteSpace(fileComponent.BlockType))
                {
                    componentSettings = new(
                        fileComponent.ID,
                        fileComponent.Name,
                        fileComponent.BlockType,
                        fileComponent.PrintMetrics);

                    LoadParents(
                        fileComponent,
                        componentSettings);

                    LoadDescription(
                        fileComponent,
                        componentSettings);

                    componentSettings.QualifiesName = fileComponent.QualifiesName == true;

                    LoadUnits(
                        fileComponent,
                        componentSettings);

                    if (fileComponent.Patterns?.Count > 0)
                    {
                        LoadPatterns(
                            fileComponent.Patterns,
                            componentSettings.Patterns
                        );
                    }

                    structureSettings.Components.Add(componentSettings);
                }
            }
        }

        private static void LoadGroupGang(
            GroupGang? gangGroup,
            GroupSettings groupSettings)
        {
            if (gangGroup is null)
            {
                return;
            }

            GroupGangSettings? groupGangSettings;

            if (!String.IsNullOrWhiteSpace(gangGroup?.Name)
                && !String.IsNullOrWhiteSpace(gangGroup.Type)
                && gangGroup.Position.HasValue)
            {
                groupGangSettings = new(
                    gangGroup.Name,
                    gangGroup.Type,
                    gangGroup.Position.Value
                );

                groupSettings.Gang = groupGangSettings;
            }
        }

        private static void LoadDescription(
            FileComponent fileComponent,
            ComponentSettings componentSettings)
        {
            if (fileComponent.Description is { }
                && !String.IsNullOrWhiteSpace(fileComponent.Description))
            {
                componentSettings.Description = fileComponent.Description;
            }
        }

        private static void LoadParents(
            FileComponent fileComponent,
            ComponentSettings componentSettings)
        {
            if (fileComponent.Parents is { })
            {
                foreach (string parentName in fileComponent.Parents)
                {
                    if (!String.IsNullOrWhiteSpace(parentName))
                    {
                        componentSettings.Parents.Add(parentName);
                    }
                }
            }
        }

        private static void LoadBlocks(
            FileGroup fileGroup,
            GroupSettings groupSettings)
        {
            if (fileGroup.Blocks is { })
            {
                foreach (string block in fileGroup.Blocks)
                {
                    if (!String.IsNullOrWhiteSpace(block))
                    {
                        groupSettings.Blocks.Add(block);
                    }
                }
            }
        }

        private static List<string> BuildBoundaries(
            List<string>? boundaries)
        {
            List<string> settings = new();

            if (boundaries is { }
                && boundaries.Count > 0)
            {
                foreach (string boundary in boundaries)
                {
                    if (!String.IsNullOrWhiteSpace(boundary))
                    {
                        settings.Add(boundary);
                    }
                }
            }

            return settings;
        }

        private static List<FacetSettings> BuildFacets(
            List<Facet>? facets)
        {
            List<FacetSettings> settings = new();
            FacetSettings facetSettings;

            if (facets is { }
                && facets.Count > 0)
            {
                foreach (Facet facet in facets)
                {
                    if (facet is { }
                        && facet.Enabled == true
                        && !String.IsNullOrWhiteSpace(facet.Name)
                        && !String.IsNullOrWhiteSpace(facet.BlockType)
                        && facet.Patterns is { }
                        && facet.Patterns.Count > 0)
                    {
                        facetSettings = new(
                            facet.Name,
                            facet.BlockType
                        );

                        LoadPatterns(
                            facet.Patterns,
                            facetSettings.Patterns
                        );

                        settings.Add(facetSettings);
                    }
                }
            }

            return settings;
        }

        private static void LoadSubBlocks(
            FileGroup fileGroup,
            GroupSettings groupSettings)
        {
            if (fileGroup.SubBlocks is { })
            {
                foreach (string subBlock in fileGroup.SubBlocks)
                {
                    if (!String.IsNullOrWhiteSpace(subBlock))
                    {
                        groupSettings.SubBlocks.Add(subBlock);
                    }
                }
            }
        }

        private static void LoadUnits(
            FileComponent fileComponent,
            ComponentSettings componentSettings)
        {
            UnitSettings? unitSettings;

            foreach (FileUnit fileUnit in fileComponent.Units)
            {
                if (!String.IsNullOrWhiteSpace(fileUnit.Name)
                    && !String.IsNullOrWhiteSpace(fileUnit.BlockType)
                    && fileUnit.Enabled == true)
                {
                    unitSettings = new(
                        fileUnit.Name,
                        fileUnit.BlockType,
                        fileUnit.PrintMetrics);

                    LoadPatterns(
                        fileUnit.Patterns,
                        unitSettings.Patterns
                    );

                    componentSettings.Units.Add(unitSettings);
                }
            }
        }

        private static void LoadPatterns(
            IEnumerable<Pattern> readPatterns,
            List<PatternSettings> writePatterns)
        {
            PatternSettings? patternSettings;

            foreach (Pattern pattern in readPatterns)
            {

                if (pattern.RegexPattern is { }
                    && !String.IsNullOrWhiteSpace(pattern.RegexPattern))
                {
                    patternSettings = new(pattern.RegexPattern);
                    writePatterns.Add(patternSettings);

                    if (pattern.RejectMatchRegexPattern is { }
                        && !String.IsNullOrWhiteSpace(pattern.RejectMatchRegexPattern))
                    {
                        patternSettings.RejectMatchRegexPattern = pattern.RejectMatchRegexPattern;
                    }
                }
            }
        }

        public static CommentSettings GetCommentSettings(string language)
        {
            string inline = String.Empty;
            string prefix = String.Empty;
            string suffix = String.Empty;

            if (language != null)
            {
                foreach (Comment comment in Instance._comments)
                {
                    bool exists = LanguageExists(
                        comment.Languages ?? new string[] { "" },
                        language);

                    if (exists)
                    {
                        if (comment.Inline is { })
                        {
                            inline = comment.Inline;
                        }

                        if (comment.Prefix is { })
                        {
                            prefix = comment.Prefix;
                        }

                        if (comment.Suffix is { })
                        {
                            suffix = comment.Suffix;
                        }
                    }
                }
            }

            return new CommentSettings(
                inline,
                prefix,
                suffix
            );
        }

        private static bool LanguageExists(
            string[] languages,
            string language)
        {
            return Array.Exists(
                languages,
                x => x.Equals(
                    language,
                    StringComparison.InvariantCultureIgnoreCase
                )
            );
        }

        #endregion
    }
}