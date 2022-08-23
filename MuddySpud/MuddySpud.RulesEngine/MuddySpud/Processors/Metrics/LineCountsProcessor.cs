using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddySpud.Metrics.Engine
{
    public class LineCountsProcessor
    {
        public static void Aggregate(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            if (codeContainer.BlockStatsCache is null)
            {
                return;
            }

            AddLineNumbers(
                codeContainer.BlockStatsCache.LineCountList,
                codeContainer.BlockStatsCache.LineStarts
            );

            DistributeLineCounts(
                metricsRecord,
                codeContainer
            );

            ConvertLineCountsToTagCounts(metricsRecord.Structure);
            AggregateChildren(metricsRecord.Structure);
        }

        private static void AddLineNumbers(
            List<LineCounts> lineCounts,
            List<int> lineStarts)
        {
            lineCounts.Sort(LineCountsExtensions.Compare);

            int lineStart;
            int nextLineStart;
            int i = 1;
            int nextIndex;

            foreach (LineCounts count in lineCounts)
            {
                for (; i < lineStarts.Count; i++)
                {
                    lineStart = lineStarts[i];
                    nextIndex = i + 1;

                    if (nextIndex >= lineStarts.Count)
                    {
                        count.LineNumber = i;

                        break;
                    }
                    else
                    {
                        nextLineStart = lineStarts[nextIndex];

                        if (count.StartIndex >= lineStart
                            && count.StartIndex < nextLineStart)
                        {
                            count.LineNumber = i;

                            break;
                        }
                    }
                }
            }
        }

        private static void ConvertLineCountsToTagCounts(MetricsBlock metrics)
        {
            List<TagCounter> tagCounts = metrics.TagCounts;

            foreach (LineCounts counts in metrics.LineCounts)
            {
                tagCounts.PrintTagCount(
                    "Spaces",
                    counts.SpacesCount,
                    counts.LineNumber
                );

                tagCounts.PrintTagCount(
                    "Uppercase",
                    counts.UpperCaseCount,
                    counts.LineNumber
                );

                tagCounts.PrintTagCount(
                    "Lowercase",
                    counts.LowerCaseCount,
                    counts.LineNumber
                );

                tagCounts.PrintTagCount(
                    "_",
                    counts.UnderscoreCount,
                    counts.LineNumber
                );

                tagCounts.PrintTagCount(
                    "-",
                    counts.HyphenCount,
                    counts.LineNumber
                );

                tagCounts.PrintTagCount(
                    "Length",
                    counts.Value.Length,
                    counts.LineNumber
                );

                tagCounts.PrintTagCount(
                    "Words",
                    counts.WordCount,
                    counts.LineNumber
                );
            }
        }

        private static void AggregateChildren(MetricsBlock metrics)
        {
            foreach (MetricsBlock child in metrics.ChildBlocks)
            {
                ConvertLineCountsToTagCounts(child);
                AggregateChildren(child);
            }
        }

        private static void DistributeLineCounts(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            if (codeContainer?.BlockStatsCache?.BlockStats is null)
            {
                return;
            }

            List<LineCounts> lineCounts = codeContainer.BlockStatsCache.LineCountList;
            lineCounts.Sort(LineCountsExtensions.Compare);
            LineCounts counts;
            bool success;

            for (int i = 0; i < lineCounts.Count; i++)
            {
                counts = lineCounts[i];

                success = MatchBlock(
                     counts,
                     metricsRecord.Structure
                 );

                if (!success)
                {
                    metricsRecord.Structure.LineCounts.Add(counts);
                }
            }
        }

        private static bool MatchBlock(
            LineCounts counts,
            MetricsBlock metrics)
        {
            MetricsBlock block;
            bool success;

            for (int i = 0; i < metrics.ChildBlocks.Count; i++)
            {
                block = metrics.ChildBlocks[i];

                if (block.LineStartIndex <= counts.StartIndex
                    && block.CloseIndex >= counts.StartIndex)
                {
                    success = MatchBlock(
                        counts,
                        block
                    );

                    if (!success)
                    {
                        block.LineCounts.Add(counts);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
