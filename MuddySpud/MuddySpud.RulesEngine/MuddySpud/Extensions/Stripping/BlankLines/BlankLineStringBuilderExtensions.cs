using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public static class BlankLineStringBuilderExtensions
    {
        public static void StripBlankLines(
            this StringBuilder content,
            BlankLineStripLoopCache cache)
        {
            // Start at zero then look for nonwhitepsace then look for new line then add one then look for nonwhitespace

            int startLineIndex = 0;
            int endLineIndex;
            int lineLength;
            char c;
            MtBoundary blankLineBoundary;

            for (int i = startLineIndex; i < content.Length; i++)
            {
                c = content[i];

                if (!Char.IsWhiteSpace(c))
                {
                    // Reached a non-whitespace character 
                    // This line is not blank
                    // Find the end then start search again at next character (on a new line)

                    endLineIndex = content.IndexOf(
                        '\n',
                        i);

                    if (endLineIndex < 0)
                    {
                        break;
                    }

                    startLineIndex = endLineIndex + 1;
                    i = endLineIndex; // Will be incremented by i++
                }
                else if (c == '\n')
                {
                    // Reached then end of line without encountering a non-whitespace character
                    // It is a blank line - remove it
                    endLineIndex = i;
                    lineLength = endLineIndex - startLineIndex + 1;

                    // Remove line from content
                    content.Remove(
                        startLineIndex,
                        lineLength
                    );

                    // Adjust the startLineIndex so it relates to the FullContent
                    // not the content which might have had phrases removed
                    blankLineBoundary = new MtBoundary(
                        startLineIndex,
                        lineLength,
                        "blankLine"
                    );

                    cache.MergeInBoundary(blankLineBoundary);

                    // Include the length of the removed string to the adjuster
                    cache.OutputAdjustment += blankLineBoundary.Length;
                    i -= lineLength;
                }
            }

            cache.CompleteMergeInBoundaries();
        }

        private static void MergeInBoundary(
            this BlankLineStripLoopCache cache,
            MtBoundary blankLineBoundary)
        {
            int i = cache.InputCounter;
            MtBoundary commentBoundary;
            int adjustedIndex;

            for (; i < cache.InputBoundaries.Count; i++)
            {
                commentBoundary = cache.InputBoundaries[i];
                cache.InputCounter = i;
                adjustedIndex = blankLineBoundary.Index + cache.OutputAdjustment + cache.InputAdjustment;

                if (commentBoundary.Index > adjustedIndex)
                {
                    break;
                }

                cache.OutputBoundaries.Add(commentBoundary);
                cache.InputAdjustment += commentBoundary.Length;
                cache.InputCounter++;

                if (commentBoundary.Index == blankLineBoundary.Index)
                {
                    break;
                }
            }

            cache.AddLineBoundary(blankLineBoundary);
        }

        public static void CompleteMergeInBoundaries(this BlankLineStripLoopCache cache)
        {
            int i = cache.InputCounter;

            for (; i < cache.InputBoundaries.Count; i++)
            {
                cache.OutputBoundaries.Add(cache.InputBoundaries[i]);
            }
        }

        public static void AddLineBoundary(
            this BlankLineStripLoopCache cache,
            MtBoundary blockBoundary)
        {
            blockBoundary.Index += cache.InputAdjustment + cache.OutputAdjustment;
            cache.OutputBoundaries.Add(blockBoundary);
        }
    }
}
