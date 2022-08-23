using Microsoft.ApplicationInspector.Commands;
using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    [DebuggerDisplay("{StartIndex} - {Value.ToString()}")]
    public class LineCounts
    {
        public int LineNumber { get; set; }
        public int StartIndex { get; set; }
        public bool OnWord { get; set; }
        public int SpacesCount { get; set; }
        public int UpperCaseCount { get; set; }
        public int LowerCaseCount { get; set; }
        public int WordCount { get; set; }
        public int UnderscoreCount { get; set; }
        public int HyphenCount { get; set; }
        public StringBuilder Value { get; set; } = new();

        public LineCounts()
        {
        }
    }
}
