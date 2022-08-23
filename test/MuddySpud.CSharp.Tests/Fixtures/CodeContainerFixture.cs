using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine;
using System;
using System.IO;
using System.Reflection;

namespace MuddySpud.CSharp.Tests.Fixtures
{
    public class CodeContainerFixture
    {
        public BlockStatsCache? BlockStatsCache { get; init; }
        public BlockTextContainer CodeContainer { get; init; }
        public string? DirectoryPath { get; init; }
        public string Code { get; init; }

        public CodeContainerFixture(string testFileLocation)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            DirectoryPath = Path.GetDirectoryName(thisAssembly.Location);

            if (DirectoryPath is null
                || String.IsNullOrWhiteSpace(DirectoryPath))
            {
                throw new ArgumentNullException(nameof(DirectoryPath));
            }

            string path = Path.Combine(DirectoryPath, testFileLocation);
            Code = File.ReadAllText(path);

            CodeContainer = new(
                Code,
                "csharp",
                0,
                true);

            BlockStatsCache = CodeContainer.BlockStatsCache;

        }
    }
}
