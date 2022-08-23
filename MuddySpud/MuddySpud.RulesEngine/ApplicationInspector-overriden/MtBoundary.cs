// Copyright (C) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.Diagnostics;

namespace MuddySpud.RulesEngine.Commands
{
    /// <summary>
    /// Class represents boundary in text
    /// </summary>
    [DebuggerDisplay("{Index}-{Length}")]
    public class MtBoundary
    {
        public int Index { get; set; }
        public int LineNumber { get; set; }
        public int Length { get; }
        public string Type { get; }

        public MtBoundary(
            int index,
            int length,
            string type)
        {
            Index = index;
            Length = length;
            Type = type;
        }

        public MtBoundary(
            int index,
            int length,
            string type,
            int lineNumber)
        {
            Index = index;
            Length = length;
            Type = type;
            LineNumber = lineNumber;
        }
    }
}