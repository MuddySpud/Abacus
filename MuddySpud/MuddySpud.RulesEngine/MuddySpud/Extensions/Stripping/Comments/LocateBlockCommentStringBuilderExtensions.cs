//using Microsoft.ApplicationInspector.RulesEngine;
//using MuddySpud.RulesEngine.Commands;
//using System.Text;

//namespace MuddySpud.Metrics.Engine
//{
//    static class LocateBlockCommentStringBuilderExtensions
//    {
//        public static void StripBlockComments(
//            this StringBuilder content,
//            BlockCommentStripLoopCache cache,
//            int stopIndex)
//        {
//            int suffixLength = cache.SuffixComment.Length;
//            int prefixLength = cache.PrefixComment.Length;
//            int commentStartIndex = content.IndexOf(cache.PrefixComment, 0);
//            int commentEndIndex;
//            int commentLength;
//            int startSearchIndex = commentStartIndex + prefixLength;
//            MtBoundary blockBoundary;

//            while (commentStartIndex > -1)
//            {
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

//                blockBoundary = new MtBoundary(
//                    commentStartIndex,
//                    commentLength,
//                    "blockComment"
//                );

//                cache.OutputBoundaries.Add(blockBoundary);

//                // Set the next search to start where this comment started (it will be removed in next loop) 
//                startSearchIndex = commentEndIndex + 1;

//                // Look for the next block comment prefix
//                commentStartIndex = content.IndexOf(
//                    cache.PrefixComment,
//                    startSearchIndex);

//                if (commentEndIndex > stopIndex)
//                {
//                    return;
//                }

//                if (startSearchIndex < 0)
//                {
//                    break;
//                }
//            }
//        }
//    }
//}
