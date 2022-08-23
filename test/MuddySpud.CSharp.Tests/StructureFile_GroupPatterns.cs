using MuddySpud.CSharp.Tests.Fixtures;
using MuddySpud.RulesEngine;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class StructureFile_GroupPatterns : IClassFixture<GroupFixture>
    {
        GroupFixture _fixture;

        public StructureFile_GroupPatterns(GroupFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(
            "Block",
            "Code",
            "namespace",
            "Test.Groups",
            2, 2,
            92, 1)]
        [InlineData(
            "Block",
            "Code",
            "class",
            "Concrete",
            4, 6,
            91, 5)]
        [InlineData(
            "Block",
            "Code",
            "property",
            "Property3",
            6, 10,
            8, 9)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            7, 19,
            7, 24)]
        [InlineData(
            "Block",
            "Code",
            "method",
            "Quick",
            11, 10,
            90, 9)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            10, 35,
            10, 50)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            12, 38,
            14, 47)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            16, 13,
            19, 13)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            21, 13,
            25, 13)]
        [InlineData(
            "Group",
            "Code",
            "Loop",
            "for ()",
            27, 13,
            30, 13)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            32, 13,
            33, 62)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            35, 13,
            37, 23)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            39, 13,
            42, 49)]
        [InlineData(
            "Group",
            "Code",
            "Loop",
            "for ()",
            41, 17,
            42, 49)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            42, 21,
            42, 49)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            44, 13,
            47, 53)]
        [InlineData(
            "Group",
            "Code",
            "Loop",
            "for ()",
            46, 17,
            47, 53)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            47, 21,
            47, 53)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            49, 13,
            53, 31)]
        [InlineData(
            "Group",
            "Code",
            "Loop",
            "for ()",
            51, 17,
            53, 31)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            52, 21,
            53, 31)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            55, 13,
            59, 31)]
        [InlineData(
            "Group",
            "Code",
            "Loop",
            "for ()",
            57, 17,
            59, 31)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            58, 21,
            59, 31)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            61, 13,
            63, 23)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            65, 13,
            66, 66)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            68, 13,
            68, 82)]
        [InlineData(
            "Gang",
            "Code",
            "Branch",
            "ifElse",
            70, 13,
            75, 23)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            70, 13,
            71, 23)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            70, 17,
            70, 26)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            71, 14,
            71, 23)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            71, 14,
            71, 21)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "else if ()",
            72, 13,
            73, 23)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            72, 22,
            72, 31)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            73, 14,
            73, 23)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            73, 14,
            73, 21)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "else",
            74, 13,
            75, 23)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            75, 14,
            75, 23)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            75, 14,
            75, 21)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            77, 13,
            89, 21)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            77, 17,
            78, 54)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            78, 46,
            78, 53)]
        [InlineData(
            "Group",
            "Code",
            "Loop",
            "for ()",
            79, 17,
            89, 21)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            79, 22,
            79, 43)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            79, 22,
            79, 31)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            79, 32,
            79, 38)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            79, 39,
            79, 43)]
        [InlineData(
            "Gang",
            "Code",
            "Branch",
            "ifElse",
            80, 21,
            89, 21)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            80, 21,
            80, 49)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            80, 25,
            80, 41)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            80, 42,
            80, 49)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "else if ()",
            81, 21,
            84, 21)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            81, 30,
            81, 46)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            82, 22,
            84, 21)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            82, 22,
            83, 33)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "else",
            85, 21,
            89, 21)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            86, 22,
            89, 21)]
        [InlineData(
            "Group",
            "Code",
            "Loop",
            "for ()",
            87, 25,
            88, 49)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            87, 30,
            87, 51)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            87, 30,
            87, 39)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            87, 40,
            87, 46)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            87, 47,
            87, 51)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "if ()",
            88, 29,
            88, 49)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            88, 33,
            88, 39)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            88, 40,
            88, 49)]
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
            Assert.NotNull(_fixture.BlockStatsCache);

            bool found = false;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (BlockStats block in _fixture.BlockStatsCache.BlockStats)
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

