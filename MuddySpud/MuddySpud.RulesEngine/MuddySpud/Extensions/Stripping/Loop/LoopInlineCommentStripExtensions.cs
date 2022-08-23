using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    static class LoopInlineCommentStripExtensions
    {
        public static int ProcessInlineComment(
            this StringBuilder content,
            InlineCommentStripLoopCache cache,
            int index)
        {
            int commentStartIndex = index;

            int commentEndIndex = content.IndexOf(
                '\n',
                commentStartIndex);

            if (commentEndIndex < 0)
            {
                commentEndIndex = content.Length - 1;
            }

            int commentLength = commentEndIndex - commentStartIndex;
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

            MtBoundary inlineBoundary = new MtBoundary(
                    adjustedCommentStartIndex,
                    commentLength,
                    "inlineComment"
                );

            cache.OutputBoundaries.Boundaries.Add(inlineBoundary);

            cache.Comments.Add(
                new(
                    adjustedCommentStartIndex * 10,
                    comment
                )
            );

            // Include the length of the removed string to the adjuster
            cache.OutputBoundaries.Adjustment += commentLength;

            return commentStartIndex;
        }
    }
}
