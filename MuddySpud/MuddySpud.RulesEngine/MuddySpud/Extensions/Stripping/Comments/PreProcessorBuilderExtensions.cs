//using Microsoft.ApplicationInspector.RulesEngine;
//using MuddySpud.RulesEngine.Commands;
//using System;
//using System.Text;

//namespace MuddySpud.Metrics.Engine
//{
//    static class PreProcessorBuilderExtensions
//    {
//        public static void MultiStripPreProcessors(
//            this StringBuilder content,
//            PreProcessorStripLoopCache cache)
//        {
//            content.StripPreProcessors(
//                cache,
//                PreProcessorIndexOfAny
//            );
//        }

//        public static void SingleStripPreProcessors(
//            this StringBuilder content,
//            PreProcessorStripLoopCache cache)
//        {
//            content.StripPreProcessors(
//                cache,
//                PreProcessorIndexOf
//            );
//        }

//        private static int PreProcessorIndexOfAny(
//            this StringBuilder content,
//            PreProcessorStripLoopCache cache,
//            string preProcessor,
//            int startSearchIndex)
//        {
//            (int commentStartIndex, _) = content.IndexOfAny(
//                cache.PreProcessorFirstChars,
//                cache.PreProcessors,
//                startSearchIndex
//            );

//            return commentStartIndex;
//        }

//        private static int PreProcessorIndexOf(
//            this StringBuilder content,
//            PreProcessorStripLoopCache _,
//            string preProcessor,
//            int startSearchIndex)
//        {
//            return content.IndexOf(
//                preProcessor,
//                startSearchIndex
//            );
//        }

//        private static void StripPreProcessors(
//            this StringBuilder content,
//            PreProcessorStripLoopCache cache,
//            Func<StringBuilder, PreProcessorStripLoopCache, string, int, int> indexOfDelegate)
//        {
//            string preProcessor = cache.PreProcessors[0];
//            int commentStartIndex = 0;
//            int lineStartIndex;
//            int lineEndIndex;
//            int lineLength;
//            string preProcess;
//            int startSearchIndex = commentStartIndex;
//            int contentLength = content.Length;
//            MtBoundary preProcessorBoundary;

//            while (startSearchIndex < contentLength)
//            {
//                commentStartIndex = indexOfDelegate(
//                    content,
//                    cache,
//                    preProcessor,
//                    startSearchIndex
//                );

//                if (commentStartIndex < 0)
//                {
//                    break;
//                }

//                lineEndIndex = content.IndexOf(
//                    '\n',
//                    commentStartIndex + 1);

//                lineStartIndex = content.LastIndexOf(
//                    '\n',
//                    commentStartIndex - 1);

//                lineStartIndex++;
//                lineEndIndex++;

//                if (lineEndIndex < 0)
//                {
//                    break;
//                }
//                else if (lineStartIndex < 0)
//                {
//                    lineStartIndex = 0;
//                }

//                lineLength = lineEndIndex - lineStartIndex;

//                preProcess = content.ToString(
//                    lineStartIndex,
//                    lineLength
//                );

//                cache.PreProcessorContent.AppendLine(preProcess);

//                // Remove preprocessor line from content
//                content.Remove(
//                    lineStartIndex,
//                    lineLength
//                );

//                // Adjust the startIndex so it relates to the FullContent
//                // not the content which might have had phrases removed
//                preProcessorBoundary = new MtBoundary(
//                    lineStartIndex,
//                    lineLength,
//                    "preProcessor"
//                );

//                cache.MergeInBoundary(preProcessorBoundary);

//                // Include the length of the removed string to the adjuster
//                cache.OutputAdjustment += preProcessorBoundary.Length;

//                // Set the next search to start where this line started (it will be removed in next loop) 
//                startSearchIndex = lineStartIndex;
//            }

//            cache.CompleteMergeInBoundaries();
//        }

//        private static void MergeInBoundary(
//            this PreProcessorStripLoopCache cache,
//            MtBoundary blockBoundary)
//        {
//            int i = cache.InputCounter;
//            MtBoundary inlineBoundary;
//            int adjustedIndex;

//            for (; i < cache.InputBoundaries.Count; i++)
//            {
//                inlineBoundary = cache.InputBoundaries[i];
//                cache.InputCounter = i;
//                adjustedIndex = blockBoundary.Index + cache.OutputAdjustment + cache.InputAdjustment;

//                if (inlineBoundary.Index > adjustedIndex)
//                {
//                    break;
//                }

//                cache.OutputBoundaries.Add(inlineBoundary);
//                cache.InputAdjustment += inlineBoundary.Length;
//                cache.InputCounter++;

//                if (inlineBoundary.Index == blockBoundary.Index)
//                {
//                    break;
//                }
//            }

//            cache.AddBlockBoundary(blockBoundary);
//        }

//        public static void CompleteMergeInBoundaries(this PreProcessorStripLoopCache cache)
//        {
//            int i = cache.InputCounter;

//            for (; i < cache.InputBoundaries.Count; i++)
//            {
//                cache.OutputBoundaries.Add(cache.InputBoundaries[i]);
//            }
//        }

//        public static void AddBlockBoundary(
//            this PreProcessorStripLoopCache cache,
//            MtBoundary blockBoundary)
//        {
//            blockBoundary.Index += cache.InputAdjustment + cache.OutputAdjustment;
//            cache.OutputBoundaries.Add(blockBoundary);
//        }
//    }
//}
