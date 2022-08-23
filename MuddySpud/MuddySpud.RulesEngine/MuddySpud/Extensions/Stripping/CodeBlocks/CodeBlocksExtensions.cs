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
    static class CodeBlocksExtensions
    {
        internal static void ScanForBlocks(
            this StringBuilder content,
            CodeBlockLoopCache cache,
            string[] opens,
            char[] openFirstChars)
        {
            int openIndex = 0;
            int openNeedleIndex = 0;
            BlockStats? closeBlock = null;
            int closeIndex;
            int openStartSearchIndex = openIndex;
            int closeStartSearchIndex = openIndex;
            int contentLength = content.Length;

            while (openStartSearchIndex < contentLength)
            {
                if (openIndex >= 0)
                {
                    // TODO Need a way of not looking this up etc unless has changed!!!!!
                    (openIndex, openNeedleIndex) = content.IndexOfAny(
                        openFirstChars,
                        opens,
                        openStartSearchIndex
                    );
                }

                // Search for the next close
                (closeIndex, closeBlock) = content.GetNextCloseIndex(
                    cache,
                    closeStartSearchIndex,
                    closeBlock?.Settings
                );

                if (openIndex < 0)
                {
                    if (closeIndex < 0)
                    {
                        break;
                    }
                    else
                    {
                        closeIndex = cache.CloseCodeBlock(
                            closeBlock,
                            closeIndex
                        );

                        closeStartSearchIndex = closeIndex + 1;
                    }
                }
                else
                {
                    if (closeIndex > 0
                        && closeIndex < openIndex)
                    {
                        closeIndex = cache.CloseCodeBlock(
                            closeBlock,
                            closeIndex
                        );

                        closeStartSearchIndex = closeIndex + 1;
                    }
                    else
                    {
                        cache.OpenCodeBlock(
                            cache.Opens[openNeedleIndex],
                            openIndex
                        );

                        openStartSearchIndex = openIndex + 1;
                    }
                }
            }
        }

        private static (int closeIndex, BlockStats? block) GetNextCloseIndex(
            this StringBuilder content,
            CodeBlockLoopCache cache,
            int startSearchIndex,
            IBoundarySettings? settings)
        {
            if (cache.UnClosed.Count == 0)
            {
                return (-1, null);
            }

            // The closedNeedleIndex gives the settings type
            // CHeck if it matches the last unclosed block
            // If it doesn't is that block marked as explicitClose = false
            // If so can check its parent for correct type
            // And if that doesn't match but it is marked as explicitClose = false
            // we can check its parent.
            // When we hit a block need to go back and look for close index and repeat
            int unclosedBlockIndex = cache.UnClosed.Count - 1;
            BlockStats? block = cache.UnClosed[unclosedBlockIndex];

            int blockCloseIndex = content.GetNextCloseForBlock(
                block,
                startSearchIndex);

            if (block.Settings.ExplicitClose)
            {
                return (blockCloseIndex, block);
            }

            BlockStats? current = block;
            BlockStats? parent;
            int parentCloseIndex = blockCloseIndex;
            int closeIndex;

            // TODO NEED to set parent = parent.Parent and close set that parent.CloseIndex too!!!!
            while (!current.Settings.ExplicitClose)
            {
                parent = current.Parent;

                if (parent is null)
                {
                    break;
                }

                closeIndex = content.GetNextCloseForBlock(
                    parent,
                    startSearchIndex);

                if (closeIndex < parentCloseIndex)
                {
                    parentCloseIndex = closeIndex;
                }

                current = parent;
            }

            if (parentCloseIndex < blockCloseIndex)
            {
                return (parentCloseIndex, current);
            }

            return (blockCloseIndex, block);
        }

        private static int GetNextCloseForBlock(
            this StringBuilder content,
            BlockStats block,
            int startSearchIndex)
        {
            if (block.Settings.BlockType == "file"
                && block.Settings.Close.Length == 0)
            {
                return content.Length - 1;
            }

            while (startSearchIndex < content.Length)
            {
                int closeIndex = content.IndexOf(
                    block.Settings.Close,
                    startSearchIndex);

                if (closeIndex < 0)
                {
                    return -1;
                }

                if (closeIndex >= block.OpenIndex)
                {
                    return closeIndex;
                }

                startSearchIndex = closeIndex + 1;
            }

            return -1;
        }

        private static void OpenCodeBlock(
            this CodeBlockLoopCache cache,
            string open,
            int index)
        {
            BlockBoundarySettings? blockBoundarySettings = cache.GetBounadaryFromOpen(open);

            if (blockBoundarySettings is { })
            {
                BlockStats codeBlock = new(
                    cache.Current,
                    index + open.Length,
                    cache.Current.Depth + 1,
                    blockBoundarySettings
                );

                cache.Current.ChildBlocks.Add(codeBlock);
                cache.Current = codeBlock;
                cache.UnClosed.Add(codeBlock);
            }
        }

        private static BlockBoundarySettings? GetBounadaryFromOpen(
            this CodeBlockLoopCache cache,
            string open)
        {
            foreach (BlockBoundarySettings blockBoundarySettings in cache.CodeBlockSettings.Boundaries)
            {
                if (String.Equals(blockBoundarySettings.Open, open, StringComparison.Ordinal))
                {
                    return blockBoundarySettings;
                }
            }

            return null;
        }

        private static int CloseCodeBlock(
            this CodeBlockLoopCache cache,
            BlockStats? closeBlock,
            int index)
        {
            if (closeBlock is null)
            {
                throw new NotImplementedException("Block is null...");
            }

            if (closeBlock.CloseIndex > 0)
            {
                throw new NotImplementedException("Block closeIndex has already been set");
            }

            BlockStats? parent = closeBlock.Parent;
            closeBlock.CloseIndex = index;
            cache.UnClosed.Remove(closeBlock);

            closeBlock.CloseDescendants(
                cache,
                index);

            if (parent is { })
            {
                cache.Current = parent;
            }

            return index;
        }

        private static void CloseDescendants(
            this BlockStats parent,
            CodeBlockLoopCache cache,
            int closeIndex)
        {
            foreach (BlockStats child in parent.ChildBlocks)
            {
                if (child.CloseIndex == 0
                    && !child.Settings.ExplicitClose)
                {
                    child.CloseIndex = closeIndex;
                    cache.UnClosed.Remove(child);

                    child.CloseDescendants(
                        cache,
                        closeIndex);
                }
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
