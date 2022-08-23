using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpGroupsInterpreter
    {
        internal static void FindGroups(
            this BlockStatsCache blockStatsCache,
            string text,
            IBoundaryCounter boundaryCounter,
            IBoundarySettingsBuilder boundarySettingsBuilder,
            int offset = 0,
            string fullText = "")
        {
            List<GroupSettings> groups = blockStatsCache.FileStructureSettings.Groups;

            if (groups.Count() == 0)
            {
                return;
            }

            if (String.IsNullOrEmpty(fullText))
            {
                fullText = text;
            }

            List<GroupSearchMatch> results = new();
            Regex regex;
            MatchCollection matches;
            GroupSearchMatch result;
            string pattern;

            // Need to do separate regex for each groupSettings and each block.Open
            // Then collect the results in a list and order by match.Index
            foreach (GroupSettings settings in groups)
            {
                List<BlockBoundarySettings> firstBlockSettings = GetKeywordBlockSettings(
                    settings,
                    blockStatsCache.CodeBlockSettings.Boundaries
                );

                string escape;
                string openings = String.Empty;
                BlockBoundarySettings blockSettings;

                for (int i = 0; i < firstBlockSettings.Count; i++)
                {
                    blockSettings = firstBlockSettings[i];
                    escape = Regex.Escape(blockSettings.Open);

                    if (i == 0)
                    {
                        openings = escape;
                    }
                    else
                    {
                        openings += $"|{escape}";
                    }
                }

                pattern = $"\\b{settings.Keyword}\\s*({openings})";
                regex = new Regex(pattern);
                matches = regex.Matches(text);

                foreach (Match regexMatch in matches)
                {
                    result = new(
                            regexMatch.Index + offset,
                            firstBlockSettings[0].BlockType,
                            settings,
                            regexMatch
                        );

                    results.Add(result);
                }
            }

            results.Sort(
                (x, y) =>
                    x.MatchIndex.CompareTo(y.MatchIndex)
            );

            int matchEnd;
            BlockStats? group;
            int groupIndex;
            Match match;
            GroupSettings groupSettings;
            BlockStats stats;

            for (int j = results.Count - 1; j >= 0; j--)
            {
                result = results[j];
                match = result.Match;
                groupSettings = result.GroupSettings;
                matchEnd = result.MatchIndex + match.Value.Length;

                // To save time could start looking from where last one found...
                for (int k = 0; k < blockStatsCache.BlockStats.Count; k++)
                {
                    stats = blockStatsCache.BlockStats[k];

                    if (stats.Settings.BlockType != result.BlockType
                        || !String.IsNullOrWhiteSpace(stats.Name))
                    {
                        continue;
                    }

                    if (stats.OpenIndex == matchEnd)
                    {
                        (group, groupIndex) = BuildGroup(
                            result.GroupSettings,
                            stats,
                            result.MatchIndex,
                            boundarySettingsBuilder
                        );

                        if (group is null)
                        {
                            break;
                        }

                        blockStatsCache.BlockStats.Add(group);

                        group.BuildGroupProperties(
                            boundaryCounter,
                            groupSettings,
                            fullText
                        );

                        group.ProcessGang(
                            blockStatsCache,
                            groups,
                            groupIndex,
                            result,
                            results,
                            j,
                            boundaryCounter,
                            fullText
                        );

                        group.ProcessUnits(
                            blockStatsCache,
                            groupSettings,
                            boundaryCounter,
                            fullText
                        ); 

                        break;
                    }
                    else if (stats.OpenIndex > matchEnd)
                    {
                        break;
                    }
                }
            }

            blockStatsCache.BlockStats.Sort(BlockStatsExtensions.Compare);
        }

        private static List<BlockBoundarySettings> GetBoundarySettingsByType(
            string type,
            List<BlockBoundarySettings> settings)
        {
            List<BlockBoundarySettings> matches = new();

            foreach (BlockBoundarySettings boundarySettings in settings)
            {
                if (String.Equals(boundarySettings.BlockType, type, StringComparison.Ordinal))
                {
                    matches.Add(boundarySettings);
                }
            }

            if (matches.Count == 0)
            {
                throw new NotImplementedException($"There was no BlockBoundarySettings of type: {type}");
            }

            return matches;
        }

        private static List<StatementBoundarySettings> GetStatementSettingsByType(
            string type,
            List<StatementBoundarySettings> settings)
        {
            List<StatementBoundarySettings> matches = new();

            foreach (StatementBoundarySettings statementSettings in settings)
            {
                if (String.Equals(statementSettings.BlockType, type, StringComparison.Ordinal))
                {
                    matches.Add(statementSettings);
                }
            }

            return matches;
        }

        private static List<BlockBoundarySettings> GetKeywordBlockSettings(
            GroupSettings groupSettings,
            List<BlockBoundarySettings> settings)
        {
            if (groupSettings.Blocks.Count == 0)
            {
                throw new NotImplementedException("There must be at least one block");
            }

            // First block 
            string firstBlockName = groupSettings.Blocks[0];

            return GetBoundarySettingsByType(
                firstBlockName,
                settings
            );
        }

        private static (BlockStats? group, int index) BuildGroup(
            GroupSettings groupSettings,
            BlockStats stats,
            int matchStart,
            IBoundarySettingsBuilder boundarySettingsBuilder)
        {
            BlockStats? parent = stats.Parent;
            int depth = stats.Parent?.Depth ?? 0;
            List<BlockStats> resetChildren = new();

            BlockStats grouping = new(
                parent,
                matchStart,
                stats.Depth,
                boundarySettingsBuilder.Build(groupSettings)
            );

            if (parent is null)
            {
                throw new NotImplementedException("BlockStats parent cannot be null");
            }

            int index = 1;
            List<BlockStats> syblings = parent.ChildBlocks;
            BlockStats? sybling = null;
            BlockStats? previousSybling = null;
            int keywordBlockIndex = -1;

            for (int i = 0; i < syblings.Count; i++)
            {
                previousSybling = sybling;
                sybling = syblings[i];

                if (sybling == stats)
                {
                    keywordBlockIndex = i;

                    break;
                }

                resetChildren.Add(sybling);
            }

            if (keywordBlockIndex < 0)
            {
                throw new NotImplementedException("BlockStats not found in Parent BlockStats");
            }

            resetChildren.Add(grouping);
            grouping.ChildBlocks.Add(stats);
            stats.Parent = grouping;

            BlockStats? nextSybling = null;
            int nextSyblingIndex = keywordBlockIndex;
            string nextBlockSettingsType;

            while (index < groupSettings.Blocks.Count)
            {
                nextBlockSettingsType = groupSettings.Blocks[index];
                nextSyblingIndex = keywordBlockIndex + index;

                if (nextSyblingIndex < syblings.Count)
                {
                    nextSybling = syblings[nextSyblingIndex];
                }

                if (nextSybling is null)
                {
                    return (null, keywordBlockIndex);
                }

                if (nextSybling.Settings.BlockType != nextBlockSettingsType
                    && nextSybling.Settings.Model != "Group"
                    && nextSybling.Settings.Model != "Gang")
                {
                    return (null, keywordBlockIndex);
                }

                grouping.ChildBlocks.Add(nextSybling);
                nextSybling.Parent = grouping;
                index++;
            }

            grouping.CloseIndex = grouping.ChildBlocks[^1].CloseIndex;

            for (int j = nextSyblingIndex + 1; j < syblings.Count; j++)
            {
                resetChildren.Add(syblings[j]);
            }

            parent.ChildBlocks = resetChildren;
            grouping.ResetDepth();

            return (grouping, keywordBlockIndex);
        }
    }
}
