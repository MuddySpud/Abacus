// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    public class CodeBlockSettings
    {
        public List<BlockBoundarySettings> AllBoundaries { get; } = new();
        public List<BlockBoundarySettings> Boundaries { get; } = new();
        public List<StatementBoundarySettings> Statements { get; } = new();
        public MaskSettings? Mask { get; set; } = null;

        public CodeBlockSettings()
        {
        }
    }
}
