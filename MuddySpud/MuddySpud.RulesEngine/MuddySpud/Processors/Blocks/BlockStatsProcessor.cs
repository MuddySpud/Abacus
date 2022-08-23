using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MuddySpud.Metrics.Engine
{
    public class BlockStatsProcessor
    {
        public static void Aggregate(
            MetricsRecord metricsRecord,
            BlockTextContainer codeContainer)
        {
            if (codeContainer?.BlockStatsCache?.BlockStats is { })
            {
                metricsRecord.Blocks = codeContainer.BlockStatsCache.BlockStats;

                BuildMetricBlocks(
                    codeContainer?.BlockStatsCache?.RootBlockStats,
                    metricsRecord.Structure
                );
            }

            if (codeContainer?.BlockStatsCache?.RootBlockStats is { })
            {
                codeContainer.BlockStatsCache.RootBlockStats.LinkedToMetrics = true;
            }
        }

        private static List<BlockStatsError> BuildMetricBlocks(
            BlockStats? parentBlock,
            MetricsBlock metrics)
        {
            List<BlockStatsError> errors = new();

            if (parentBlock is null)
            {
                return errors;
            }

            foreach (BlockStats child in parentBlock.ChildBlocks)
            {
                if (child.PrintMetrics)
                {
                    MetricsBlock childMetrics = new()
                    {
                        Type = child.Type,
                        Signature = child.CleanedSignature,
                        Content = child.Content,
                        Partial = child.Flags.Contains("partial"),
                        Depth = child.Depth,
                        SyblingCount = child.SyblingCount,
                        OpenIndex = child.AdjustedMatchStart < child.AdjustedOpenIndex ? child.AdjustedMatchStart : child.AdjustedOpenIndex,
                        CloseIndex = child.AdjustedCloseIndex,
                        BlockEndLocation = child.BlockEndLocation,
                        Block = child,
                        Errors = child.Errors
                    };

                    childMetrics.SetBlockStartLocation(child.BlockStartLocation);

                    metrics.ChildBlocks.Add(childMetrics);
                    child.LinkedToMetrics = true;

                    BuildMetricBlocks(
                        child,
                        childMetrics
                    );
                }
            }

            return errors;
        }

        public static void AggregateBlockDepth(MetricsRecord metricsRecord)
        {
            GetBlockDepth(metricsRecord.Structure);
            AggregateBlockDepth(metricsRecord.Structure);
        }

        private static void AggregateBlockDepth(MetricsBlock metrics)
        {
            foreach (MetricsBlock child in metrics.ChildBlocks)
            {
                GetBlockDepth(child);
                AggregateBlockDepth(child);
            }
        }

        private static void GetBlockDepth(MetricsBlock metrics)
        {
            List<(int Depth, int Count)> depthLog = new();

            GetChildDepths(
                metrics.Block,
                depthLog
            );

            foreach ((int Depth, int Count) tuple in depthLog)
            {
                metrics.TagCounts.IncrementTagCount(
                    $"A.ChildBlock.Depth.{tuple.Depth}",
                    tuple.Count
                );
            }
        }

        private static void GetChildDepths(
            BlockStats? block,
            List<(int Depth, int Count)> depthLog)
        {
            if (block is null)
            {
                return;
            }

            foreach (BlockStats child in block.ChildBlocks)
            {
                if (child.LinkedToMetrics)
                {
                    // Will gets its details from tag counts later
                    continue;
                }

                if (child.ChildBlocks.Count == 0)
                {
                    IncrementDepthCount(
                        depthLog,
                        child.Depth
                    );
                }
                else
                {
                    GetChildDepths(
                        child,
                        depthLog
                    );
                }
            }
        }

        public static void IncrementDepthCount(
            List<(int Depth, int Count)> depthLog,
            int depth,
            int count = 1)
        {
            if (count <= 0)
            {
                return;
            }

            (int Depth, int Count) tuple;

            for (int i = 0; i < depthLog.Count; i++)
            {
                tuple = depthLog[i];

                if (String.Equals(tuple.Depth, depth))
                {
                    tuple.Count += count;

                    return;
                }
            }

            depthLog.Add((depth, count));
        }

    }
}

