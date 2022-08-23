// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace MuddySpud.RulesEngine
{
    /// <summary>
    /// Comment class to hold information about comment for each language
    /// </summary>
    public class StringSettings
    {
        public char Quote { get; }
        public char PhrasePrefix { get; }
        public StringEscape CharacterEscape { get; }
        public StringEscape PhraseCharacterEscape { get; }
        public char FormatPhrasePrefix { get; }
        public char FormatPrefix { get; }
        public StringEscape FormatPrefixEscape { get; }
        public char FormatSuffix { get; }
        public StringEscape FormatSuffixEscape { get; }
        public bool UseFormatPrefixEscapeAsDefault { get; }

        public StringSettings(
            char quote,
            char phrasePrefix,
            StringEscape characterEscape,
            StringEscape phraseCharacterEscape,
            char formatPhrasePrefix,
            char formatPrefix,
            StringEscape formatPrefixEscape,
            char formatSufffix,
            StringEscape formatSufffixEscape,
            bool useFormatPrefixEscapeAsDefault = false)
        {
            Quote = quote;
            PhrasePrefix = phrasePrefix;
            CharacterEscape = characterEscape;
            PhraseCharacterEscape = phraseCharacterEscape;

            FormatPhrasePrefix = formatPhrasePrefix;
            FormatPrefix = formatPrefix;
            FormatPrefixEscape = formatPrefixEscape;
            FormatSuffix = formatSufffix;
            FormatSuffixEscape = formatSufffixEscape;
            UseFormatPrefixEscapeAsDefault = useFormatPrefixEscapeAsDefault;
        }
    }
}