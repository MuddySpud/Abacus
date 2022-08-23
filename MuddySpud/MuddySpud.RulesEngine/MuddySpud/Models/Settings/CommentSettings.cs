// Copyright(C) Microsoft.All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace MuddySpud.RulesEngine
{
    /// <summary>
    /// Comment class to hold information about comment for each language
    /// </summary>
    public class CommentSettings
    {
        public string Inline { get; } = string.Empty;
        public string Prefix { get; } = string.Empty;
        public string Suffix { get; } = string.Empty;

        public CommentSettings(
            string inline,
            string prefix,
            string suffix)
        {
            Inline = inline;
            Prefix = prefix;
            Suffix = suffix;
        }
    }
}