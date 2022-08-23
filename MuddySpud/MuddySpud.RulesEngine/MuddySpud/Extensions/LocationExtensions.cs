using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public static class LocationExtensions
    {
        public static int Compare(
            this Location a,
            Location b)
        {
            if (a.Line < b.Line)
            {
                return -1;
            }

            if (a.Line == b.Line)
            {
                if (a.Column == b.Column)
                {
                    return 0;
                }

                if (a.Column > b.Column)
                {
                    return -1;
                }
            }

            return 1;
        }
    }
}
