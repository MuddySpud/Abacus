using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class StructureFile_DebugStringPatterns
    {
        private readonly string _code = String.Empty;
        private readonly string _result = String.Empty;
        private readonly BlockStatsCache _blockStatsCache;
        private readonly BlockTextContainer _codeContainer;

        public StructureFile_DebugStringPatterns()
        {
            //Assembly thisAssembly = Assembly.GetExecutingAssembly();
            //string? dirPath = Path.GetDirectoryName(thisAssembly.Location);

            //if (dirPath is null
            //    || String.IsNullOrWhiteSpace(dirPath))
            //{
            //    throw new ArgumentNullException(nameof(dirPath));
            //}

            //string path = Path.Combine(dirPath, "Files\\CSharp9.cs");
            //string code = File.ReadAllText(path);

            //_codeContainer = new(
            //    code,
            //    "csharp",
            //    0,
            //    true);

            //_blockStatsCache = _codeContainer.BlockStatsCache;
            //_code = _codeContainer.FullContent;
            //_result = _codeContainer.StrippedContent;
        }

        [Theory]
        [InlineData(
            "            string test1 = \"Fred was here\";",
            "            string test1 = \"\";")]
        [InlineData(
            "            string test1 = \"Fred was \\\"NOT\\\" here\";",
            "            string test1 = \"\";")]
        [InlineData(
            "            string test1 = @\"Fred was \"\"NOT\\ here\";",
            "            string test1 = @\"\";")]
        [InlineData(
            "            string test8 = $\"Fred was here: {DateTime.Now.ToString()} 2 hours late\";",
            "            string test8 = $\"{DateTime.Now.ToString()}\";")]
        [InlineData(
            "            string test11 = $\"Fred was here: {DateTime.Now.ToString()} 2 \\\"hours\\\" late\";",
            "            string test11 = $\"{DateTime.Now.ToString()}\";")]
        [InlineData(
            "            string test15 = $\"Fred was here: {DateTime.Now.ToString(\"yyyy/MM/dd_HH:mm:ss::fff\")} 2 hours late\";",
            "            string test15 = $\"{DateTime.Now.ToString(\"\")}\";")]
        public void OpenAndCloseBlockMatches(
            string test,
            string expected)
        {
            BlockTextContainer codeContainer = new(
                test,
                "csharp",
                0,
                true
            );

            Assert.Equal(expected, codeContainer.CodeContent);
        }

        [Fact]
        public void SsringsMatches()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string? dirPath = Path.GetDirectoryName(thisAssembly.Location);

            if (dirPath is null
                || String.IsNullOrWhiteSpace(dirPath))
            {
                throw new ArgumentNullException(nameof(dirPath));
            }

            string path = Path.Combine(dirPath, "Files\\CSharp9StringStripped.cs");
            string expected = File.ReadAllText(path);

            Assert.Equal(expected, _result);
        }

        //    private BlockStats? GetBlockBySignature(
        //            string signature,
        //            string fullName,
        //            string type)
        //    {
        //        foreach (BlockStats block in _blockStatsCache.Stats)
        //        {
        //            if (String.Equals(block?.Signature, signature, StringComparison.Ordinal)
        //                && String.Equals(block?.FullName, fullName, StringComparison.Ordinal)
        //                && String.Equals(block?.Type, type, StringComparison.Ordinal))
        //            {
        //                return block;
        //            }
        //        }

        //        return null;
        //    }

        //    private BlockStats? GetBlockByName(
        //        string name,
        //        string type)
        //    {
        //        foreach (BlockStats block in _blockStatsCache.Stats)
        //        {
        //            if (String.Equals(block?.Name, name, StringComparison.Ordinal))
        //            {
        //                if (String.Equals(block?.Type, type, StringComparison.Ordinal))
        //                {
        //                    return block;
        //                }
        //            }
        //        }

        //        return null;
        //    }
    }
}
