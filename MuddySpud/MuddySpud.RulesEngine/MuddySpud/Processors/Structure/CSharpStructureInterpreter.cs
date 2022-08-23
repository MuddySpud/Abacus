using MuddySpud.Metrics.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuddySpud.RulesEngine
{
    public class CSharpStructureInterpreter : IFileStructureInterpreter
    {
        private StringBuilder _cleanedBuilder = new();

        public CSharpStructureInterpreter()
        {
        }

        public void StructureFile(
            BlockStatsCache blockStatsCache,
            string codeContent,
            IBoundaryCounter boundaryCounter,
            bool setBlockContent = false,
            string fullContent = "")
        {
            foreach (ComponentSettings componentSettings in blockStatsCache.FileStructureSettings.Components)
            {
                blockStatsCache.BlockStats.FindComponent(
                    componentSettings,
                    codeContent,
                    boundaryCounter,
                    _cleanedBuilder
                );
            }

            blockStatsCache.FindGroups(
                codeContent,
                boundaryCounter,
                new GroupBoundarySettingsBuilder()
            );

            blockStatsCache.UnMask(
                codeContent,
                boundaryCounter,
                new GroupBoundarySettingsBuilder()
            );

            blockStatsCache.FindUnitsOfWork(
                codeContent,
                boundaryCounter,
                new GroupBoundarySettingsBuilder()
            );

            blockStatsCache.RootBlockStats.BuildFullNames(
                String.Empty,
                blockStatsCache.FileStructureSettings.NameJoiner,
                fullContent,
                codeContent,
                setBlockContent
            );

            blockStatsCache.CountCharacters(
                codeContent,
                boundaryCounter
            );
        }
    }
}
