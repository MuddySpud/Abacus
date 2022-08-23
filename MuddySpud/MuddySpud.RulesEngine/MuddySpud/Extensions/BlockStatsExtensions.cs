using MuddySpud.RulesEngine;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public static class BlockStatsExtensions
    {
        public static int Compare(
            this BlockStats a,
            BlockStats b)
        {
            if (a.OpenIndex < b.OpenIndex)
            {
                return -1;
            }

            if (a.OpenIndex == b.OpenIndex)
            {
                if (a.CloseIndex == b.CloseIndex)
                {
                    return 0;
                }

                if (a.CloseIndex > b.CloseIndex)
                {
                    return -1;
                }
            }

            return 1;
        }

        internal static void ResetDepth(this BlockStats stats)
        {
            BlockStats child;

            for (int i = 0; i < stats.ChildBlocks.Count; i++)
            {
                child = stats.ChildBlocks[i];
                child.Depth++;
                child.ResetDepth();
            }
        }

        internal static void UpdateIndices(
            this BlockStats block,
            int offset)
        {
            BlockStats child;

            for (int i = 0; i < block.ChildBlocks.Count; i++)
            {
                child = block.ChildBlocks[i];
                child.OpenIndex += offset;
                child.CloseIndex += offset;

                if (child.MatchEnd > 0
                    || child.MatchStart > 0)
                {
                    child.MatchStart += offset;
                    child.MatchEnd += offset;
                }

                child.UpdateIndices(offset);
            }
        }

        internal static void ResetDepthAndCache(
            this BlockStats block,
            BlockStatsCache blockStatsCache)
        {
            BlockStats child;

            for (int i = 0; i < block.ChildBlocks.Count; i++)
            {
                child = block.ChildBlocks[i];
                child.Depth++;
                blockStatsCache.BlockStats.Add(child);
                child.ResetDepthAndCache(blockStatsCache);
            }
        }

        internal static void BuildGroupProperties(
            this BlockStats group,
            IBoundaryCounter boundaryCounter,
            GroupSettings groupSettings,
            string text)
        {
            group.BuildProperties(
                boundaryCounter,
                groupSettings.Type,
                text
            );

            group.Name = groupSettings.Name;
        }

        internal static void BuildGroupUnitProperties(
            this BlockStats group,
            IBoundaryCounter boundaryCounter,
            GroupSettings groupSettings,
            string text)
        {
            group.BuildProperties(
                boundaryCounter,
                groupSettings.Type,
                text
            );

            group.Name = group.Settings.Name;
            group.FullName = text.Substring(group.MatchStart, group.MatchEnd - group.MatchStart);
        }

        internal static void BuildGangProperties(
            this BlockStats group,
            IBoundaryCounter boundaryCounter,
            GroupSettings groupSettings,
            string text)
        {
            group.BuildProperties(
                boundaryCounter,
                groupSettings.Type,
                text
            );

            group.Name = group.Settings.Name;
        }

        internal static void BuildUnMaskProperties(
            this BlockStats block,
            IBoundaryCounter boundaryCounter,
            string type,
            string text)
        {
            block.BuildProperties(
                boundaryCounter,
                type,
                text
            );

            block.Name = text.Substring(block.MatchStart, block.MatchEnd - block.MatchStart).Trim();

            if (!string.IsNullOrWhiteSpace(block.Name))
            {
                block.FullName = block.Name;
            }
        }

        private static void BuildProperties(
            this BlockStats group,
            IBoundaryCounter boundaryCounter,
            string type,
            string text)
        {
            group.BuildProperties(boundaryCounter);
            group.Type = type;

            if (group.CloseIndex > 0)
            {
                group.Value = text.Substring(group.OpenIndex, group.CloseIndex - group.OpenIndex);
            }
        }

        private static void BuildProperties(
            this BlockStats group,
            IBoundaryCounter boundaryCounter,
            StringBuilder content)
        {
            group.BuildProperties(boundaryCounter);

            if (group.CloseIndex > 0)
            {
                group.Value = content.ToString(group.OpenIndex, group.CloseIndex - group.OpenIndex);
            }
        }

        internal static void BuildProperties(
            this BlockStats block,
            IBoundaryCounter boundaryCounter)
        {
            block.AdjustedOpenIndex = boundaryCounter.GetFullIndexFromCodeIndex(block.OpenIndex);
            block.BlockStartLocation = boundaryCounter.GetLocation(block.AdjustedOpenIndex);

            if (block.CloseIndex > 0)
            {
                block.AdjustedCloseIndex = boundaryCounter.GetFullIndexFromCodeIndex(block.CloseIndex);
                block.BlockEndLocation = boundaryCounter.GetLocation(block.AdjustedCloseIndex);
            }

            if (block.MatchStart > 0)
            {
                block.AdjustedMatchStart = boundaryCounter.GetFullIndexFromCodeIndex(block.MatchStart);
                block.MatchStartLocation = boundaryCounter.GetLocation(block.AdjustedMatchStart);
            }

            if (block.MatchEnd > 0)
            {
                block.AdjustedMatchEnd = boundaryCounter.GetFullIndexFromCodeIndex(block.MatchEnd);
                block.MatchEndLocation = boundaryCounter.GetLocation(block.AdjustedMatchEnd);
            }
        }
    }
}
