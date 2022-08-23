using MuddySpud.CSharp.Tests.Fixtures;
using MuddySpud.RulesEngine;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{

    public class StructureFile_SwitchExpressionPatterns : IClassFixture<SwitchFixture>
    {
        SwitchFixture _fixture;

        public StructureFile_SwitchExpressionPatterns(SwitchFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch expression",
            66, 36,
            73, 13)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            67, 14,
            73, 13)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "1",
            68, 21,
            68, 30)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "2",
            69, 21,
            69, 30)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "3",
            70, 21,
            70, 30)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "4",
            71, 21,
            71, 30)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "_",
            72, 21,
            73, 13)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch expression",
            75, 23,
            82, 13)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            76, 14,
            82, 13)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "Direction.Up",
            77, 32,
            77, 50)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "Direction.Right",
            78, 35,
            78, 52)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "Direction.Down",
            79, 34,
            79, 52)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "Direction.Left",
            80, 34,
            80, 51)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "_",
            81, 21,
            82, 13)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            85, 32,
            85, 43)]
        [InlineData(
            "Block",
            "Code",
            "method",
            "Transform",
            85, 47,
            91, 10)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch expression",
            85, 54,
            91, 9)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            86, 10,
            91, 9)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            87, 14,
            87, 26)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: 0, Y: 0 }",
            87, 30,
            87, 46)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            87, 41,
            87, 45)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            88, 14,
            88, 34)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: var x, Y: var y } when x < y",
            88, 49,
            88, 69)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            88, 60,
            88, 68)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            89, 14,
            89, 34)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: var x, Y: var y } when x > y",
            89, 49,
            89, 89)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            89, 72,
            89, 72)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            89, 80,
            89, 88)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            90, 14,
            90, 34)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: var x, Y: var y }",
            90, 38,
            91, 9)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            90, 49,
            90, 61)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            93, 51,
            93, 62)]
        [InlineData(
            "Block",
            "Code",
            "method",
            "Transform2",
            93, 66,
            99, 10)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch expression",
            93, 73,
            99, 9)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            94, 10,
            99, 9)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            95, 14,
            95, 26)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: 0, Y: 0 }",
            95, 30,
            95, 60)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            95, 59,
            95, 59)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            96, 14,
            96, 34)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: var x, Y: var y } when x < y",
            96, 49,
            96, 98)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            96, 78,
            96, 78)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            96, 81,
            96, 97)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            96, 83,
            96, 95)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            97, 14,
            97, 34)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: var x, Y: var y } when x > y",
            97, 49,
            97, 79)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            97, 78,
            97, 78)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            98, 14,
            98, 34)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: var x, Y: var y }",
            98, 38,
            99, 9)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            98, 67,
            98, 67)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            101, 51,
            101, 62)]
        [InlineData(
            "Block",
            "Code",
            "method",
            "Transform3",
            101, 66,
            124, 10)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch expression",
            101, 73,
            124, 9)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            102, 10,
            124, 9)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            103, 14,
            103, 26)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: 0, Y: 0 }",
            103, 30,
            103, 60)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            103, 59,
            103, 59)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            104, 14,
            104, 34)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: var x, Y: var y } when x < y",
            104, 49,
            109, 14)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch expression",
            104, 69,
            109, 13)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            105, 14,
            109, 13)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "> 5000",
            106, 26,
            106, 41)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "< 3000",
            107, 26,
            107, 41)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "_",
            108, 21,
            109, 13)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            110, 14,
            110, 34)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: var x, Y: var y } when x > y",
            110, 49,
            122, 14)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch expression",
            110, 59,
            122, 13)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            111, 14,
            122, 13)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "TaskStatus.EXECUTING",
            112, 40,
            112, 79)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "TaskStatus.WAITING",
            113, 38,
            120, 18)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "_",
            121, 21,
            122, 13)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            123, 14,
            123, 34)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "{ X: var x, Y: var y }",
            123, 38,
            124, 9)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            123, 67,
            123, 67)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            127, 50,
            127, 61)]
        [InlineData(
            "Block",
            "Code",
            "method",
            "GetParser",
            128, 15,
            137, 14)]
        [InlineData(
            "Group",
            "Code",
            "Branch",
            "switch expression",
            128, 21,
            137, 13)]
        [InlineData(
            "Block",
            "Code",
            "",
            "",
            129, 14,
            137, 13)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            130, 24,
            130, 31)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "nameof(Boolean)",
            130, 35,
            134, 48)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            134, 34,
            134, 47)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            134, 45,
            134, 46)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            135, 24,
            135, 29)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "nameof(Int32)",
            135, 33,
            135, 60)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            135, 47,
            135, 59)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            135, 57,
            135, 58)]
        [InlineData(
            "UnMask",
            "SwitchArm",
            "Branch",
            "_",
            136, 21,
            137, 13)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            136, 28,
            136, 29)]
        [InlineData(
            "Block",
            "Parameter",
            "",
            "",
            136, 31,
            136, 37)]
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

