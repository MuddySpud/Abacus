using MuddySpud.Metrics.Engine;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpUnMaskInterpreter
    {
        internal static void UnMask(
            this BlockStatsCache blockStatsCache,
            string text,
            IBoundaryCounter boundaryCounter,
            IBoundarySettingsBuilder boundarySettingsBuilder)
        {
            foreach (UnMaskSettings unMaskSettings in blockStatsCache.FileStructureSettings.UnMasking)
            {
                blockStatsCache.UnMask(
                    text,
                    unMaskSettings,
                    boundaryCounter
                );
            }

            blockStatsCache.BlockStats.Sort(BlockStatsExtensions.Compare);
        }

        private static void UnMask(
            this BlockStatsCache blockStatsCache,
            string text,
            UnMaskSettings unMaskSettings,
            IBoundaryCounter boundaryCounter)
        {
            if (unMaskSettings.ActionName == "unMaskSwitchExpression")
            {
                blockStatsCache.UnMaskSwitchExpression(
                    text,
                    boundaryCounter
                );
            }
        }
    }
}
