
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    internal class FileUnit
    {
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty(PropertyName = "patterns")]
        public List<Pattern> Patterns { get; set; } = new();

        [JsonProperty(PropertyName = "blockType")]
        public string? BlockType { get; set; }

        [JsonProperty(PropertyName = "printMetrics")]
        public bool? PrintMetrics { get; set; }
    }
}