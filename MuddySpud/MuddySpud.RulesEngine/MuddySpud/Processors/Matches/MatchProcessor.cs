using MuddySpud.RulesEngine.Commands;
using System;
using System.Collections.Generic;

namespace MuddySpud.Metrics.Engine
{
    public class MatchProcessor
    {
        public static void Aggregate(MetricsRecord metricsRecord)
        {
            SetLocalTagCounts(metricsRecord.Structure);
            AggregateChildren(metricsRecord.Structure);
        }

        private static void AggregateChildren(MetricsBlock metrics)
        {
            foreach (MetricsBlock child in metrics.ChildBlocks)
            {
                SetLocalTagCounts(child);
                AggregateChildren(child);
            }
        }

        private static void SetLocalTagCounts(MetricsBlock metrics)
        {
            string matchRecordTag;

            foreach (MtMatchRecord matchRecord in metrics.Matches)
            {
                if (matchRecord == null
                    || matchRecord.Tags == null)
                {
                    continue;
                }

                for (int j = 0; j < matchRecord.Tags.Length; j++)
                {
                    matchRecordTag = matchRecord.Tags[j];
                    metrics.TagCounts.IncrementTagCount(matchRecordTag);
                }
            }
        }

        public static void DistributeMatches(MetricsRecord metricsRecord)
        {
            metricsRecord.Matches.Sort(MtMatchRecordExtensions.Compare);
            bool success;

            foreach (MtMatchRecord match in metricsRecord.Matches)
            {
                success = MatchBlock(
                    match,
                    metricsRecord.Structure
                );

                if (!success)
                {
                    metricsRecord.Structure.Matches.Add(match);
                }
            }
        }

        private static bool MatchBlock(
            MtMatchRecord match,
            MetricsBlock metrics)
        {
            MetricsBlock block;
            bool success;

            for (int i = 0; i < metrics.ChildBlocks.Count; i++)
            {
                block = metrics.ChildBlocks[i];

                if (block.OpenIndex <= match.StartIndex
                    && (block.CloseIndex >= match.EndIndex
                    || (block.CloseIndex == 0 
                        && block.Errors.Count > 0)))
                {
                    success = MatchBlock(
                        match,
                        block
                    );

                    if (!success)
                    {
                        block.Matches.Add(match);
                    }

                    return true;
                }
            }

            return false;
        }

    }
}
