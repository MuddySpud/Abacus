using System;
using System.Text.RegularExpressions;

namespace MuddySpud.Metrics.Engine
{
    public static class RegexMatchExtensions
    {
        public static string GetGroupValue(
            this Match match,
            string groupName)
        {
            if (match is null)
            {
                return String.Empty;
            }

            Group group = match.Groups[groupName];

            if (group is null)
            {
                return String.Empty;
            }

            string value = group.Value;

            return value.Trim();
        }
    }
}
