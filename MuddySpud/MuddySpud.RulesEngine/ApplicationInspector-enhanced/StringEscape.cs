// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace MuddySpud.RulesEngine
{
    /// <summary>
    /// QuoteEscape class to hold information about escaping quotes in strings for each language
    /// </summary>
    public class StringEscape
    {
        [JsonProperty(PropertyName = "escape")]
        public char? Escape { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string? Location { get; set; }

        public StringEscape(
            char escape,
            string location)
        {
            Escape = escape;
            Location = location;
        }
    }
}