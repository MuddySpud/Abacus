using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddySpud.Metrics.Engine
{
    public class ErrorProcessor
    {
        public static void Aggregate(MetricsRecord metricsRecord)
        {
            SetLocalErrorTagCounts(metricsRecord.Structure);
            AggregateChildren(metricsRecord.Structure);
        }

        private static void AggregateChildren(MetricsBlock metrics)
        {
            foreach (MetricsBlock child in metrics.ChildBlocks)
            {
                SetLocalErrorTagCounts(child);
                AggregateChildren(child);
            }
        }

        private static void SetLocalErrorTagCounts(MetricsBlock metrics)
        {
            foreach (BlockStatsError error in metrics.Errors)
            {
                metrics.TagCounts.IncrementTagCount($"A.Error.{error.Type}");
            }
        }
    }
}
