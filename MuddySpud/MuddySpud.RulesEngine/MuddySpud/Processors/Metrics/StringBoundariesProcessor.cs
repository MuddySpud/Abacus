using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddySpud.Metrics.Engine
{
    public class StringBoundariesProcessor
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
                codeContainer.BlockStatsCache.StringOutputBoundaries,
                codeContainer.BlockStatsCache.LineStarts
            );

            DistributeStringBondaries(
                metricsRecord,
                codeContainer
            );

            IEnumerable<MtBoundary>? groupedBoundaries = GroupBoundariesByLine(metricsRecord.Structure);

            ConvertBoundariesToTagCounts(
                metricsRecord.Structure,
                groupedBoundaries
            );

            AggregateChildren(metricsRecord.Structure);
        }

        private static void AddLineNumbers(
            List<MtBoundary> stringBoundaries,
            List<int> lineStarts)
        {
            stringBoundaries.Sort(MtBoundaryExtensions.Compare);

            int lineStart;
            int nextLineStart;
            int i = 1;
            int nextIndex;

            foreach (MtBoundary boundary in stringBoundaries)
            {
                for (; i < lineStarts.Count; i++)
                {
                    lineStart = lineStarts[i];
                    nextIndex = i + 1;

                    if (nextIndex >= lineStarts.Count)
                    {
                        boundary.LineNumber = i;

                        break;
                    }
                    else
                    {
                        nextLineStart = lineStarts[nextIndex];

                        if (boundary.Index >= lineStart
                            && boundary.Index < nextLineStart)
                        {
                            boundary.LineNumber = i;

                            break;
                        }
                    }
                }
            }
        }

        private static void DistributeStringBondaries(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            if (codeContainer?.BlockStatsCache?.BlockStats is null)
            {
                return;
            }

            List<MtBoundary> stringBoundaries = codeContainer.BlockStatsCache.StringOutputBoundaries;
            stringBoundaries.Sort(MtBoundaryExtensions.Compare);
            bool success;

            foreach (MtBoundary boundary in stringBoundaries)
            {
                success = MatchBlock(
                     boundary,
                     metricsRecord.Structure
                 );

                if (!success)
                {
                    metricsRecord.Structure.StringBoundaries.Add(boundary);
                }
            }
        }

        private static bool MatchBlock(
            MtBoundary boundary,
            MetricsBlock metrics)
        {
            MetricsBlock block;
            bool success;

            for (int i = 0; i < metrics.ChildBlocks.Count; i++)
            {
                block = metrics.ChildBlocks[i];

                if (block.LineStartIndex <= boundary.Index
                    && block.CloseIndex >= boundary.Index)
                {
                    success = MatchBlock(
                        boundary,
                        block
                    );

                    if (!success)
                    {
                        block.StringBoundaries.Add(boundary);
                    }

                    return true;
                }
            }

            return false;
        }

        private static void AggregateChildren(MetricsBlock metrics)
        {
            IEnumerable<MtBoundary>? groupedBoundaries;

            foreach (MetricsBlock child in metrics.ChildBlocks)
            {
                groupedBoundaries = GroupBoundariesByLine(child);

                ConvertBoundariesToTagCounts(
                    child,
                    groupedBoundaries
                );

                AggregateChildren(child);
            }
        }

        private static IEnumerable<MtBoundary>? GroupBoundariesByLine(MetricsBlock metrics)
        {
            IEnumerable<MtBoundary>? query = metrics.StringBoundaries
                .GroupBy(m => new { m.LineNumber, m.Type })
                .Select(n => new MtBoundary(
                        n.Min(s => s.Index),
                        n.Sum(s => s.Length),
                        n.First().Type,
                        n.First().LineNumber
                    )
                )
            ;

            return query;
        }

        private static void ConvertBoundariesToTagCounts(
            MetricsBlock metrics,
            IEnumerable<MtBoundary>? groupedBoundaries)
        {
            if (groupedBoundaries is null)
            {
                return;
            }

            List<TagCounter> tagCounts = metrics.TagCounts;

            foreach (MtBoundary boundary in groupedBoundaries)
            {
                if (!String.Equals(boundary.Type, "blankLine"))
                {
                    tagCounts.PrintTagCount(
                        boundary.Type,
                        boundary.Length,
                        boundary.LineNumber
                    );
                }
            }
        }
    }
}

