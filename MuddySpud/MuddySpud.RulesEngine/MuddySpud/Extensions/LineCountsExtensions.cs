using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public static class LineCountsExtensions
    {
        public static int Compare(
            this LineCounts a,
            LineCounts b)
        {
            if (a.StartIndex < b.StartIndex)
            {
                return -1;
            }

            if (a.StartIndex == b.StartIndex)
            {
                return 0;
            }

            return 1;
        }

        public static void SetOnWord(
            this LineCounts lineCounts,
            bool isWordChar)
        {
            if (lineCounts.OnWord
                && !isWordChar)
            {
                lineCounts.OnWord = false;

                return;
            }

            if (!lineCounts.OnWord
                && isWordChar)
            {
                lineCounts.OnWord = true;
                ++lineCounts.WordCount;

                return;
            }
        }
    }
}
