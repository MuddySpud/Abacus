using Microsoft.ApplicationInspector.Commands;
using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace MuddySpud.Metrics.Engine
{
    [DebuggerDisplay("{Location.Line}-{Location.Column}")]
    public record LocationIndex(int Index, Location Location);
}
