using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddySpud.Metrics.Engine
{
    public class LocalBlockStatsProcessor
    {
        public static void Aggregate(MetricsRecord metricsRecord)
        {
            AggregateLocal(metricsRecord.Structure);
            AggregateChildren(metricsRecord.Structure);
        }

        private static void AggregateChildren(MetricsBlock metrics)
        {
            foreach (MetricsBlock child in metrics.ChildBlocks)
            {
                AggregateLocal(child);
                AggregateChildren(child);
            }
        }

        private static void AggregateLocal(MetricsBlock metrics)
        {
            CountSyblings(metrics);
            CountChildBlocks(metrics);
            CountChildStatements(metrics);
            CountChildMetricBlocks(metrics);
        }

        private static void CountSyblings(MetricsBlock metrics)
        {
            int syblingCount = metrics.Block?.Parent?.ChildBlocks.Count ?? 0;
            syblingCount = syblingCount > 0 ? --syblingCount : 0;

            metrics.TagCounts.IncrementTagCount(
                $"A.Syblings",
                syblingCount
            );
        }

        private static void CountChildBlocks(MetricsBlock metrics)
        {
            metrics.TagCounts.IncrementTagCount(
                $"A.ChildBlocks.FirstGen",
                metrics.Block?.ChildBlocks.Count ?? 0
            );
        }

        private static void CountChildStatements(MetricsBlock metrics)
        {
            metrics.TagCounts.IncrementTagCount(
                $"A.ChildStatements.FirstGen",
                metrics.Block?.ChildStatements.Count ?? 0
            );
        }

        private static void CountChildMetricBlocks(MetricsBlock metrics)
        {
            metrics.TagCounts.IncrementTagCount(
                $"A.ChildBlocks",
                metrics.ChildBlocks?.Count ?? 0
            );

            IEnumerable<(string Type, int Count)>? query = metrics.ChildBlocks?
                .GroupBy(m => m.Type)
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

            foreach ((string Type, int Count) result in query)
            {
                if (result is { })
                {
                    metrics.TagCounts.IncrementTagCount(
                        $"A.Blocks.{result.Type}",
                        result.Count
                    );
                }
            }
        }
    }
}

