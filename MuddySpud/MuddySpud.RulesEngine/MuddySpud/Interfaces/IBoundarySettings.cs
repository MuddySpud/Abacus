using Newtonsoft.Json;

namespace MuddySpud.RulesEngine
{
    public interface IBoundarySettings
    {
        [JsonProperty(PropertyName = "name")]
        string Name { get; }

        [JsonProperty(PropertyName = "model")]
        string Model { get; }

        [JsonProperty(PropertyName = "type")]
        string BlockType { get; }

        [JsonProperty(PropertyName = "open")]
        string Open { get; }

        [JsonProperty(PropertyName = "close")]
        string Close { get; }

        [JsonIgnore]
        string SearchType { get; }

        [JsonIgnore]
        bool ExplicitClose { get; }
    }
}