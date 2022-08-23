using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddySpud.Metrics.Engine
{
    public class BlockStatsCache
    {
        public FileStructureSettings FileStructureSettings { get; set; }
        public CodeBlockSettings CodeBlockSettings { get; }
        public BlockStats RootBlockStats { get; }
        public List<BlockStats> BlockStats { get; }
        public List<LocationIndex> UnitsOfWorkStart { get; } = new();
        public List<LineCounts> LineCountList { get; } = new();
        public List<MtBoundary> StringOutputBoundaries { get; }
        public List<int> LineStarts { get; }

        public BlockStatsCache(
            FileStructureSettings fileStructureSettings,
            CodeBlockSettings codeBlockSettings,
            BlockStats rootBlockStats,
            List<BlockStats> blockStats,
            List<MtBoundary> stringOutputBoundaries,
            List<int> lineStarts)
        {
            FileStructureSettings = fileStructureSettings;
            CodeBlockSettings = codeBlockSettings;
            RootBlockStats = rootBlockStats;
            BlockStats = blockStats;
            StringOutputBoundaries = stringOutputBoundaries;
            LineStarts = lineStarts;
        }
    }
}
