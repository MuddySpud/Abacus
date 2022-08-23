using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.Metrics.Engine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpUnitsOfWorkInterpreter
    {
        internal static void FindUnitsOfWork(
            this BlockStatsCache blockStatsCache,
            string text,
            IBoundaryCounter boundaryCounter,
            IBoundarySettingsBuilder boundarySettingsBuilder)
        {
            UnitOfWorkSettings unitOfWorkSettings = blockStatsCache.FileStructureSettings.UnitsOfWork;

            Regex regex;
            Regex rejectMatchRegex;
            MatchCollection matches;
            int adjustedMatchStart;
            Location matchStartLocation;

            foreach (PatternSettings patternSettings in unitOfWorkSettings.NonBlockPatterns)
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

                    adjustedMatchStart = boundaryCounter.GetFullIndexFromCodeIndex(match.Index);
                    matchStartLocation = boundaryCounter.GetLocation(adjustedMatchStart);

                    blockStatsCache.UnitsOfWorkStart.Add(
                        new(
                            adjustedMatchStart,
                            matchStartLocation
                        )
                    );
                }
            }
        }
    }
}
