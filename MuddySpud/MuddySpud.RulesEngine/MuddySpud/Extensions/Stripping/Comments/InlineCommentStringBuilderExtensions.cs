//using Microsoft.ApplicationInspector.RulesEngine;
//using MuddySpud.RulesEngine.Commands;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace MuddySpud.Metrics.Engine
//{
//    static class InlineCommentStringBuilderExtensions
//    {
//        public static void StripInlineComments(
//            this StringBuilder content,
//            InlineCommentStripLoopCache cache)
//        {
//            int commentStartIndex = content.IndexOf(
//                cache.InlineComment,
//                0
//            );

//            int commentEndIndex;
//            int commentLength;
//            string comment;
//            int adjustedCommentStartIndex;
//            int startSearchIndex = commentStartIndex;
//            MtBoundary inlineBoundary;
//            bool isShieldedByQuote;

//            while (commentStartIndex > -1)
//            {
//                isShieldedByQuote = content.CheckQuotesAndBlockComments(
//                    cache,
//                    commentStartIndex
//                );

//                if (isShieldedByQuote)
//                {
//                    // Look for the next inline comment
//                    startSearchIndex += cache.InlineComment.Length;

//                    commentStartIndex = content.IndexOf(
//                        cache.InlineComment,
//                        startSearchIndex);

//                    continue;
//                }

//                commentEndIndex = content.IndexOf(
//                    '\n',
//                    commentStartIndex);

//                if (commentEndIndex < 0)
//                {
//                    break;
//                }

//                commentLength = commentEndIndex - commentStartIndex;
//                adjustedCommentStartIndex = commentStartIndex + cache.Adjustment;

//                comment = content.ToString(
//                    commentStartIndex,
//                    commentLength
//                );

//                // Remove comment from content
//                content.Remove(
//                    commentStartIndex,
//                    commentLength
//                );

//                inlineBoundary = new MtBoundary(
//                    adjustedCommentStartIndex,
//                    commentLength,
//                    "inlineComment"
//                );

//                cache.InlineBoundaries.Add(inlineBoundary);

//                cache.CommentContent.Add(
//                    (adjustedCommentStartIndex * 10),
//                    comment
//                );

//                // Include the length of the removed string to the adjuster
//                cache.Adjustment += commentLength;

//                startSearchIndex = startSearchIndex < 0
//                    ? 0
//                    : startSearchIndex;

//                // Look for the next inline comment
//                commentStartIndex = content.IndexOf(
//                    cache.InlineComment,
//                    startSearchIndex);

//                // Set the next search to start where this comment started (it will be removed in next loop) 
//                startSearchIndex = commentStartIndex;

//                if (startSearchIndex < 0)
//                {
//                    break;
//                }
//            }
//        }

//        private static bool CheckQuotesAndBlockComments(
//            this StringBuilder content,
//            InlineCommentStripLoopCache cache,
//            int commentStartIndex)
//        {
//            bool skip = CheckQuotes(
//                content,
//                cache,
//                commentStartIndex
//            );

//            if (skip)
//            {
//                return true;
//            }

//            return CheckBlockComments(
//                content,
//                cache,
//                commentStartIndex
//            );
//        }

//        private static bool CheckQuotesAndBlockComments2(
//            this StringBuilder content,
//            InlineCommentStripLoopCache cache,
//            int commentStartIndex)
//        {
//            // Look for a quote just before this inline comment on the same line
//            // If it is preceded by and escape char ignore and keep searching
//            //      A unescaped quote on same line it is valid but is it the beginning or the end of a string
//            //          Search to the beginning of the string for another unescaped quote
//            //              If there are odd unescaped quotes on the same line then check after the comment on the same line is there an odd number of unescaped quotes?
//            //                  If Yes comment is in a string and ignore
//            //                  If No there aren't odd number of strings then trhow an error for now and add it to the file errors
//            // If there were no unescaped quotes on this line the look for quotes in previous lines ignoring phrase escaped quotes if the first is one is phrase escaped the comment is in a string and ignore
//            // If not the comment is valid

//            // Need to bear in mind how strings in formats mess this up...

//            if (!cache.StringSettings.PhraseCharacterEscape.Escape.HasValue)
//            {
//                throw new NotImplementedException("Have not coded for either an empty phrase escape or post");
//            }

//            if (!cache.StringSettings.CharacterEscape.Escape.HasValue
//                || cache.StringSettings.CharacterEscape.Location == "pre")
//            {
//                throw new NotImplementedException("Have not coded for either an empty quote escape or post");
//            }

//            char quote = cache.StringSettings.Quote;
//            char quoteEscape = cache.StringSettings.CharacterEscape.Escape.Value;

//            char phrasePrefeix = cache.StringSettings.PhrasePrefix;
//            char phraseQuoteEscape = cache.StringSettings.PhraseCharacterEscape.Escape.Value;

//            if (phraseQuoteEscape != '"')
//            {
//                throw new NotImplementedException("Have not coded for either an empty quote escape or post");
//            }

//            int index = commentStartIndex - 1;
//            char c = content[index];
//            int priorIndex;
//            char priorC;
//            int singleQuoteCount = 0;
//            int doubleQuoteCount = 0;
//            int phraseQuoteEscapeCount = 0;

//            while (c != '\n')
//            {
//                if (c == quote)
//                {
//                    // Check if escaped
//                    priorIndex = index - 1;
//                    priorC = content[priorIndex];

//                    if (priorC == quoteEscape)
//                    {
//                        // The could be in a string
//                        // Now need to check to see if it is the start or end on
//                        continue;
//                    }
//                    else if (priorC == quote)
//                    {
//                        // Then this is possibly an empty string
//                        // It could also be a phrase quote escape]
//                        // Keep looking
//                        ++doubleQuoteCount;
//                    }
//                    else if (priorC == phraseQuoteEscape)
//                    {
//                        // Then the comment is in a string
//                        ++phraseQuoteEscapeCount;
//                    }
//                    else
//                    {
//                        // Any other character
//                        ++singleQuoteCount;
//                    }
//                }
//            }

//            if (singleQuoteCount % 2 == 1)
//            {
//                // Then an odd number of line quotes before comment mean comment is in a string
//                return true;
//            }

//            return false;
//        }

//        private static bool CheckQuotes(
//            this StringBuilder content,
//            InlineCommentStripLoopCache cache,
//            int commentStartIndex)
//        {
//            int inlineCommentEndIndex = commentStartIndex + cache.InlineComment.Length;
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
//                if (stringBoundary.Index <= commentStartIndex
//                    && (stringBoundary.Index + stringBoundary.Length) >= inlineCommentEndIndex)
//                {
//                    return true;
//                }
//            }

//            return false;
//        }

//        private static bool CheckBlockComments(
//            this StringBuilder content,
//            InlineCommentStripLoopCache cache,
//            int commentStartIndex)
//        {
//            int prefixEndIndex = commentStartIndex + cache.CommentSettings.Prefix.Length;
//            int stopIndex = prefixEndIndex > content.Length ? content.Length : prefixEndIndex;

//            List<MtBoundary> emptyBoundaries = new();
//            List<MtBoundary> stringBoundaries = new();
//            StringBuilder copiedContent = new StringBuilder(content.ToString());

//            BlockCommentStripLoopCache blockCache = new(
//                emptyBoundaries,
//                stringBoundaries,
//                cache.CommentSettings.Prefix,
//                cache.CommentSettings.Suffix,
//                cache.StringSettings,
//                new()
//            );

//            copiedContent.StripBlockComments(
//                blockCache,
//                stopIndex
//            );

//            foreach (MtBoundary stringBoundary in blockCache.OutputBoundaries)
//            {
//                if (stringBoundary.Index <= commentStartIndex
//                    && (stringBoundary.Index + stringBoundary.Length) >= prefixEndIndex)
//                {
//                    return true;
//                }
//            }

//            return false;
//        }
//    }
//}
