// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace MuddySpud.RulesEngine
{
    internal class Statement
    {
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; }

        [JsonProperty(PropertyName = "close")]
        public string? Close { get; set; }

        [JsonProperty(PropertyName = "searchType")]
        public string? SearchType { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled { get; set; }
    }
}