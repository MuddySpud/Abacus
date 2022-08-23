using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpFacetInterpreter
    {
        internal static void FindFacet(
            this List<BlockStats> blockStats,
            FacetSettings facetSettings,
            StringBuilder content,
            string text)
        {
            int matchEnd;
            Regex regex;
            MatchCollection matches;
            Regex rejectMatchRegex;

            foreach (PatternSettings patternSettings in facetSettings.Patterns)
            {
                regex = new Regex(patternSettings.RegexPattern);
                matches = regex.Matches(text);

                foreach (Match match in matches)
                {
                    if (patternSettings.RejectMatchRegexPattern is { })
                    {
                        rejectMatchRegex = new Regex(patternSettings.RejectMatchRegexPattern);

                        if (rejectMatchRegex.IsMatch(match.Value))
                        {
                            continue;
                        }
                    }

                    matchEnd = match.Index + match.Value.Length;

                    foreach (BlockStats stats in blockStats)
                    {
                        if (stats.Settings.BlockType != facetSettings.BlockType
                            || stats.Settings.Model != "Block"
                            || !String.IsNullOrWhiteSpace(stats.Name))
                        {
                            // block already named so can't do it again
                            continue;
                        }

                        if (stats.OpenIndex == matchEnd)
                        {
                            for (int i = matchEnd; i < stats.CloseIndex; i++)
                            {
                                if (!Char.IsWhiteSpace(content[i]))
                                {
                                    content[i] = 'x';
                                }
                            }

                            break;
                        }
                        else if (stats.OpenIndex > matchEnd)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
