// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    internal class Mask
    {

        [JsonProperty(PropertyName = "boundaries")]
        public List<string>? Boundaries { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool? Enabled { get; set; }

        [JsonProperty(PropertyName = "facets")]
        public List<Facet>? Facets { get; set; }
    }
}