using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System.Collections.Generic;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public class StringStripLoopCache
    {
        public StringBuilder StringContent { get; }
        public StringSettings StringSettings { get; set; }
        public StringState? Current { get; set; }
        public OutputBoundaries OutputBoundaries { get; }

        public StringStripLoopCache(
            StringBuilder stringContent,
            StringSettings stringSettinge,
            OutputBoundaries outputBoundaries)
        {
            StringContent = stringContent;
            StringSettings = stringSettinge;
            OutputBoundaries = outputBoundaries;
        }
    }
}
