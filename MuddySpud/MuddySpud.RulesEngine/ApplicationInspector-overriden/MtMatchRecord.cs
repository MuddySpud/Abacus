// Copyright (C) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.ApplicationInspector.RulesEngine;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MuddySpud.RulesEngine.Commands
{
    /// <summary>
    /// Represents augmented record of result issue from rules engine
    /// </summary>
    [DebuggerDisplay("{Pattern}")]
    public class MtMatchRecord
    {
        /// <summary>
        /// Force required arg to avoid null rule
        /// </summary>
        /// <param name="rule"></param>
        public MtMatchRecord(Rule rule)
        {
            Rule = rule;
            RuleId = rule.Id;
            RuleName = rule.Name;
            RuleDescription = rule.Description;
            Tags = rule.Tags;
            Severity = rule.Severity;
        }

        [JsonConstructor]
        public MtMatchRecord(
            string ruleId,
            string ruleName)
        {
            RuleId = ruleId;
            RuleName = ruleName;
        }

        [JsonIgnore]
        public Rule? Rule { get; set; }

        /// <summary>
        /// Rule Id found in matching rule
        /// </summary>
        //[JsonProperty(PropertyName = "ruleId")]
        [JsonIgnore]
        public string RuleId { get; set; }

        /// <summary>
        /// Rule name found in matching rule
        /// </summary>
        [JsonProperty(PropertyName = "ruleName")]
        public string RuleName { get; set; }

        /// <summary>
        /// Rule description found in matching rule
        /// </summary>
        //[JsonProperty(PropertyName = "ruleDescription")]
        [JsonIgnore]
        public string? RuleDescription { get; set; }

        /// <summary>
        /// Tags in matching rule
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public string[]? Tags { get; set; }

        /// <summary>
        /// Rule severity
        /// </summary>_rule
        //[JsonProperty(PropertyName = "severity")]
        [JsonIgnore]
        public Severity Severity { get; set; }

        [JsonIgnore]
        public SearchPattern? MatchingPattern { get; set; }

        /// <summary>
        /// Matching pattern found in matching rule
        /// </summary>
        [JsonProperty(PropertyName = "pattern")]
        public string? Pattern
        {
            get { return MatchingPattern?.Pattern; }
        }

        /// <summary>
        /// Pattern confidence in matching rule pattern
        /// </summary>
        //[JsonProperty(PropertyName = "confidence")]
        [JsonIgnore]
        public Confidence Confidence
        {
            get { return MatchingPattern?.Confidence ?? Confidence.Medium; }
        }

        /// <summary>
        /// Pattern type of matching pattern
        /// </summary>
        //[JsonProperty(PropertyName = "type")]
        [JsonIgnore]
        public string? PatternType => MatchingPattern?.PatternType.ToString();

        [JsonIgnore]
        public BlockTextContainer? BlockTextContainer { get; set; }

        /// <summary>
        /// Internal to namespace only
        /// </summary>
        [JsonIgnore]
        public LanguageInfo LanguageInfo { get; set; } = new LanguageInfo();

        /// <summary>
        /// Friendly source type
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        public string? Language => LanguageInfo?.Name;

        /// <summary>
        /// Filename of this match
        /// </summary>
        //[JsonProperty(PropertyName = "fileName")]
        [JsonIgnore]
        public string? FileName { get; set; }

        /// <summary>
        /// Matching text for this record
        /// </summary>
        //[JsonProperty(PropertyName = "sample")]
        [JsonIgnore]
        public string Sample { get; set; } = "";

        /// <summary>
        /// Matching surrounding context text for sample in this record
        /// </summary>
        [JsonProperty(PropertyName = "excerpt")]
        public string Excerpt { get; set; } = "";

        [JsonIgnore]
        public Boundary Boundary { get; set; } = new();

        /// <summary>
        /// Starting line location of the matching text
        /// </summary>
        [JsonProperty(PropertyName = "startLocationLine")]
        public int StartLocationLine { get; set; }

        /// <summary>
        /// Starting column location of the matching text
        /// </summary>
        [JsonProperty(PropertyName = "startLocationColumn")]
        public int StartLocationColumn { get; set; }

        /// <summary>
        /// Ending line location of the matching text
        /// </summary>
        [JsonProperty(PropertyName = "endLocationLine")]
        public int EndLocationLine { get; set; }

        /// <summary>
        /// Ending column of the matching text
        /// </summary>
        [JsonProperty(PropertyName = "endLocationColumn")]
        public int EndLocationColumn { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public MtPatternScope Scope { get; set; }

        [JsonProperty(PropertyName = "startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty(PropertyName = "endIndex")]
        public int EndIndex { get; set; }
    }
}