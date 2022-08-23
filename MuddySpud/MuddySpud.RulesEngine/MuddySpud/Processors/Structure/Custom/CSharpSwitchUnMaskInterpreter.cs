using MuddySpud.Metrics.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpSwitchUnMaskInterpreter
    {
        internal static void UnMaskSwitchExpression(
            this BlockStatsCache blockStatsCache,
            string code,
            IBoundaryCounter boundaryCounter)
        {
            blockStatsCache.RootBlockStats.ScanForSwitchExpressions(
                blockStatsCache,
                boundaryCounter,
                code
            );
        }

        private static void ScanForSwitchExpressions(
            this BlockStats block,
            BlockStatsCache blockStatsCache,
            IBoundaryCounter boundaryCounter,
            string code)
        {
            BlockStats codeBlock;

            foreach (BlockStats child in block.ChildBlocks)
            {
                if (child.Name == "switch expression"
                    && child.Type == "Branch"
                    && child.Settings.Model == "Group")
                {
                    if (child.ChildBlocks.Count != 1)
                    {
                        throw new NotImplementedException("This should be a group block. It should have a single child block representing the code block");
                    }

                    codeBlock = child.ChildBlocks[0];
                    string content = String.Empty;

                    if (codeBlock.CloseIndex > 0)
                    {
                        content = code.Substring(
                            codeBlock.OpenIndex,
                            codeBlock.CloseIndex - codeBlock.OpenIndex
                        );
                    }

                    codeBlock.UnMaskSwitchExpression(
                        blockStatsCache,
                        boundaryCounter,
                        content,
                        code
                    );

                    codeBlock.ResetDepthAndCache(blockStatsCache);

                    continue;
                }

                child.ScanForSwitchExpressions(
                    blockStatsCache,
                    boundaryCounter,
                    code
                );
            }
        }

        private static void CheckForNestedSwitch(
            this BlockStats block,
            BlockStatsCache blockStatsCache,
            IBoundaryCounter boundaryCounter,
            string code)
        {
            string blockContent = String.Empty;

            if (block.CloseIndex > 0)
            {
                blockContent = code.Substring(
                    block.OpenIndex,
                    block.CloseIndex - block.OpenIndex
                );
            }

            string pattern = "\\bswitch\\s*{";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(blockContent);

            if (matches.Count == 0)
            {
                return;
            }

            foreach (Match match in matches)
            {
                int matchEnd = match.Index + match.Value.Length + block.OpenIndex;
                BlockStats stats;

                for (int i = 0; i < block.ChildBlocks.Count; i++)
                {
                    stats = block.ChildBlocks[i];

                    if (stats.Settings.BlockType != "Code")
                    {
                        continue;
                    }

                    if (stats.OpenIndex == matchEnd)
                    {
                        stats.Name = "switch expression";
                        stats.FullName = stats.Name;
                        stats.Type = "Branch";

                        string statsContent = String.Empty;

                        if (stats.CloseIndex > 0)
                        {
                            statsContent = code.Substring(
                                stats.OpenIndex,
                                stats.CloseIndex - stats.OpenIndex
                            );
                        }

                        stats.UnMaskSwitchExpression(
                            blockStatsCache,
                            boundaryCounter,
                            statsContent,
                            code
                        );

                        stats.ResetDepthAndCache(blockStatsCache);

                        continue;

                    }
                }
            }
        }

        private static void UnMaskSwitchExpression(
            this BlockStats parent,
            BlockStatsCache blockStatsCache,
            IBoundaryCounter boundaryCounter,
            string content,
            string code)
        {
            StringBuilder caseGuardMaskedContent = new(content);

            // Need to find code and parameter blocks but not statements and replace their non-whitespace char contents with x
            // Then replace any case guards
            // Then look for the last comma before the next lambda and call that end
            // And the last lambda that ends with parent block
            // expression arm
            // ONLY Need to run this as it will find all blocks even nested ones during that search
            CodeBlockSettings codeBlockSettings = new();

            foreach (BlockBoundarySettings boundary in blockStatsCache.CodeBlockSettings.AllBoundaries)
            {
                if (boundary.Name == "Curly"
                    || boundary.Name == "Round")
                {
                    codeBlockSettings.Boundaries.Add(boundary);
                }
            }

            CodeBlockLoopCache codeBlockLoopCache = new(
                parent,
                codeBlockSettings
            );

            caseGuardMaskedContent.ScanForBlocks(
                codeBlockLoopCache,
                codeBlockLoopCache.Opens,
                codeBlockLoopCache.OpenFirstChars
            );

            codeBlockLoopCache.RootCodeBlock.UpdateIndices(parent.OpenIndex);

            if (parent.ChildBlocks.Count > 0
                && parent.ChildBlocks[^1].CloseIndex == 0)
            {
                parent.ChildBlocks[^1].CloseIndex = parent.CloseIndex;
            }

            codeBlockLoopCache.BuildStats(
                boundaryCounter,
                code
            );

            BlockStatsCache childBlockStatsCache = new(
                blockStatsCache.FileStructureSettings,
                codeBlockSettings,
                parent,
                codeBlockLoopCache.BlockStats,
                blockStatsCache.StringOutputBoundaries,
                blockStatsCache.LineStarts
            );

            childBlockStatsCache.FindGroups(
                content,
                boundaryCounter,
                new GroupBoundarySettingsBuilder(),
                parent.OpenIndex,
                code
            );

            parent.CheckForNestedSwitchExpressions(
                childBlockStatsCache,
                boundaryCounter,
                code
            );

            parent.UnMaskSwitchExpression(
                blockStatsCache,
                boundaryCounter,
                caseGuardMaskedContent,
                content,
                code
            );
        }

        private static void CheckForNestedSwitchExpressions(
            this BlockStats parent,
            BlockStatsCache blockStatsCache,
            IBoundaryCounter boundaryCounter,
            string code)
        {
            BlockStats codeBlock;

            foreach (BlockStats child in parent.ChildBlocks)
            {
                if (child.Name == "switch expression"
                    && child.Type == "Branch"
                    && child.Settings.Model == "Group")
                {
                    if (child.ChildBlocks.Count != 1)
                    {
                        throw new NotImplementedException("This should be a group block. It should have a single child block representing the code block");
                    }

                    codeBlock = child.ChildBlocks[0];

                    string content = string.Empty;

                    if (codeBlock.CloseIndex > 0)
                    {
                        content = code.Substring(
                            codeBlock.OpenIndex,
                            codeBlock.CloseIndex - codeBlock.OpenIndex
                        );
                    }

                    StringBuilder caseGuardMaskedContent = new(content);

                    codeBlock.UnMaskSwitchExpression(
                        blockStatsCache,
                        boundaryCounter,
                        caseGuardMaskedContent,
                        content,
                        code
                    );
                }

                child.CheckForNestedSwitchExpressions(
                    blockStatsCache,
                    boundaryCounter,
                    code
                );
            }
        }

        private static void UnMaskSwitchExpression(
            this BlockStats parent,
            BlockStatsCache blockStatsCache,
            IBoundaryCounter boundaryCounter,
            StringBuilder caseGuardMaskedContent,
            string content,
            string code)
        {

            blockStatsCache.RootBlockStats.UpdateChildrensProperties(boundaryCounter);
            caseGuardMaskedContent.MaskOutChildBlocks(parent);
            caseGuardMaskedContent.MaskOutCaseGuards(content);

            List<BlockStats> switchArmBlocks = caseGuardMaskedContent.FindExpressionArms(
                parent,
                boundaryCounter,
                code
            );

            if (switchArmBlocks.Count == 0)
            {
                return;
            }

            BlockStats child;
            BlockStats armBlock = switchArmBlocks[0];
            int j = 0;

            for (int i = 0; i < parent.ChildBlocks.Count; i++)
            {
                child = parent.ChildBlocks[i];

                while (j < switchArmBlocks.Count)
                {
                    if (child.OpenIndex >= armBlock.MatchStart
                        && child.CloseIndex <= armBlock.CloseIndex)
                    {
                        armBlock.ChildBlocks.Add(child);
                        child.Parent = armBlock;

                        break;
                    }
                    else
                    {
                        armBlock = switchArmBlocks[++j];
                    }
                }
            };

            parent.ChildBlocks = switchArmBlocks;

            for (int k = 0; k < switchArmBlocks.Count; k++)
            {
                switchArmBlocks[k].CheckForNestedSwitch(
                    blockStatsCache,
                    boundaryCounter,
                    code
                );
            }
        }

        private static List<BlockStats> FindExpressionArms(
            this StringBuilder content,
            BlockStats parent,
            IBoundaryCounter boundaryCounter,
            string code)
        {
            int commaIndex = 0;
            bool open = false;
            BlockStats? switchArm = null;
            List<BlockStats> switchArmBlocks = new();
            string type = "SwitchArm";

            for (int i = 0; i < content.Length; i++)
            {
                if (!open
                    && content[i] == '='
                    && content[i + 1] == '>')
                {
                    switchArm = new(
                        parent,
                        i + 2 + parent.OpenIndex,
                        parent.Depth + 1,
                        new UnMaskBoundarySettings(type)
                    );

                    switchArm.MatchStart = commaIndex + parent.OpenIndex + 1;
                    switchArm.MatchEnd = switchArm.OpenIndex - 2;
                    switchArmBlocks.Add(switchArm);
                    open = true;
                }
                else if (content[i] == ',')
                {
                    commaIndex = i;

                    if (switchArm is { })
                    {
                        switchArm.CloseIndex = commaIndex + parent.OpenIndex;
                        open = false;
                    }
                }
            }

            if (switchArmBlocks.Count == 0)
            {
                return switchArmBlocks;
            }

            switchArmBlocks[^1].CloseIndex = content.Length + parent.OpenIndex;

            for (int i = 0; i < switchArmBlocks.Count; i++)
            {
                if (switchArmBlocks[i].CloseIndex == 0)
                {
                    throw new NotImplementedException("CloseIndex cannot be zero for a child block");
                }

                switchArmBlocks[i].BuildUnMaskProperties(
                    boundaryCounter,
                    "Branch",
                    code
                );
            }

            return switchArmBlocks;
        }

        private static void MaskOutCaseGuards(
            this StringBuilder content,
            string code)
        {
            string pattern = "when.+?(?=(=>))";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(code);
            int matchEnd;

            foreach (Match match in matches)
            {
                matchEnd = match.Index + match.Value.Length;

                for (int i = match.Index; i < matchEnd; i++)
                {
                    if (!Char.IsWhiteSpace(content[i]))
                    {
                        content[i] = 'x';
                    }
                }
            }
        }

        private static void UpdateChildrensProperties(
            this BlockStats block,
            IBoundaryCounter boundaryCounter)
        {
            foreach (BlockStats child in block.ChildBlocks)
            {
                child.BuildProperties(boundaryCounter);
                child.UpdateChildrensProperties(boundaryCounter);
            }
        }

        private static void MaskOutChildBlocks(
            this StringBuilder content,
            BlockStats parent)
        {
            int start;
            int stop;

            foreach (BlockStats child in parent.ChildBlocks)
            {
                start = child.OpenIndex - parent.OpenIndex;
                stop = child.CloseIndex - parent.OpenIndex;

                for (int i = start; i < stop; i++)
                {
                    if (!Char.IsWhiteSpace(content[i]))
                    {
                        content[i] = 'x';
                    }
                }
            }
        }
    }
}
