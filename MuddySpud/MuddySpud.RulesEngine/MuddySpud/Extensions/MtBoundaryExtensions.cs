using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public static class MtBoundaryExtensions
    {
        public static int Compare(
            this MtBoundary a,
            MtBoundary b)
        {
            if (a.Index < b.Index)
            {
                return -1;
            }

            if (a.Index == b.Index)
            {
                return 0;
            }

            return 1;
        }
    }
}
