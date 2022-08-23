// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    /// <summary>
    /// Code block class to hold information about code blocks for each language
    /// </summary>
    internal class CodeBlock
    {
        [JsonProperty(PropertyName = "language")]
        public string[]? Languages { get; set; }

        [JsonProperty(PropertyName = "boundaries")]
        public List<BlockBoundary>? Boundaries { get; set; }

        [JsonProperty(PropertyName = "enabledNames")]
        public List<string>? enabledNames { get; set; }

        [JsonProperty(PropertyName = "statements")]
        public List<Statement>? Statements { get; set; }

        [JsonProperty(PropertyName = "mask")]
        public Mask? Mask { get; set; }
    }
}