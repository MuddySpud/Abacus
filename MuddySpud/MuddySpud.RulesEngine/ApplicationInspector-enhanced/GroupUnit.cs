using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    internal class GroupUnit
    {
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; }

        [JsonProperty(PropertyName = "searchType")]
        public string? SearchType { get; set; }

        [JsonProperty(PropertyName = "patterns")]
        public List<Pattern> Patterns { get; set; } = new();
    }
}