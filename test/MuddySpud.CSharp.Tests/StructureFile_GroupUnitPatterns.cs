using MuddySpud.CSharp.Tests.Fixtures;
using MuddySpud.RulesEngine;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class StructureFile_GroupUnitPatterns : IClassFixture<SwitchFixture>
    {
        SwitchFixture _fixture;

        public StructureFile_GroupUnitPatterns(SwitchFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch ()",
            9, 13,
            33, 13)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            9, 21,
            9, 32)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            10, 14,
            33, 13)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            11, 28,
            15, 16)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            12, 39,
            12, 83)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            12, 60,
            12, 71)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            12, 84,
            12, 84)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            12, 85,
            13, 26)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            15, 29,
            21, 16)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            16, 22,
            19, 21)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            17, 43,
            17, 88)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            17, 64,
            17, 75)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            17, 89,
            17, 89)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            17, 90,
            18, 30)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            21, 33,
            25, 16)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            22, 39,
            22, 60)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            22, 61,
            22, 61)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            22, 62,
            23, 26)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            25, 33,
            26, 16)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            26, 30,
            30, 16)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            27, 39,
            27, 60)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            27, 61,
            27, 61)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            27, 62,
            28, 26)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "default",
            30, 25,
            33, 13)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            31, 39,
            31, 74)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            31, 75,
            31, 75)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            31, 76,
            32, 26)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch ()",
            35, 13,
            49, 13)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            35, 21,
            35, 32)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            36, 14,
            49, 13)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            37, 26,
            38, 16)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            38, 28,
            44, 16)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            39, 22,
            42, 21)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            40, 43,
            40, 106)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            40, 64,
            40, 75)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            40, 107,
            40, 107)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            40, 108,
            41, 30)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "default",
            44, 25,
            49, 13)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            45, 22,
            48, 21)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            46, 43,
            46, 78)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            46, 64,
            46, 75)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            46, 79,
            46, 79)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            46, 80,
            47, 30)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch ()",
            51, 13,
            64, 13)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            51, 21,
            51, 27)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            51, 22,
            51, 26)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            52, 14,
            64, 13)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            53, 46,
            57, 16)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            53, 23,
            53, 32)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            54, 39,
            54, 87)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            54, 83,
            54, 84)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            54, 88,
            54, 88)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            54, 89,
            55, 26)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            57, 34,
            61, 16)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            57, 23,
            57, 32)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            58, 39,
            58, 94)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            58, 63,
            58, 64)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            58, 90,
            58, 91)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            58, 95,
            58, 95)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            58, 96,
            59, 26)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "default",
            61, 25,
            64, 13)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            62, 39,
            62, 80)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            62, 81,
            62, 81)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            62, 82,
            63, 26)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch ()",
            141, 13,
            164, 13)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            141, 21,
            141, 25)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            142, 14,
            164, 13)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            143, 77,
            144, 16)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            143, 86,
            143, 121)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            143, 122,
            143, 122)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            144, 46,
            145, 16)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            144, 55,
            144, 90)]
        [InlineData(
            "Statement",
            "Code",
            "",
            "",
            144, 91,
            144, 91)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            145, 47,
            163, 16)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch ()",
            147, 21,
            161, 21)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            147, 29,
            147, 39)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            148, 22,
            161, 21)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "case",
            149, 34,
            150, 24)]
        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "default",
            156, 33,
            161, 21)]

        [InlineData(
            "GroupUnit",
            "Code",
            "Branch",
            "default",
            163, 25,
            164, 13)]
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

