using MuddySpud.RulesEngine;
using System.Collections.Generic;

namespace MuddySpud.Metrics.Engine
{
    public class CodeBlockLoopCache
    {
        public BlockStats RootCodeBlock { get; }
        public CodeBlockSettings CodeBlockSettings { get; }
        public BlockStats Current { get; set; }
        public List<BlockStats> UnClosed { get; }
        public List<BlockStats> BlockStats { get; }
        public string[] Opens { get; }
        public string[] Ends { get; }
        public char[] OpenFirstChars { get; }
        public char[] EndFirstChars { get; }

        public CodeBlockLoopCache(
            BlockStats codeBlock,
            CodeBlockSettings codeBlockSettings)
        {
            RootCodeBlock = codeBlock;
            Current = codeBlock;
            CodeBlockSettings = codeBlockSettings;
            BlockStats = new();

            int boundaryCount = codeBlockSettings.Boundaries.Count;
            List<string> opens = new();
            List<char> openFirstChars = new();
            UnClosed = new();

            BlockBoundarySettings blockBoundarySettings;

            for (int i = 0; i < boundaryCount; i++)
            {
                blockBoundarySettings = codeBlockSettings.Boundaries[i];
                opens.Add(blockBoundarySettings.Open);

                if (!openFirstChars.Contains(blockBoundarySettings.Open[0]))
                {
                    openFirstChars.Add(blockBoundarySettings.Open[0]);
                }
            }

            int statementCount = codeBlockSettings.Statements.Count;
            string[] ends = new string[statementCount];
            List<char> endFirstChars = new();

            StatementBoundarySettings statementSettings;

            for (int i = 0; i < statementCount; i++)
            {
                statementSettings = codeBlockSettings.Statements[i];
                ends[i] = statementSettings.Close;

                if (!endFirstChars.Contains(statementSettings.Close[0]))
                {
                    endFirstChars.Add(statementSettings.Close[0]);
                }
            }

            Opens = opens.ToArray();
            Ends = ends;
            OpenFirstChars = openFirstChars.ToArray();
            EndFirstChars = endFirstChars.ToArray();
        }
    }
}
