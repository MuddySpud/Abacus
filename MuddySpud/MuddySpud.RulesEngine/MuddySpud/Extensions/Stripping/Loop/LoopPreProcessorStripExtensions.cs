using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public static class LoopPreProcessorStripExtensions
    {
        public static int ProcessPreProcessors(
            this StringBuilder content,
            PreProcessorStripLoopCache cache,
            int index)
        {
            string preProcessor = cache.PreProcessors[0];
            int commentStartIndex = index;
            int startSearchIndex = commentStartIndex;
            int contentLength = content.Length;
            MtBoundary preProcessorBoundary;

            int lineEndIndex = content.IndexOf(
                '\n',
                commentStartIndex + 1) + 1; // include the \n

            if (lineEndIndex < 1)
            {
                lineEndIndex = content.Length - 1;
            }

            int lineStartIndex = content.LastIndexOf(
                '\n',
                commentStartIndex - 1);

            if (lineStartIndex < 0)
            {
                lineStartIndex = 0;
            }

            lineStartIndex++;

            int lineLength = lineEndIndex - lineStartIndex;
            int adjustedCommentStartIndex = lineStartIndex + cache.OutputBoundaries.Adjustment;

            string preProcess = content.ToString(
                lineStartIndex,
                lineLength
            );

            cache.PreProcessorContent.AppendLine(preProcess);

            // Remove preprocessor line from content
            content.Remove(
                lineStartIndex,
                lineLength
            );

            // Adjust the startIndex so it relates to the FullContent
            // not the content which might have had phrases removed
            preProcessorBoundary = new MtBoundary(
                adjustedCommentStartIndex,
                lineLength,
                "preProcessor"
            );

            cache.OutputBoundaries.Boundaries.Add(preProcessorBoundary);

            // Include the length of the removed string to the adjuster
            cache.OutputBoundaries.Adjustment += lineLength;

            return lineStartIndex;
        }
    }
}
