using Newtonsoft.Json;

namespace MuddySpud.RulesEngine
{
    internal class GroupGang
    {
        [JsonProperty(PropertyName = "name")]
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; }

        [JsonProperty(PropertyName = "position")]
        public int? Position { get; set; }
    }
}