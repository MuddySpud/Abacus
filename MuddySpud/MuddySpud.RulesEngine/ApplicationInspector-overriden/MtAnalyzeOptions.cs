// Copyright (C) Microsoft. All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using GlobExpressions;
using Microsoft.ApplicationInspector.RulesEngine;
using Microsoft.CST.RecursiveExtractor;
using Newtonsoft.Json;
using NLog;
using ShellProgressBar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInspector.Common;

namespace Microsoft.ApplicationInspector.Commands
{
    /// <summary>
    /// Options specific to analyze operation not to be confused with CLIAnalyzeCmdOptions which include CLI only args
    /// </summary>
    public class MtAnalyzeOptions : LogOptions
    {
        public IEnumerable<string> SourcePath { get; set; } = Array.Empty<string>();
        public string? CustomRulesPath { get; set; }
        public bool IgnoreDefaultRules { get; set; }
        public string ConfidenceFilters { get; set; } = "high,medium";
        public IEnumerable<string> FilePathExclusions { get; set; } = Array.Empty<string>();
        public IEnumerable<string> WhiteListExtensions { get; set; } = Array.Empty<string>();
        public bool SingleThread { get; set; } = false;
        public bool TreatEverythingAsCode { get; set; } = false;
        public bool NoShowProgress { get; set; } = true;
        public bool TagsOnly { get; set; } = false;
        public int FileTimeOut { get; set; } = 0;
        public int ProcessingTimeOut { get; set; } = 0;
        public int ContextLines { get; set; } = 3;
        public bool ScanUnknownTypes { get; set; }
        public bool NoFileMetadata { get; set; }
    }
}