using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MuddySpud.Metrics.Engine
{
    public class MetricsProcessor
    {
        public static void Aggregate(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            if (metricsRecord.Matches.Count() == 0)
            {
                return;
            }

            string fileName = metricsRecord.Matches.First()?.FileName ?? String.Empty;

            if (String.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            PreTagCountAggregator(
                metricsRecord,
                codeContainer
            );

            TagCountsProcessor.Aggregate(metricsRecord.Structure);

            PostTagCountAggregator(
                metricsRecord,
                codeContainer);

            //// Remove later
            //TagCountsProcessor.OrderTagCounts(metricsRecord);
        }

        private static void PreTagCountAggregator(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            BlockStatsProcessor.Aggregate(
                metricsRecord,
                codeContainer
            );

            StringBoundariesProcessor.Aggregate(
                metricsRecord,
                codeContainer
            );

            UnitsOfWorkProcessor.Aggregate(
                metricsRecord,
                codeContainer
            );

            LineCountsProcessor.Aggregate(
                metricsRecord,
                codeContainer
            );

            MatchProcessor.DistributeMatches(metricsRecord);

            MatchProcessor.Aggregate(metricsRecord);
            ErrorProcessor.Aggregate(metricsRecord);
            BlockStatsProcessor.AggregateBlockDepth(metricsRecord);
        }

        private static void PostTagCountAggregator(
            MetricsRecord metricsRecord,
            BlockTextContainer _codeContainer)
        {
            LocalBlockStatsProcessor.Aggregate(metricsRecord);
        }
    }
}
