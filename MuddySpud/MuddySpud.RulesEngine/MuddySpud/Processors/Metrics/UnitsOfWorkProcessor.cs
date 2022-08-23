using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddySpud.Metrics.Engine
{
    public class UnitsOfWorkProcessor
    {
        public static void Aggregate(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            AddBlocksToUnitsOfWork(codeContainer);

            DistributeUnitsOfWork(
                metricsRecord,
                codeContainer
            );

            ConvertUnitsOfWorkToTagCounts(metricsRecord.Structure);
            AggregateChildren(metricsRecord.Structure);
        }

        private static void AddBlocksToUnitsOfWork(BlockTextContainer codeContainer)
        {
            if (codeContainer?.BlockStatsCache?.BlockStats is null)
            {
                return;
            }

            BlockStatsCache cache = codeContainer.BlockStatsCache;
            List<BlockStats> blocks = cache.BlockStats;

            foreach (BlockStats block in blocks)
            {
                cache.UnitsOfWorkStart.Add(
                    new(
                        block.AdjustedMatchStart,
                        block.MatchStartLocation
                    )
                );
            }
        }

        private static void ConvertUnitsOfWorkToTagCounts(MetricsBlock metrics)
        {
            IEnumerable<(int Line, int Count)>? query = metrics.UnitsOfWork
                .GroupBy(m => m.Location.Line)
                .Select(n =>
                    (
                        n.Key,
                        n.Count()
                    )
                )
            ;

            if (query is null)
            {
                return;
            }

            foreach ((int Line, int Count) result in query)
            {
                if (result is { })
                {
                    metrics.TagCounts.IncrementTagCount(
                        $"A.Blocks.UnitsOfWork.Line.{result.Line}",
                        result.Count
                    );
                }
            }
        }

        private static void AggregateChildren(MetricsBlock metrics)
        {
            foreach (MetricsBlock child in metrics.ChildBlocks)
            {
                ConvertUnitsOfWorkToTagCounts(child);
                AggregateChildren(child);
            }
        }

        public static void DistributeUnitsOfWork(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            if (codeContainer?.BlockStatsCache?.BlockStats is null)
            {
                return;
            }

            List<LocationIndex> unitsOfWorkStart = codeContainer.BlockStatsCache.UnitsOfWorkStart;
            unitsOfWorkStart.Sort(LocationIndexExtensions.Compare);
            bool success;

            foreach (LocationIndex locationIndex in unitsOfWorkStart)
            {
                success = MatchBlock(
                    locationIndex,
                    metricsRecord.Structure
                );

                if (!success)
                {
                    metricsRecord.Structure.UnitsOfWork.Add(locationIndex);
                }
            }
        }

        private static bool MatchBlock(
            LocationIndex unitsOfWork,
            MetricsBlock metrics)
        {
            MetricsBlock block;
            bool success;

            for (int i = 0; i < metrics.ChildBlocks.Count; i++)
            {
                block = metrics.ChildBlocks[i];

                if (block.OpenIndex <= unitsOfWork.Index)
                {
                    success = MatchBlock(
                        unitsOfWork,
                        block
                    );

                    if (!success)
                    {
                        block.UnitsOfWork.Add(unitsOfWork);
                    }

                    return true;
                }
            }

            return false;
        }

    }
}
