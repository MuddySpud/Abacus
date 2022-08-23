using Newtonsoft.Json;
using System;

namespace MuddySpud.Metrics.Engine
{
    public class TagCounter
    {
        //[JsonProperty(PropertyName = "tag")]
        [JsonIgnore]
        public string Tag { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        public TagCounter()
        { }

        public TagCounter(
            string tag,
            int count)
        {
            Tag = tag;
            Count = count;
        }
    }
}
