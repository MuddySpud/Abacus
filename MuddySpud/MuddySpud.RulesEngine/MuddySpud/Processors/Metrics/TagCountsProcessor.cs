using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddySpud.Metrics.Engine
{
    public class TagCountsProcessor
    {
        public static void Aggregate(MetricsBlock metrics)
        {
            foreach (MetricsBlock childMetrics in metrics.ChildBlocks)
            {
                Aggregate(childMetrics);
                metrics.TagCounts.MergeTagCounts(childMetrics.TagCounts);
            }
        }

        public static void OrderTagCounts(MetricsRecord metricsRecord)
        {
            OrderTagCounts(metricsRecord.Structure);
            OrderChildren(metricsRecord.Structure);
        }

        private static void OrderTagCounts(MetricsBlock metrics)
        {
            metrics.TagCounts.Sort((x, y) => x.Tag.CompareTo(y.Tag));
        }

        private static void OrderChildren(MetricsBlock metrics)
        {
            foreach (MetricsBlock childMetrics in metrics.ChildBlocks)
            {
                OrderTagCounts(childMetrics);
                OrderChildren(childMetrics);
            }
        }
    }
}
