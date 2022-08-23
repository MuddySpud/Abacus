﻿// Copyright (C) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics;

namespace Microsoft.ApplicationInspector.RulesEngine
{
    /// <summary>
    /// Class represents boundary in text
    /// </summary>
    [DebuggerDisplay("{Index}-{Length}")]
    public class Boundary
    {
        /// <summary>
        /// Starting position
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Length of boundary
        /// </summary>
        public int Length { get; set; }
    }
}