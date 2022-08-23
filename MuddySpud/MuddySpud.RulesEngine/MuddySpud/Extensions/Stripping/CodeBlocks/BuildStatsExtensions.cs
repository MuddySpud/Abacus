using MuddySpud.RulesEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/*
    Search for any open,
    Then search for any close,
    Keep searching for opens until the current open index is greater than the close.
    That means we know which opens are between the first open and the first close.
    Now we can close that last opened block of that right type.
    We keep going like this till all children have been closed and then can close the parent.
    If a block setting is marked as not explicit close then can close it with the parent
 */
namespace MuddySpud.Metrics.Engine
{
    static class BuildStatsExtensions
    {
        internal static void BuildStats(
            this CodeBlockLoopCache cache,
            IBoundaryCounter boundaryCounter,
            string contentString)
        {
            cache.RootCodeBlock.AddChildrenStats(
                cache.BlockStats,
                boundaryCounter,
                contentString,
                new List<StatementStats>(),
                false
            );

            cache.BlockStats.Sort(BlockStatsExtensions.Compare);
        }

        internal static void BuildStats(
            this CodeBlockLoopCache cache,
            IBoundaryCounter boundaryCounter,
            string contentString,
            List<StatementStats> statementStats)
        {
            cache.RootCodeBlock.AddChildrenStats(
                cache.BlockStats,
                boundaryCounter,
                contentString,
                statementStats
            );

            foreach (StatementStats stats in statementStats)
            {
                if (!stats.Assigned
                    && stats.CloseIndex < cache.RootCodeBlock.CloseIndex)
                {
                    cache.RootCodeBlock.ChildStatements.Add(stats);
                    stats.Assigned = true;
                }
            }

            cache.BlockStats.Sort(BlockStatsExtensions.Compare);
        }

        private static void AddChildrenStats(
            this BlockStats block,
            List<BlockStats> blockStats,
            IBoundaryCounter boundaryCounter,
            string contentString,
            List<StatementStats> statementStats,
            bool checkForTails = true)
        {
            List<StatementStats> childStatementStats;
            StatementStats childStatement;
            int i = 0;

            foreach (BlockStats child in block.ChildBlocks)
            {
                if (child.CloseIndex < 1)
                {
                    child.Errors.Add(new BlockStatsError("UnclosedBlock", "There is a block without a closeIndex, this could corrupt all the results."));
                }

                childStatementStats = new();

                for (; i < statementStats.Count; i++)
                {
                    childStatement = statementStats[i];

                    if (childStatement.Assigned)
                    {
                        continue;
                    }

                    if (childStatement.CloseIndex >= child.OpenIndex
                        && childStatement.CloseIndex < child.CloseIndex)
                    {
                        childStatementStats.Add(childStatement);
                    }
                    else if (childStatement.CloseIndex > child.CloseIndex)
                    {
                        break;
                    }
                    else if (childStatement.CloseIndex == child.CloseIndex)
                    {
                        childStatement.Assigned = true;
                    }
                }

                child.AdjustedOpenIndex = boundaryCounter.GetFullIndexFromCodeIndex(child.OpenIndex);
                child.BlockStartLocation = boundaryCounter.GetLocation(child.AdjustedOpenIndex);

                if (child.CloseIndex > 0)
                {
                    child.AdjustedCloseIndex = boundaryCounter.GetFullIndexFromCodeIndex(child.CloseIndex);
                    child.BlockEndLocation = boundaryCounter.GetLocation(child.AdjustedCloseIndex);
                }


                if (statementStats.Count == 0)
                {
                    blockStats.Add(child);
                }

                child.AddChildrenStats(
                    blockStats,
                    boundaryCounter,
                    contentString,
                    childStatementStats,
                    checkForTails
                );
            }

            if (statementStats.Count > 0)
            {
                block.ReBuildChildBlocks(
                    statementStats,
                    blockStats,
                    boundaryCounter
                );
            }

            if (checkForTails)
            {
                // If there is any non-blank text after last child-block then create a statement to hold it and add it as a block
                block.CheckForTailStatement(
                    blockStats,
                    contentString,
                    boundaryCounter
                );
            }
        }

        private static void ReBuildChildBlocks(
            this BlockStats block,
            List<StatementStats> statementStats,
            List<BlockStats> blockStats,
            IBoundaryCounter boundaryCounter)
        {
            List<BlockStats> reorderedBlockStats = new();

            int statementIndex = 0;
            int childBlockIndex = 0;
            int openIndex = block.OpenIndex;
            int depth = block.Depth + 1;

            StatementStats? statement;
            BlockStats? childBlock;

            while (true)
            {
                if (statementIndex < statementStats.Count)
                {
                    // TODO Need a way of not reloading this unless it has changed!!!!!!!!!!!!!!!!!
                    statement = statementStats[statementIndex];

                    if (statement.Assigned)
                    {
                        ++statementIndex;

                        continue;
                    }

                    if (statement.CloseIndex == block.CloseIndex)
                    {
                        ++statementIndex;
                        statement.Assigned = true;

                        continue;
                    }
                }
                else
                {
                    statement = null;
                }

                if (childBlockIndex < block.ChildBlocks.Count)
                {
                    childBlock = block.ChildBlocks[childBlockIndex];
                }
                else
                {
                    childBlock = null;
                }

                if (statement is { })
                {
                    if (childBlock is { })
                    {
                        if (statement.CloseIndex < childBlock.OpenIndex)
                        {
                            (statementIndex, openIndex) = reorderedBlockStats.AddStatementBlock(
                                blockStats,
                                block,
                                openIndex,
                                statement,
                                depth,
                                boundaryCounter,
                                statementIndex
                            );
                        }
                        else if (statement.CloseIndex > childBlock.OpenIndex)
                        {
                            (childBlockIndex, openIndex) = reorderedBlockStats.AddChildBlock(
                                blockStats,
                                childBlock,
                                childBlockIndex
                            );
                        }
                    }
                    else if (statement.CloseIndex >= 0)
                    {
                        (statementIndex, openIndex) = reorderedBlockStats.AddStatementBlock(
                            blockStats,
                            block,
                            openIndex,
                            statement,
                            depth,
                            boundaryCounter,
                            statementIndex
                        );
                    }
                }
                else if (childBlock is { }
                    && childBlock.CloseIndex >= 0)
                {
                    (childBlockIndex, openIndex) = reorderedBlockStats.AddChildBlock(
                        blockStats,
                        childBlock,
                        childBlockIndex
                    );
                }
                else
                {
                    break; ;
                }
            }

            block.ChildBlocks = reorderedBlockStats;
        }

        private static (int childBlockIndex, int openIndex) AddChildBlock(
            this List<BlockStats> reorderedBlockStats,
            List<BlockStats> blockStats,
            BlockStats childBlock,
            int childBlockIndex)
        {
            reorderedBlockStats.Add(childBlock);
            blockStats.Add(childBlock);

            return (++childBlockIndex, childBlock.CloseIndex + 1);
        }
    }
}
