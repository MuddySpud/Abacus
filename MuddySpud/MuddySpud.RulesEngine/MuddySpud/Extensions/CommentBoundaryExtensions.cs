using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public static class CommentBoundaryExtensions
    {
        public static int Compare(
            this CommentBoundary a,
            CommentBoundary b)
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
