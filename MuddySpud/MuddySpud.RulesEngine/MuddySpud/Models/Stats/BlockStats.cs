using Microsoft.ApplicationInspector.RulesEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace MuddySpud.RulesEngine
{
    [DebuggerDisplay("({BlockStartLocation.Line}-{BlockStartLocation.Column}) => ({BlockEndLocation.Line}-{BlockEndLocation.Column}) - {Settings.Model} -{Settings.BlockType} - {Name} - {Type}")]
    public class BlockStats
    {
        private int _closeIndex;

        [JsonIgnore]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "errors")]
        public List<BlockStatsError> Errors { get; set; } = new();

        [JsonIgnore]
        public bool PrintMetrics { get; set; } = false;

        [JsonIgnore]
        public string Signature { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "signature")]
        public string CleanedSignature { get; set; } = string.Empty;

        [JsonIgnore]
        public string Value { get; set; } = string.Empty;

        [JsonIgnore]
        public string Content { get; set; } = string.Empty;

        [JsonIgnore]
        public string StrippedContent { get; set; } = string.Empty;

        public IBoundarySettings Settings { get; set; }

        [JsonIgnore]
        public int MatchStart { get; set; }

        [JsonIgnore]
        public int MatchEnd { get; set; }

        [JsonIgnore]
        public int AdjustedMatchStart { get; set; }

        [JsonIgnore]
        public int AdjustedMatchEnd { get; set; }

        [JsonIgnore]
        public Location MatchStartLocation { get; set; } = new();

        [JsonIgnore]
        public Location MatchEndLocation { get; set; } = new();

        [JsonProperty(PropertyName = "flags")]
        public List<string> Flags { get; } = new();

        [JsonIgnore]
        public bool QualifiesName { get; set; } = false;

        [JsonProperty(PropertyName = "depth")]
        public int Depth { get; set; }

        [JsonProperty(PropertyName = "syblingCount")]
        public int SyblingCount { get; set; }

        [JsonIgnore]
        public int OpenIndex { get; set; }

        [JsonIgnore]
        public bool LinkedToMetrics { get; set; }

        [JsonProperty(PropertyName = "startIndex")]
        public int AdjustedOpenIndex { get; set; }

        [JsonProperty(PropertyName = "endIndex")]
        public int AdjustedCloseIndex { get; set; }

        [JsonProperty(PropertyName = "startLocation")]
        public Location BlockStartLocation { get; set; } = new();

        [JsonProperty(PropertyName = "endLocation")]
        public Location BlockEndLocation { get; set; } = new();

        [JsonIgnore]
        public List<BlockStats> ChildBlocks { get; set; } = new();

        [JsonIgnore]
        public List<StatementStats> ChildStatements { get; set; } = new();

        [JsonIgnore]
        public BlockStats? Parent { get; set; }

        [JsonIgnore]
        public int CloseIndex {
            get => _closeIndex;
            set
            {
                _closeIndex = value;

                if (_closeIndex < OpenIndex)
                {
                    Errors.Add(new BlockStatsError("UnclosedBlock", "There is a block without a closeIndex, this could corrupt all the results."));
                }
            }
        }

        public BlockStats(
            BlockStats? parent,
            int openIndex,
            int depth,
            IBoundarySettings settings)
        {
            Parent = parent;
            OpenIndex = openIndex;
            Depth = depth;
            Settings = settings;
        }
    }
}