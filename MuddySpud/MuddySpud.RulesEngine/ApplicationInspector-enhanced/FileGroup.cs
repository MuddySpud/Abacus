using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    internal class FileGroup
    {
        [JsonProperty(PropertyName = "id")]
        public string? ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; }

        [JsonProperty(PropertyName = "keyword")]
        public string? Keyword { get; set; }

        [JsonProperty(PropertyName = "blocks")]
        public List<string>? Blocks { get; set; }

        [JsonProperty(PropertyName = "subBlocks")]
        public List<string>? SubBlocks { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty(PropertyName = "gang")]
        public GroupGang? Gang { get; set; }

        [JsonProperty(PropertyName = "units")]
        public List<GroupUnit>? Units { get; set; }
    }
}