using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System.Collections.Generic;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    static class LoopBlockCommentStripExtensions
    {
        public static int ProcessBlockComment(
            this StringBuilder content,
            BlockCommentStripLoopCache cache,
            int index)
        {
            int suffixLength = cache.SuffixComment.Length;
            int prefixLength = cache.PrefixComment.Length;
            int commentStartIndex = index;
            int startSearchIndex = commentStartIndex + prefixLength;

            int commentEndIndex = content.IndexOf(
                cache.SuffixComment,
                startSearchIndex
            );

            int commentLength = commentEndIndex + suffixLength - commentStartIndex;

            if (commentEndIndex < 0)
            {
                commentEndIndex = content.Length - 1;
                commentLength = commentEndIndex - commentStartIndex;
            }

            int adjustedCommentStartIndex = commentStartIndex + cache.OutputBoundaries.Adjustment;

            string comment = content.ToString(
                commentStartIndex,
                commentLength
            );

            // Remove comment from content
            content.Remove(
                commentStartIndex,
                commentLength
            );

            MtBoundary blockBoundary = new MtBoundary(
                adjustedCommentStartIndex,
                commentLength,
                "blockComment"
            );

            cache.OutputBoundaries.Boundaries.Add(blockBoundary);

            cache.Comments.Add(
                new(
                    (adjustedCommentStartIndex * 10) + 1,
                    comment
                )
            );

            // Include the length of the removed string to the adjuster
            cache.OutputBoundaries.Adjustment += blockBoundary.Length;

            return commentStartIndex;
        }
    }
}
