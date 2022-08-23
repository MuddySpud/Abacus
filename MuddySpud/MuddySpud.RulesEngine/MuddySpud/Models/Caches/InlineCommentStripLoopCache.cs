using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System.Collections.Generic;

namespace MuddySpud.Metrics.Engine
{
    public class InlineCommentStripLoopCache
    {
        //public int Adjustment { get; set; }
        public string InlineComment { get; set; }
        public List<CommentBoundary> Comments { get; } = new();
        public OutputBoundaries OutputBoundaries { get; }

        public InlineCommentStripLoopCache(
            string inlineComment,
            OutputBoundaries outputBoundaries)
        {
            InlineComment = inlineComment;
            OutputBoundaries = outputBoundaries;
        }
    }
}
