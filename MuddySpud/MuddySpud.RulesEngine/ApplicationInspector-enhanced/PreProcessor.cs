// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    /// <summary>
    /// Code block class to hold information about code blocks for each language
    /// </summary>
    internal class PreProcessor
    {
        [JsonProperty(PropertyName = "language")]
        public string[]? Languages { get; set; }

        [JsonProperty(PropertyName = "preprocessors")]
        public List<string>? Preprocessors { get; set; }
    }
}