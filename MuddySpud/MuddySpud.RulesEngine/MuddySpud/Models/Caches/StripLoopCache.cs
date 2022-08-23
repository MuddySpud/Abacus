using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddySpud.Metrics.Engine
{
    public class StripLoopCache
    {
        public BlockCommentStripLoopCache BlockCommentCache { get; }
        public InlineCommentStripLoopCache InlineCommentCache { get; }
        public StringStripLoopCache StringCache { get; }
        public PreProcessorStripLoopCache PreProcessorCache { get; }


        public StripLoopCache(
            BlockCommentStripLoopCache blockCommentCache,
            InlineCommentStripLoopCache inlineCommentCache,
            StringStripLoopCache stringCache,
            PreProcessorStripLoopCache preProcessorCache)
        {
            BlockCommentCache = blockCommentCache;
            InlineCommentCache = inlineCommentCache;
            StringCache = stringCache;
            PreProcessorCache = preProcessorCache;
        }
    }
}
