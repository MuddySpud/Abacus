using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class StructureFile_DebugRegexPatterns
    {
        private readonly string _raw = String.Empty;
        private readonly string _code = String.Empty;
        private readonly BlockStatsCache _blockStatsCache;
        private readonly BlockTextContainer _codeContainer;

        public StructureFile_DebugRegexPatterns()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string? dirPath = Path.GetDirectoryName(thisAssembly.Location);

            if (dirPath is null
                || String.IsNullOrWhiteSpace(dirPath))
            {
                throw new ArgumentNullException(nameof(dirPath));
            }

            string path = Path.Combine(dirPath, "Files\\CSharp3.cs");
            string code = File.ReadAllText(path);

            _codeContainer = new(
                code,
                "csharp");

            _blockStatsCache = _codeContainer.BlockStatsCache;
            _raw = _codeContainer.RawContent;
            _code = _codeContainer.CodeContent;
        }

        #region method

        [Theory]
        [InlineData(
            "method",
            "internal abstract static Task<   string   > MoveBranch16   <    T,    U   >     (     \r\n            string      \r\n            fred     \r\n                  )",
            "internal abstract static Task<string> MoveBranch16<T, U>(string fred)",
            "MoveBranch16<T, U>",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.MoveBranch16<T, U>",
            22, 10,
            55, 9,
            12, 9,
            22, 10)]
        public void MethodMatches(
            string componentType,
            string signature,
            string cleanedSignature,
            string componentName,
            string fullName,
            int blockStartLine, //
            int blockStartColumn,
            int blockEndLine,   //
            int blockEndColumn,
            int matchStartLine, //
            int matchStartColumn,
            int matchEndLine,   //
            int matchEndColumn)
        {
            BlockStats? block = GetBlockBySignature(
                signature,
                "method"
            );

            //BlockStats block = GetBlockByName(
            //    componentName,
            //    "method"
            //);

            _codeContainer.GetFullIndexFromCodeIndex(218);

            Assert.Equal(blockStartLine, block?.BlockStartLocation.Line);
            Assert.Equal(blockStartColumn, block?.BlockStartLocation.Column);

            Assert.Equal(blockEndLine, block?.BlockEndLocation.Line);
            Assert.Equal(blockEndColumn, block?.BlockEndLocation.Column);

            Assert.Equal(matchStartLine, block?.MatchStartLocation.Line);
            Assert.Equal(matchStartColumn, block?.MatchStartLocation.Column);

            Assert.Equal(matchEndLine, block?.MatchEndLocation.Line);
            Assert.Equal(matchEndColumn, block?.MatchEndLocation.Column);

            Assert.Equal(componentType, block?.Type);
            Assert.Equal(componentName, block?.Name);
            Assert.Equal(cleanedSignature, block?.CleanedSignature);
            Assert.Equal(fullName, block?.FullName);
        }

        #endregion

        private BlockStats? GetBlockBySignature(
            string signature,
            string type)
        {
            foreach (BlockStats block in _blockStatsCache.BlockStats)
            {
                if (String.Equals(block?.Signature, signature, StringComparison.Ordinal))
                {
                    if (String.Equals(block?.Type, type, StringComparison.Ordinal))
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        private BlockStats? GetBlockByName(
            string name,
            string type)
        {
            foreach (BlockStats block in _blockStatsCache.BlockStats)
            {
                if (String.Equals(block?.Name, name, StringComparison.Ordinal))
                {
                    if (String.Equals(block?.Type, type, StringComparison.Ordinal))
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        //private int AdjustLineNumber(int strippedIndex)
        //{
        //    Boundary inputBoundary;
        //    int adjustedIndex = strippedIndex;

        //    for (int i = 0; i < _outputBoundaries.Count; i++)
        //    {
        //        inputBoundary = _outputBoundaries[i];

        //        if (inputBoundary.Index > adjustedIndex)
        //        {
        //            return adjustedIndex;
        //        }

        //        adjustedIndex += inputBoundary.Length;
        //    }

        //    return adjustedIndex;
        //}


        //private Location GetLocation(int index)
        //{
        //    Location result = new Location();

        //    if (index == 0)
        //    {
        //        result.Line = 1;
        //        result.Column = 1;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < _lineEnds.Count; i++)
        //        {
        //            if (_lineEnds[i] >= index)
        //            {
        //                result.Line = i;
        //                result.Column = index - _lineEnds[i - 1];

        //                break;
        //            }
        //        }
        //    }

        //    return result;
        //}
    }
}
