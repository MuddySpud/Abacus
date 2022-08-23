// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace MuddySpud.RulesEngine
{
    /// <summary>
    /// QuoteEscape class to hold information about escaping quotes in strings for each language
    /// </summary>
    internal class StringQuote
    {
        [JsonProperty(PropertyName = "language")]
        public string[]? Languages { get; set; }

        [JsonProperty(PropertyName = "quote")]
        public char? Quote { get; set; }

        [JsonProperty(PropertyName = "phrasePrefix")]
        public char? PhrasePrefix { get; set; }

        [JsonProperty(PropertyName = "characterEscape")]
        public StringEscape? CharacterEscape { get; set; }

        [JsonProperty(PropertyName = "phraseCharacterEscape")]
        public StringEscape? PhraseCharacterEscape { get; set; }

        [JsonProperty(PropertyName = "formatPhrasePrefix")]
        public char? FormatPhrasePrefix { get; set; }

        [JsonProperty(PropertyName = "formatPrefix")]
        public char? FormatPrefix { get; set; }

        [JsonProperty(PropertyName = "formatPrefixEscape")]
        public StringEscape? FormatPrefixEscape { get; set; }

        [JsonProperty(PropertyName = "formatSufffix")]
        public char? FormatSufffix { get; set; }

        [JsonProperty(PropertyName = "formatSufffixEscape")]
        public StringEscape? FormatSufffixEscape { get; set; }

        [JsonProperty(PropertyName = "useFormatPrefixEscapeAsDefault")]
        public bool? UseFormatPrefixEscapeAsDefault { get; set; }
    }
}