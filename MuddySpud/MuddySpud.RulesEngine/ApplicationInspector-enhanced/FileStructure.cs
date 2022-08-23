using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    internal class FileStructure
    {
        [JsonProperty(PropertyName = "language")]
        public string[]? Languages { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; }

        [JsonProperty(PropertyName = "nameJoiner")]
        public string? NameJoiner { get; set; }

        [JsonProperty(PropertyName = "components")]
        public List<FileComponent> Components { get; set; } = new();

        [JsonProperty(PropertyName = "groups")]
        public List<FileGroup> Groups { get; set; } = new();

        [JsonProperty(PropertyName = "unmasking")]
        public List<UnMask> UnMasking { get; set; } = new();

        [JsonProperty(PropertyName = "unitsOfWork")]
        public UnitsOfWork UnitsOfWork { get; set; } = new();
    }
}