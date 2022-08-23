using MuddySpud.RulesEngine;
using System;
using System.Collections.Generic;
using System.Text;

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
    static class CodeStatementsExtensions
    {
        internal static List<StatementStats> ScanForStatements(
            this StringBuilder content,
            CodeBlockLoopCache cache)
        {
            int index = 0;
            int needleIndex;
            List<StatementStats> statementStatList = new();
            int startSearchIndex = index;
            int contentLength = content.Length;
            StatementStats statementStats;

            while (startSearchIndex < contentLength)
            {
                (index, needleIndex) = content.IndexOfAny(
                    cache.EndFirstChars,
                    cache.Ends,
                    startSearchIndex
                );

                if (index < 0)
                {
                    break;
                }
                else
                {
                    statementStats = new(
                        index,
                        cache.CodeBlockSettings.Statements[needleIndex]
                    );

                    statementStatList.Add(statementStats);

                    startSearchIndex = index + 1;
                }
            }

            return statementStatList;
        }

        internal static (int statementIndex, int openIndex) AddStatementBlock(
            this List<BlockStats> reorderedBlockStats,
            List<BlockStats> blockStats,
            BlockStats parent,
            int openIndex,
            StatementStats statement,
            int depth,
            IBoundaryCounter boundaryCounter,
            int statementIndex)
        {
            openIndex = reorderedBlockStats.AddStatementBlock(
                blockStats,
                parent,
                openIndex,
                statement.CloseIndex,
                depth,
                boundaryCounter,
                statement.Settings
            );

            statement.Assigned = true;

            return (++statementIndex, openIndex);
        }

        internal static void CheckForTailStatement(
            this BlockStats block,
            List<BlockStats> blockStats,
            string contentString,
            IBoundaryCounter boundaryCounter)
        {
            if (block.ChildBlocks.Count == 0)
            {
                return;
            }

            int lastCloseIndex = block.ChildBlocks[^1].CloseIndex + 1;

            if (block.CloseIndex <= lastCloseIndex)
            {
                return;
            }

            string tail = String.Empty;

            if (block.CloseIndex > 0)
            {
                tail = contentString.Substring(lastCloseIndex, block.CloseIndex - lastCloseIndex).Trim();
            }

            if (!String.IsNullOrWhiteSpace(tail)
                && tail != "\"") // In a format string the closing " is sometimes caught by the tail statement so exclude it
            {
                block.ChildBlocks.AddStatementBlock(
                    blockStats,
                    block,
                    lastCloseIndex,
                    block.CloseIndex,
                    block.Depth + 1,
                    boundaryCounter,
                    new StatementBoundarySettings(
                        "Tail",
                        "Code",
                        "",
                        "text"
                    )
                );
            }
        }

        private static int AddStatementBlock(
            this List<BlockStats> reorderedBlockStats,
            List<BlockStats> blockStats,
            BlockStats parent,
            int openIndex,
            int closeIndex,
            int depth,
            IBoundaryCounter boundaryCounter,
            IBoundarySettings settings)
        {
            BlockStats statementBlock = new(
                parent,
                openIndex,
                depth,
                settings
            );

            statementBlock.CloseIndex = closeIndex;

            if (statementBlock.OpenIndex > statementBlock.CloseIndex)
            {
                throw new NotImplementedException("Statement CloseIndex is less than OpenIndex");
            }

            statementBlock.AdjustedOpenIndex = boundaryCounter.GetFullIndexFromCodeIndex(statementBlock.OpenIndex);
            statementBlock.BlockStartLocation = boundaryCounter.GetLocation(statementBlock.AdjustedOpenIndex);

            if (statementBlock.CloseIndex > 0)
            {
                statementBlock.AdjustedCloseIndex = boundaryCounter.GetFullIndexFromCodeIndex(statementBlock.CloseIndex);
                statementBlock.BlockEndLocation = boundaryCounter.GetLocation(statementBlock.AdjustedCloseIndex);
            }

            reorderedBlockStats.Add(statementBlock);
            blockStats.Add(statementBlock);

            return statementBlock.CloseIndex + 1;
        }
    }
}
