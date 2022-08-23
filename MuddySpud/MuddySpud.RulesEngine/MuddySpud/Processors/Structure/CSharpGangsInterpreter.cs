using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpGangsInterpreter
    {
        public static void ProcessGang(
            this BlockStats group,
            BlockStatsCache blockStatsCache,
            IEnumerable<GroupSettings> groups,
            int groupIndex,
            GroupSearchMatch result,
            List<GroupSearchMatch> results,
            int resultIndex,
            IBoundaryCounter boundaryCounter,
            string text)
        {
            if (result.GroupSettings.Gang is null)
            {
                return;
            }

            if (result.GangBlock is { } gangBlock)
            {
                gangBlock.ProcessGangBlocks(
                    blockStatsCache,
                    group,
                    result,
                    boundaryCounter,
                    text
                );
            }
            else
            {
                group.ScanForGang(
                    groups,
                    groupIndex,
                    result,
                    results,
                    resultIndex
                );
            }
        }

        public static void ProcessGangBlocks(
            this BlockStats gangBlock,
            BlockStatsCache blockStatsCache,
            BlockStats group,
            GroupSearchMatch result,
            IBoundaryCounter boundaryCounter,
            string text)
        {
            // Add group to gangBlock
            // If position is 1 need to

            gangBlock.ChildBlocks.Add(group);
            BlockStats? parent = group.Parent;

            if (result.GroupSettings.Gang?.Position == 1
                && parent is { })
            {
                List<BlockStats> reArranged = new();
                BlockStats child;

                for (int p = 0; p < parent.ChildBlocks.Count; p++)
                {
                    child = parent.ChildBlocks[p];

                    if (!gangBlock.ChildBlocks.Contains(child))
                    {
                        reArranged.Add(child);
                    }
                }

                reArranged.Add(gangBlock);
                reArranged.Sort(BlockStatsExtensions.Compare);
                gangBlock.ChildBlocks.Sort(BlockStatsExtensions.Compare);
                gangBlock.OpenIndex = group.OpenIndex;
                gangBlock.Parent = parent;
                parent.ChildBlocks = reArranged;
                gangBlock.ResetDepth();
                blockStatsCache.BlockStats.Add(gangBlock);

                for (int q = 0; q < gangBlock.ChildBlocks.Count; q++)
                {
                    gangBlock.ChildBlocks[q].Parent = gangBlock;
                }

                gangBlock.BuildGangProperties(
                    boundaryCounter,
                    result.GroupSettings,
                    text
                );
            }
        }

        public static void ScanForGang(
            this BlockStats group,
            IEnumerable<GroupSettings> groups,
            int groupIndex,
            GroupSearchMatch result,
            List<GroupSearchMatch> results,
            int resultIndex)
        {
            if (group is null
                || group.Parent is null
                || groupIndex == 0
                || result.GroupSettings.Gang is null)
            {
                return;
            }

            // what is the last block in the group with matching gang that has the next position?
            //      If the current block is an else the next group should be an elseif or if
            //      The last block of each of those is Code the next would be Parameter
            // what is the previous sybling block does it have the blocktype ie Code or Parameter?
            // A gang cannot consist of a single group so we need to look for at least one group beyond this first one
            BlockStats gangBlock = new(
                group.Parent,
                group.OpenIndex,
                group.Depth,
                new GangBoundarySettings(result.GroupSettings.Gang.Name)
            );

            gangBlock.ChildBlocks.Add(group);
            gangBlock.CloseIndex = group.CloseIndex;
            result.GangBlock = gangBlock;

            group.ScanForGangMembers(
                groups,
                groupIndex,
                result,
                results,
                resultIndex,
                gangBlock
            );
        }

        private static void ScanForGangMembers(
            this BlockStats lastGangMember,
            IEnumerable<GroupSettings> groups,
            int lastGangMemberIndex,
            GroupSearchMatch result,
            List<GroupSearchMatch> results,
            int resultIndex,
            BlockStats gangBlock)
        {
            if (lastGangMember is null
                || lastGangMember.Parent is null
                || lastGangMemberIndex == 0
                || result.GroupSettings.Gang is null)
            {
                return;
            }

            List<GroupSettings> earlierGangMembers = new();
            GroupGangSettings gang = result.GroupSettings.Gang;

            foreach (GroupSettings settings in groups)
            {
                if (settings.Gang is { }
                    && settings.Gang.Name == gang.Name)
                {
                    if (gang.Position > settings.Gang.Position)
                    {
                        earlierGangMembers.Add(settings);
                    }
                    else if (gang.Position == settings.Gang.Position
                        && gang.Type == "multi")
                    {
                        earlierGangMembers.Add(settings);
                    }
                }
            }

            BlockStats? sybling = null;
            BlockStats? previousSybling = null;
            List<GroupSettings> gangMemberMatches = new();
            List<GroupSettings> possibleGangMembers = earlierGangMembers;
            bool blocksFinished = false;
            int gangMemberBlockCount = 1;
            int previousSyblingIndex = -1;
            int syblingIndex = -1;

            while (!blocksFinished)
            {
                blocksFinished = true;
                syblingIndex = lastGangMemberIndex - gangMemberBlockCount;

                if (syblingIndex < 0)
                {
                    return;
                }

                sybling = lastGangMember.Parent.ChildBlocks[syblingIndex];

                foreach (GroupSettings gangMember in possibleGangMembers)
                {
                    if (gangMember.Blocks.Count >= gangMemberBlockCount)
                    {
                        blocksFinished = false;

                        if (gangMember.Blocks[^gangMemberBlockCount] == sybling.Settings.BlockType)
                        {
                            gangMemberMatches.Add(gangMember);
                        }
                    }
                }

                if (blocksFinished)
                {
                    break;
                }

                gangMemberBlockCount++;
                possibleGangMembers = gangMemberMatches;
                gangMemberMatches = new();
                previousSyblingIndex = syblingIndex;
                previousSybling = sybling;
            }

            if (possibleGangMembers.Count == 0
                || previousSybling is null)
            {
                // No gang
                return;
            }

            // If reached this point then forming a gang still a possibility
            // After reaching the last block listed does its opening index match any of the results
            // with the expected type.
            GroupSearchMatch gangResult;
            Match gangMatch;
            int gangMatchEnd;
            GroupSettings gangGroupSettings;

            for (int n = resultIndex; n >= 0; n--)
            {
                gangResult = results[n];
                gangMatch = gangResult.Match;
                gangMatchEnd = gangResult.MatchIndex + gangMatch.Value.Length;
                gangGroupSettings = gangResult.GroupSettings;

                if (previousSybling.OpenIndex == gangMatchEnd)
                {
                    gangResult.GangBlock = gangBlock;

                    // repeat till we arrive at 1 here then finish
                    if (gangGroupSettings.Gang?.Position > 1)
                    {
                        previousSybling.ScanForGangMembers(
                            groups,
                            previousSyblingIndex,
                            gangResult,
                            results,
                            n,
                            gangBlock
                        );
                    }
                    else
                    {
                        break;
                    }

                    // which gangMember was this?
                    // Then we have a gang
                    // So need to keep the result
                    // Now check the gang again for previous groups and repeat until a blank is drawn.
                    // If it is it a valid gang create a gangblock and set it as a property on all the results
                    // Then process as normal
                    // As the results are turned into groups add them to the gang block
                    // When reaching the beginning of the gang (marked in the results) check that all the gangs
                    // children point to the same parent. Check that there are no siblings in between them
                    // Then remove them from that parent and add the gang instead to the parent
                    // Then reset all depths
                }
            }
        }
    }
}
