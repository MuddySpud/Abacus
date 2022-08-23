using MuddySpud.Metrics.Engine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpGroupUnitsInterpreter
    {
        public static void ProcessUnits(
            this BlockStats group,
            BlockStatsCache blockStatsCache,
            GroupSettings groupSettings,
            IBoundaryCounter boundaryCounter,
            string text)
        {
            if (groupSettings.Units.Count == 0)
            {
                return;
            }

            BlockStats? parent = null;
            BlockStats block;

            // The last code block must hold the units as children
            // Find the startlocattion and endlocation then find the block that matches these
            // Then use these are the parent block etc...
            for (int m = group.ChildBlocks.Count - 1; m >= 0; m--)
            {
                block = group.ChildBlocks[m];

                if (block.Settings.BlockType == "Code")
                {
                    parent = block;

                    break;
                }
            }

            if (parent is null)
            {
                throw new NotImplementedException(nameof(parent));
            }

            int matchEnd;
            int matchStart;
            Regex regex;
            Regex rejectMatchRegex;
            MatchCollection matches;
            List<BlockStats> units = new();
            BlockStats unit;
            int depth = group.Depth + 1;
            string groupText = String.Empty;

            if (parent.CloseIndex > 0)
            {
                groupText = text.Substring(parent.OpenIndex, parent.CloseIndex - parent.OpenIndex);
            }

            // Create groups for each unit add to a list along with match start and block start
            // order units by match start
            // Start at end of parent block look for last unit, parent block end is its end
            // Look for next unit, previous match start is its block end
            // etc
            // Find all blocks within each unit, add them to unit
            // replace their parent with unit
            // repace group children with unit list
            // calculate all properties etc
            // calculate all depths
            // add units to blockscache

            Match match;
            bool skipMatch = false;
            char c;
            BlockStats lastUnit;

            List<char> lastCodeCloseCharacters = new();

            foreach (BlockBoundarySettings boundarySettings in blockStatsCache.CodeBlockSettings.Boundaries)
            {
                if (boundarySettings.BlockType == "Code")
                {
                    lastCodeCloseCharacters.Add(boundarySettings.Close[^1]);
                }
            }

            foreach (GroupUnitSettings unitSettings in groupSettings.Units)
            {
                foreach (PatternSettings patternSettings in unitSettings.Patterns)
                {
                    regex = new Regex(patternSettings.RegexPattern);
                    matches = regex.Matches(groupText);

                    for (int i = 0; i < matches.Count; i++)
                    {
                        match = matches[i];

                        if (patternSettings.RejectMatchRegexPattern is { })
                        {
                            rejectMatchRegex = new Regex(patternSettings.RejectMatchRegexPattern);

                            if (rejectMatchRegex.IsMatch(match.Value))
                            {
                                continue;
                            }
                        }

                        matchStart = parent.OpenIndex + match.Index;

                        if (units.Count == 0)
                        {
                            // Then it will be the first units
                            // and there must be only whitespace between it and the start of the parent block
                            for (int j = parent.OpenIndex + 1; j < matchStart; j++)
                            {
                                c = text[j];

                                if (!Char.IsWhiteSpace(c))
                                {
                                    skipMatch = true;

                                    break;
                                }
                            }

                            if (skipMatch)
                            {
                                parent.Errors.Add(
                                    new("SwitchStatement", "Case not as expected.")
                                );
                            }
                        }
                        else
                        {
                            int k = matchStart - 1;

                            // unit must follow a code statement end or a code block end
                            for (; k > parent.OpenIndex; k--)
                            {
                                c = text[k];

                                if (Char.IsWhiteSpace(c))
                                {
                                    continue;
                                }
                                else if (lastCodeCloseCharacters.Contains(c))
                                {
                                    break;
                                }
                                else
                                {
                                    skipMatch = true;

                                    break;
                                }
                            }

                            if (skipMatch)
                            {
                                skipMatch = false;

                                // It can also end with the previous units match end
                                // ie cascading case statements
                                if (units.Count == 0
                                    || units[^1].MatchEnd != k + 1)
                                {
                                    continue;
                                }
                            }

                            // unit cannot be a child of a child block
                            foreach (BlockStats child in parent.ChildBlocks)
                            {
                                if (child.Settings.Model == "Statement")
                                {
                                    continue;
                                }

                                if (matchStart >= child.OpenIndex
                                    && matchStart <= child.CloseIndex)
                                {
                                    skipMatch = true;
                                }
                            }
                        }

                        if (skipMatch)
                        {
                            skipMatch = false;

                            continue;
                        }

                        foreach (BlockStats u in units)
                        {
                            if (matchStart >= u.MatchStart
                                && matchStart <= u.MatchEnd)
                            {
                                skipMatch = true;
                            }
                        }

                        if (skipMatch)
                        {
                            skipMatch = false;

                            continue;
                        }

                        matchEnd = matchStart + match.Value.Length;

                        if (units.Count > 0)
                        {
                            lastUnit = units[^1];

                            if (lastUnit.CloseIndex == 0)
                            {
                                lastUnit.CloseIndex = matchStart - 1;
                            }
                        }

                        unit = new(
                            group,
                            matchEnd,
                            depth,
                            new GroupUnitBoundarySettings(unitSettings.Name)
                        );

                        unit.MatchStart = matchStart;
                        unit.MatchEnd = matchEnd;
                        units.Add(unit);
                    }
                }
            }

            units.Sort(BlockStatsExtensions.Compare);
            int last = parent.CloseIndex;

            for (int i = units.Count - 1; i >= 0; i--)
            {
                unit = units[i];

                if (unit.CloseIndex == 0)
                {
                    unit.CloseIndex = last;
                }
                else
                {
                    break;
                }
            }

            for (int j = 0; j < units.Count; j++)
            {
                unit = units[j];

                for (int k = parent.ChildBlocks.Count - 1; k >= 0; k--)
                {
                    block = parent.ChildBlocks[k];

                    if (block.OpenIndex >= unit.OpenIndex
                        && block.CloseIndex <= unit.CloseIndex)
                    {
                        unit.ChildBlocks.Add(block);
                        block.Parent = unit;
                        parent.ChildBlocks.RemoveAt(k);
                    }
                }

                unit.BuildGroupUnitProperties(
                    boundaryCounter,
                    groupSettings,
                    text
                );

                blockStatsCache.BlockStats.Add(unit);
            }

            for (int i = 0; i < units.Count; i++)
            {
                group.ChildBlocks.Add(units[i]);
            }

            group.ResetDepth();
        }
    }
}
