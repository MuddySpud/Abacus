using MuddySpud.Metrics.Engine;
using System;
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    internal static class LineCountsInterpreter
    {

        public static void CountCharacters(
            this BlockStatsCache blockStatsCache,
            string codeContent,
            IBoundaryCounter boundaryCounter)
        {
            LineCounts lineCounts = new();
            blockStatsCache.LineCountList.Add(lineCounts);
            int lineEndIndex;
            char c;

            for (int i = 0; i < codeContent.Length; i++)
            {
                c = codeContent[i];
                lineCounts.Value.Append(c);

                if (Char.IsWhiteSpace(c))
                {
                    ++lineCounts.SpacesCount;
                    lineCounts.SetOnWord(false);

                    if (Char.Equals('\n', c))
                    {
                        lineEndIndex = boundaryCounter.GetFullIndexFromCodeIndex(i);
                        lineCounts.StartIndex = lineEndIndex - lineCounts.Value.Length + 1;

                        lineCounts = new();
                        blockStatsCache.LineCountList.Add(lineCounts);
                    }
                }
                else if (Char.IsUpper(c))
                {
                    ++lineCounts.UpperCaseCount;
                    lineCounts.SetOnWord(true);
                }
                else if (Char.IsLower(c))
                {
                    ++lineCounts.LowerCaseCount;
                    lineCounts.SetOnWord(true);
                }
                else if (Char.Equals('-', c))
                {
                    ++lineCounts.HyphenCount;
                    lineCounts.SetOnWord(true);
                }
                else if (Char.Equals('_', c))
                {
                    ++lineCounts.UnderscoreCount;
                    lineCounts.SetOnWord(true);
                }
                else
                {
                    lineCounts.SetOnWord(false);
                }
            }
        }
    }
}
