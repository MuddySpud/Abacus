using Microsoft.ApplicationInspector.Commands;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddySpud.Metrics.Engine
{
    public class AbacusRecord
    {
        [JsonProperty(PropertyName = "file")]
        public FileRecord File { get; set; }

        [JsonProperty(PropertyName = "metrics")]
        public MetricsRecord Metrics { get; set; } = new();

        public AbacusRecord(FileRecord file)
        {
            File = file;
        }
    }
}
