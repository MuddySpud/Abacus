using MuddySpud.Metrics.Engine;
using System.Text;
using System.Text.RegularExpressions;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpSignatureInterpreter
    {
        internal static string CleanSignature(
            this Match match,
            StringBuilder cleanedBuilder,
            string componentName,
            string signature)
        {
            return componentName switch
            {
                "method" => match.CleanMethodSignature(cleanedBuilder),
                "property" => match.CleanPropertySignature(cleanedBuilder),
                "indexer" => match.CleanIndexerSignature(cleanedBuilder),
                _ => signature.CleanSpaces()
            };
        }

        private static string CleanMethodSignature(
            this Match match,
            StringBuilder cleanedBuilder)
        {
            cleanedBuilder.Clear();
            cleanedBuilder.Append(match.Groups["keywords"].Value.CleanSpaces().Trim());
            cleanedBuilder.Append(" ");
            cleanedBuilder.Append(match.Groups["partial"].Value.CleanSpaces().Trim());
            cleanedBuilder.Append(" ");
            cleanedBuilder.Append(match.Groups["returnValue"].Value.CleanSpaces().Trim());
            cleanedBuilder.Append(" ");
            cleanedBuilder.Append(match.Groups["method"].Value.CleanSpaces().Trim());
            cleanedBuilder.Append(match.Groups["params"].Value.CleanSpaces().Trim());

            return cleanedBuilder
                .ToString()
                .ReplaceWhiteSpaceWithSingleSpace()
                .Trim();
        }

        private static string CleanPropertySignature(
            this Match match,
            StringBuilder cleanedBuilder)
        {
            cleanedBuilder.Clear();
            cleanedBuilder.Append(match.Groups["keywords"].Value.CleanSpaces().Trim());
            cleanedBuilder.Append(" ");
            cleanedBuilder.Append(match.Groups["returnValue"].Value.CleanSpaces().Trim());
            cleanedBuilder.Append(" ");
            cleanedBuilder.Append(match.Groups["property"].Value.CleanSpaces().Trim());

            return cleanedBuilder
                .ToString()
                .ReplaceWhiteSpaceWithSingleSpace()
                .Trim();
        }

        private static string CleanIndexerSignature(
            this Match match,
            StringBuilder cleanedBuilder)
        {
            cleanedBuilder.Clear();
            cleanedBuilder.Append(match.Groups["keywords"].Value.CleanSpaces().Trim());
            cleanedBuilder.Append(" ");
            cleanedBuilder.Append(match.Groups["returnValue"].Value.CleanSpaces().Trim());
            cleanedBuilder.Append(" ");
            cleanedBuilder.Append("this");
            cleanedBuilder.Append(match.Groups["params"].Value.CleanSpaces().Trim());

            return cleanedBuilder
                .ToString()
                .ReplaceWhiteSpaceWithSingleSpace()
                .Trim();
        }
    }
}
