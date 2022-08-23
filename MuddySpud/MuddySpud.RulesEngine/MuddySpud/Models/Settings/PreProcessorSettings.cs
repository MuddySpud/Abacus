// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    public class PreProcessorSettings
    {
        public List<string> Preprocessors { get; } = new();

        public PreProcessorSettings()
        {
        }
    }
}