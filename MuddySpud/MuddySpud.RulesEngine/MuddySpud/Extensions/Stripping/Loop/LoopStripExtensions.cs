using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public static class LoopStripExtensions
    {
        public static void Strip(
            this StringBuilder content,
            StripLoopCache cache)
        {
            // Start at zero then look for " or an inlineComment prefix or a blockComment prefix

            // The character after " is the start of the string, it must end with a single " if the line ends first it is an error
            //      if it is preceded by a @ it is multiline
            //      if it is preceded by a $ or a $@ it is a format string so format rules apply
            // The character after // is the start of the inline comment till the end of the line, all characters in here should be ignored
            // The character after /* is the start of the block comment till the block comment end, all characters in here should be ignored
            // then check to see if it is preceeded by @ 
            // if so use phrasecharacterescape
            // if not use the characterescape
            // then check for the next "
            // check caracter before to if it is escaped
            // if it is ignore and search for next " or {
            // if it isn't escaped then the preceeding character is the end of the string
            // remove the string leaving the first " and the last "
            // then look for next "

            char c;

            for (int index = 0; index < content.Length; index++)
            {
                c = content[index];

                if (c == cache.StringCache.StringSettings.Quote)
                {
                    index = content.ProcessString(
                       cache.StringCache,
                       index
                    );
                }
                else if (content.IsMatchAt(
                            cache.InlineCommentCache.InlineComment,
                            index))
                {
                    index = content.ProcessInlineComment(
                       cache.InlineCommentCache,
                       index
                    );
                }
                else if (content.IsMatchAt(
                            cache.BlockCommentCache.PrefixComment,
                            index))
                {
                    index = content.ProcessBlockComment(
                       cache.BlockCommentCache,
                       index
                    );
                }
                else if (content.IsMatchAnyAt(
                            cache.PreProcessorCache.PreProcessors,
                            index))
                {
                    index = content.ProcessPreProcessors(
                       cache.PreProcessorCache,
                       index
                    );
                }
            }
        }
    }
}
