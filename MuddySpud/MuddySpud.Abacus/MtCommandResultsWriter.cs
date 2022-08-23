// Copyright (C) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.ApplicationInspector.Commands;
using Microsoft.ApplicationInspector.Common;
using System.IO;

namespace MuddySpud.Abacus
{
    /// <summary>
    /// Common class for command specific writers
    /// </summary>
    public abstract class CommandResultsWriter
    {
        public TextWriter? TextWriter { get; set; }
        public string? OutputFileName { get; set; }

        public abstract void WriteResults(
            Result result, 
            MtCLICommandOptions commandOptions, 
            bool autoClose = true);

        public void FlushAndClose()
        {
            TextWriter?.Flush();
            TextWriter?.Close();
            WriteOnce.TextWriter = null;
        }
    }
}