using MuddySpud.RulesEngine;
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
    static class CodeBlockStringBuilderExtensions
    {
        public static void CountCodeBlocks(
            this StringBuilder content,
            CodeBlockLoopCache cache,
            IBoundaryCounter boundaryCounter)
        {
            StringBuilder maskedContent = content.MaskContent(cache);
            cache.Current = cache.RootCodeBlock;
            cache.RootCodeBlock.Flags.Clear();
            cache.RootCodeBlock.ChildBlocks.Clear();
            cache.RootCodeBlock.ChildStatements.Clear();
            cache.UnClosed.Clear();
            cache.BlockStats.Clear();

            maskedContent.ScanForBlocks(
                cache,
                cache.Opens,
                cache.OpenFirstChars
            );

            List<StatementStats> statementStats = content.ScanForStatements(cache);

            cache.BuildStats(
                boundaryCounter,
                content.ToString(),
                statementStats
            );
        }
    }
}
