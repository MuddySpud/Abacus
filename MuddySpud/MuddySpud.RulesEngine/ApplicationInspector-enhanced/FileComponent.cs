using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    internal class FileComponent
    {
        [JsonProperty(PropertyName = "id")]
        public string? ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "blockType")]
        public string? BlockType { get; set; }

        [JsonProperty(PropertyName = "printMetrics")]
        public bool? PrintMetrics { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string? Description { get; set; }

        [JsonProperty(PropertyName = "parents")]
        public List<string>? Parents { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty(PropertyName = "qualifiesName")]
        public bool? QualifiesName { get; set; }

        [JsonProperty(PropertyName = "patterns")]
        public List<Pattern> Patterns { get; set; } = new();

        [JsonProperty(PropertyName = "units")]
        public List<FileUnit> Units { get; set; } = new();
    }
}
