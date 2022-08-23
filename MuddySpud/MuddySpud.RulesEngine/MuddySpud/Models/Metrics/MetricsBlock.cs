using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MuddySpud.Metrics.Engine
{
    [DebuggerDisplay("{FullName} - {Signature}")]
    public class MetricsBlock
    {
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; } = string.Empty;

        //[JsonProperty(PropertyName = "content")]
        [JsonIgnore]
        public string Content { get; set; } = string.Empty;

        //[JsonProperty(PropertyName = "errors")]
        [JsonIgnore]
        public List<BlockStatsError> Errors { get; set; } = new();

        [JsonProperty(PropertyName = "partial")]
        public bool Partial { get; set; } = false;

        [JsonProperty(PropertyName = "depth")]
        public int Depth { get; set; }

        //[JsonProperty(PropertyName = "syblings")]
        [JsonIgnore]
        public int SyblingCount { get; set; }

        [JsonProperty(PropertyName = "openIndex")]
        public int OpenIndex { get; set; }

        [JsonProperty(PropertyName = "closeIndex")]
        public int CloseIndex { get; set; }

        [JsonIgnore]
        public int LineStartIndex { get; set; }

        [JsonProperty(PropertyName = "startLocation")]
        public Location BlockStartLocation { get; private set; } = new();

        [JsonProperty(PropertyName = "endLocation")]
        public Location BlockEndLocation { get; set; } = new();

        [JsonProperty(PropertyName = "children")]
        public List<MetricsBlock> ChildBlocks { get; set; } = new();

        //[JsonProperty(PropertyName = "matches")]
        [JsonIgnore]
        public List<MtMatchRecord> Matches { get; set; } = new();

        [JsonIgnore]
        public List<LocationIndex> UnitsOfWork { get; set; } = new();

        [JsonIgnore]
        public List<LineCounts> LineCounts { get; set; } = new();

        [JsonIgnore]
        public List<MtBoundary> StringBoundaries { get; set; } = new();

        [JsonIgnore]
        public MetricsBlock? Parent { get; set; }

        [JsonIgnore]
        public BlockStats? Block { get; set; }

        [JsonProperty(PropertyName = "tagCounts")]
        //[JsonIgnore]
        public List<TagCounter> TagCounts { get; } = new();

        [JsonIgnore]
        public int TagCountsCount => TagCounts.Count;


        public MetricsBlock()
        {
        }

        public void SetBlockStartLocation(
            Location blockStartLocation,
            int openIndex = -1)
        {
            if (openIndex >= 0)
            {
                OpenIndex = openIndex;
            }

            BlockStartLocation = blockStartLocation;
            LineStartIndex = OpenIndex - blockStartLocation.Column;
            LineStartIndex = LineStartIndex < 0 ? 0 : LineStartIndex;
        }
    }
}
