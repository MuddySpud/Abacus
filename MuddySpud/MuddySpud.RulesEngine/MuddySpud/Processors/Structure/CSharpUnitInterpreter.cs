using MuddySpud.Metrics.Engine;
using System;
using System.Text.RegularExpressions;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpUnitInterpreter
    {
        internal static void FindUnits(
            this BlockStats parent,
            ComponentSettings componentSettings,
            string text,
            IBoundaryCounter boundaryCounter)
        {
            foreach (UnitSettings unitSetting in componentSettings.Units)
            {
                FindUnit(
                    componentSettings,
                    unitSetting,
                    parent,
                    text,
                    boundaryCounter);
            }
        }

        private static void FindUnit(
            ComponentSettings componentSettings,
            UnitSettings unitSetting,
            BlockStats parent,
            string text,
            IBoundaryCounter boundaryCounter)
        {
            int matchStart;
            int matchEnd;
            Regex regex;
            Regex rejectMatchRegex;
            string regexPattern;
            MatchCollection matches;
            string parentText = String.Empty;

            if (parent.CloseIndex > 0)
            {
                parentText = text.Substring(
                    parent.OpenIndex,
                    parent.CloseIndex - parent.OpenIndex
                );
            }

            // Parents should be run first then children
            // More specific searches should be run before less
            // ie Constructors before methods etc
            // stats named will be skipped on following matches

            foreach (PatternSettings patternSettings in unitSetting.Patterns)
            {
                regexPattern = patternSettings.RegexPattern.Replace($"|*|{componentSettings.Name.ToUpper()}|*|", parent.Name);
                regex = new Regex(regexPattern);
                matches = regex.Matches(parentText);

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

                    matchStart = match.Index + parent.OpenIndex;
                    matchEnd = matchStart + match.Value.Length;

                    foreach (BlockStats stats in parent.ChildBlocks)
                    {
                        if (stats.Settings.BlockType != unitSetting.BlockType
                            || !String.IsNullOrWhiteSpace(stats.Name))
                        {
                            // block already named so can't do it again
                            continue;
                        }

                        if (stats.OpenIndex == matchEnd)
                        {
                            stats.PrintMetrics = unitSetting.PrintMetrics;
                            stats.MatchStart = matchStart;
                            stats.MatchEnd = matchEnd;
                            stats.AdjustedMatchStart = boundaryCounter.GetFullIndexFromCodeIndex(stats.MatchStart);
                            stats.AdjustedMatchEnd = boundaryCounter.GetFullIndexFromCodeIndex(stats.MatchEnd);
                            stats.MatchStartLocation = boundaryCounter.GetLocation(stats.AdjustedMatchStart);
                            stats.MatchEndLocation = boundaryCounter.GetLocation(stats.AdjustedMatchEnd);

                            stats.Name = match.Groups[unitSetting.Name].Value;
                            stats.Name = stats.Name.CleanSpaces();
                            stats.Type = unitSetting.Name;
                            stats.Signature = match.GetGroupValue("signature");

                            stats.CheckAndAddFlag(
                                match,
                                "partial");

                            stats.CleanedSignature = stats.Signature.CleanSpaces();
                            stats.Value = match.Value;

                            break;
                        }
                        else if (stats.OpenIndex > matchEnd)
                        {
                            break;
                        }

                        switch (stats.OpenIndex)
                        {
                            case 1:
                                {
                                    Console.WriteLine("Case 1");
                                    break;
                                }
                            case 2:
                                Console.WriteLine("Case 2");
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }
                    }
                }
            }
        }
    }
}
