using System.Text.RegularExpressions;

namespace MuddySpud.Metrics.Engine
{
    public static class StringExtensions
    {
        public static string ReplaceWhiteSpaceWithSingleSpace(this string input)
        {
            string cleaned = Regex.Replace(input, @"\s+", " ");

            return cleaned;
        }

        public static string CleanSpaces(this string input)
        {
            string cleaned = input
                .Trim()
                .ReplaceWhiteSpaceWithSingleSpace()
                .Replace(" <", "<")
                .Replace("< ", "<")
                .Replace(" >", ">")
                //.Replace("> ", ">")
                .Replace(" (", "(")
                .Replace("( ", "(")
                .Replace(" )", ")")
                //.Replace(") ", ")")
                .Replace(" [", "[")
                .Replace("[ ", "[")
                .Replace(" ]", "]")
                //.Replace("] ", "]")
                .Replace(" ,", ",")
                .Replace(",(", ", (")
                ;

            return cleaned;
        }

        public static string NormaliseLineEndings(this string input)
        {
            string cleaned = input
                .Replace("\r\n", "\r")
                 .Replace("\n\r", "\r")
                 .Replace("\n", "\r")
                 .Replace("\r", "\r\n")
            ;

            return cleaned;
        }
    }
}
