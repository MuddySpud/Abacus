//using Microsoft.ApplicationInspector.RulesEngine;
//using MuddySpud.RulesEngine.Commands;
//using System.Collections.Generic;
//using System.Text;

//namespace MuddySpud.Metrics.Engine
//{
//    static class BlockCommentStringBuilderExtensions
//    {
//        public static void StripBlockComments(
//            this StringBuilder content,
//            BlockCommentStripLoopCache cache)
//        {
//            int suffixLength = cache.SuffixComment.Length;
//            int prefixLength = cache.PrefixComment.Length;
//            int commentStartIndex = content.IndexOf(cache.PrefixComment, 0);
//            int commentEndIndex = 0;
//            int commentLength;
//            int adjustedIndex;
//            string comment;
//            int startSearchIndex = commentStartIndex + prefixLength;
//            MtBoundary blockBoundary;
//            bool isShieldedByQuote;

//            while (commentStartIndex > -1)
//            {
//                isShieldedByQuote = content.CheckQuotes(
//                    cache,
//                    cache.PrefixComment,
//                    commentStartIndex
//                );

//                if (isShieldedByQuote)
//                {
//                    // Look for the next inline comment
//                    startSearchIndex += cache.PrefixComment.Length;

//                    commentStartIndex = content.IndexOf(
//                        cache.PrefixComment,
//                        startSearchIndex);

//                    continue;
//                }

//                commentEndIndex = content.IndexOf(
//                    cache.SuffixComment,
//                    startSearchIndex
//                );

//                if (commentEndIndex < 0)
//                {
//                    cache.Errors.Add(
//                        new("BlockComment", "Comment block prefix found with no matching suffix")
//                    );

//                    return;
//                }

//                commentLength = commentEndIndex + suffixLength - commentStartIndex;

//                if (commentLength < 0)
//                {
//                    cache.Errors.Add(
//                        new("BlockComment", "Comment block length was negative")
//                    );

//                    return;
//                }

//                comment = content.ToString(
//                    commentStartIndex,
//                    commentLength
//                );

//                // Remove comment from content
//                content.Remove(
//                    commentStartIndex,
//                    commentLength
//                );

//                blockBoundary = new MtBoundary(
//                    commentStartIndex,
//                    commentLength,
//                    "blockComment"
//                );

//                cache.MergeInBoundary(blockBoundary);

//                adjustedIndex = AdjustIndexNumber(
//                    cache,
//                    commentStartIndex
//                );

//                cache.CommentContent.Add(
//                    (adjustedIndex * 10) + 1,
//                    comment
//                );

//                // Include the length of the removed string to the adjuster
//                cache.OutputAdjustment += blockBoundary.Length;

//                // Set the next search to start where this comment started (it will be removed in next loop) 
//                startSearchIndex = commentStartIndex;

//                // Look for the next block comment prefix
//                commentStartIndex = content.IndexOf(
//                    cache.PrefixComment,
//                    startSearchIndex);

//                if (startSearchIndex < 0)
//                {
//                    break;
//                }
//            }

//            cache.CompleteMergeInBoundaries();
//        }

//        private static bool CheckQuotes(
//            this StringBuilder content,
//            BlockCommentStripLoopCache cache,
//            string commentMark,
//            int commentMarkStartIndex)
//        {
//            int inlineCommentEndIndex = commentMarkStartIndex + commentMark.Length;
//            int stopIndex = inlineCommentEndIndex > content.Length ? content.Length : inlineCommentEndIndex;

//            List<MtBoundary> emptyBoundaries = new();
//            List<MtBoundary> stringBoundaries = new();
//            StringBuilder copiedContent = new StringBuilder(content.ToString());
//            StringBuilder stringContent = new StringBuilder();

//            StringStripLoopCache stringCache = new(
//                emptyBoundaries,
//                stringBoundaries,
//                stringContent,
//                cache.StringSettings
//            );

//            copiedContent.LocateStrings(
//                stringCache,
//                stopIndex)
//            ;

//            foreach (MtBoundary stringBoundary in stringCache.OutputBoundaries)
//            {
//                if (stringBoundary.Index <= commentMarkStartIndex
//                    && (stringBoundary.Index + stringBoundary.Length) >= inlineCommentEndIndex)
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        private static void MergeInBoundary(
//            this BlockCommentStripLoopCache cache,
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

//        private static void CompleteMergeInBoundaries(this BlockCommentStripLoopCache cache)
//        {
//            int i = cache.InputCounter;

//            for (; i < cache.InputBoundaries.Count; i++)
//            {
//                cache.OutputBoundaries.Add(cache.InputBoundaries[i]);
//            }
//        }

//        private static void AddBlockBoundary(
//            this BlockCommentStripLoopCache cache,
//            MtBoundary blockBoundary)
//        {
//            blockBoundary.Index += cache.InputAdjustment + cache.OutputAdjustment;
//            cache.OutputBoundaries.Add(blockBoundary);
//        }

//        private static int AdjustIndexNumber(
//            this BlockCommentStripLoopCache cache,
//            int strippedIndex)
//        {
//            MtBoundary inputBoundary;
//            int adjustedIndex = strippedIndex;

//            for (int i = 0; i < cache.OutputBoundaries.Count; i++)
//            {
//                inputBoundary = cache.OutputBoundaries[i];

//                if (inputBoundary.Index > adjustedIndex)
//                {
//                    return adjustedIndex;
//                }

//                adjustedIndex += inputBoundary.Length;
//            }

//            return adjustedIndex;
//        }
//    }
//}
