// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace MuddySpud.RulesEngine
{
    /// <summary>
    /// Comment class to hold information about comment for each language
    /// </summary>
    public class PatternSettings
    {
        public string RegexPattern { get; }
        public string? RejectMatchRegexPattern { get; set; }

        public PatternSettings(string regexPattern)
        {
            RegexPattern = regexPattern;
        }
    }
}