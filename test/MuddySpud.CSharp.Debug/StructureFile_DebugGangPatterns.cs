using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class StructureFile_DebugGangUnitPatterns
    {
        private readonly BlockStatsCache? _blockStatsCache;
        private readonly BlockTextContainer _codeContainer;

        public StructureFile_DebugGangUnitPatterns()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string? dirPath = Path.GetDirectoryName(thisAssembly.Location);

            if (dirPath is null
                || String.IsNullOrWhiteSpace(dirPath))
            {
                throw new ArgumentNullException(nameof(dirPath));
            }

            string path = Path.Combine(dirPath, "Files\\CSharp11.cs");
            string code = File.ReadAllText(path);

            _codeContainer = new(
                code,
                "csharp",
                0,
                true);

            _blockStatsCache = _codeContainer.BlockStatsCache;
        }

        [Fact]
        public void Test_IndexOfSlowe()
        {
            StringBuilder testStringBuilderIndexOf = new(@"//namespace Groups
        //{
        //    public class Concrete
        //    {
        ");

            int result = testStringBuilderIndexOf.IndexOfSlow(
                "//",
                55
            );

            Assert.Equal("//", testStringBuilderIndexOf.ToString(result, 2));
        }

        [Theory]
        [InlineData(
            "GangGroup",
            "",
            "Branch",
            "if ()",
            39, 13,
            42, 49)]
        public void GroupMatches(
            string model,
            string blockType,
            string groupType,
            string name,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn)
        {
            Assert.NotNull(_blockStatsCache);

            bool found = false;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (BlockStats block in _blockStatsCache.BlockStats)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                if (block.BlockStartLocation.Line == blockStartLine
                    && block.BlockStartLocation.Column == blockStartColumn
                    && block.Settings.Model == model)
                {
                    Assert.Equal(groupType, block?.Type);
                    Assert.Equal(blockType, block?.Settings.BlockType);
                    Assert.Equal(name, block?.Name);

                    Assert.Equal(blockEndLine, block?.BlockEndLocation.Line);
                    Assert.Equal(blockEndColumn, block?.BlockEndLocation.Column);

                    found = true;
                }
            }

            Assert.True(found);
        }
    }
}

