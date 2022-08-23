using MuddySpud.CSharp.Tests.Fixtures;
using MuddySpud.RulesEngine;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class StructureFile_IncompleteBlocks : IClassFixture<IncompleteBlockFixture>
    {
        IncompleteBlockFixture _fixture;

        public StructureFile_IncompleteBlocks(IncompleteBlockFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ErrorCount()
        {
            Assert.NotNull(_fixture.BlockStatsCache);

            int count = 0;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (BlockStats block in _fixture.BlockStatsCache.BlockStats)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                if (block.Errors.Count > 0)
                {
                    count++;
                }
            }

            Assert.Equal(10, count);
        }
    }
}

