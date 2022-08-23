using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class StructureFile_DebugBlockPatterns
    {
        private readonly string _full = String.Empty;
        private readonly string _code = String.Empty;
        private readonly BlockStatsCache _blockStatsCache;
        private readonly BlockTextContainer _codeContainer;
        private readonly List<Location> _lineEnds;

        public StructureFile_DebugBlockPatterns()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string? dirPath = Path.GetDirectoryName(thisAssembly.Location);

            if (dirPath is null
                || String.IsNullOrWhiteSpace(dirPath))
            {
                throw new ArgumentNullException(nameof(dirPath));
            }

            string path = Path.Combine(dirPath, "Files\\CSharp8.cs");
            string code = File.ReadAllText(path);

            _codeContainer = new(
                code,
                "csharp",
                0,
                true);

            _blockStatsCache = _codeContainer.BlockStatsCache;
            _full = _codeContainer.RawContent;
            _code = _codeContainer.CodeContent;
            _lineEnds = new();

            foreach (int index in _codeContainer.LineEnds)
            {
                _lineEnds.Add(_codeContainer.GetLocation(index));
            }
        }

        [Theory]
        //[InlineData(
        //    "indexer",
        //    "public A this[int i]",
        //    "public A this[int i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    540, 10,
        //    543, 9,
        //    539, 9,
        //    540, 10)]
        //[InlineData(
        //    "indexer",
        //    "public B this \r\n            [ \r\n            int i \r\n            ]",
        //    "public B this[int i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    549, 10,
        //    552, 9,
        //    545, 9,
        //    549, 10)]
        //[InlineData(
        //    "indexer",
        //    "public C this[int i]",
        //    "public C this[int i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    559, 32,
        //    559, 39,
        //    559, 9,
        //    559, 32)]
        //[InlineData(
        //    "indexer",
        //    "public D this[int i]",
        //    "public D this[int i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    561, 32,
        //    562, 19,
        //    561, 9,
        //    561, 32)]
        //[InlineData(
        //    "indexer",
        //    "public E this[int i]",
        //    "public E this[int i]",
        //    "this",
        //    "JsonEdgesParser2.this",
        //    565, 15,
        //    566, 19,
        //    564, 9,
        //    565, 15)]
        [InlineData(
            "indexer",
            "public F this[int i]",
            "public F this[int i]",
            "this",
            "JsonEdgesParser2.this",
            10, 15,
            10, 22,
            9, 9,
            10, 15)]
        //[InlineData(
        //    "indexer",
        //    "public G this[string i]",
        //    "public G this[string i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    583, 10,
        //    586, 9,
        //    582, 9,
        //    583, 10)]
        //[InlineData(
        //    "indexer",
        //    "public Task<SortedList<int, string>> this[string i]",
        //    "public Task<SortedList<int, string>> this[string i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    589, 10,
        //    595, 9,
        //    588, 9,
        //    589, 10)]
        //[InlineData(
        //    "indexer",
        //    "public Task < SortedList < int , ( string fred , int joe ) > > this [ string i ]",
        //    "public Task<SortedList<int, (string fred, int joe)>> this[string i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    601, 10,
        //    607, 9,
        //    600, 9,
        //    601, 10)]
        //[InlineData(
        //    "indexer",
        //    "public Task<SortedList<int, (string fred, int[] joe)>> this[int i]",
        //    "public Task<SortedList<int, (string fred, int[] joe)>> this[int i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    610, 10,
        //    613, 9,
        //    609, 9,
        //    610, 10)]
        //[InlineData(
        //    "indexer",
        //    "public Task<SortedList<int, (string fred, \r\n            int [\r\n            ] \r\n            joe)>> this[int i]",
        //    "public Task<SortedList<int, (string fred, int[] joe)>> this[int i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    619, 10,
        //    622, 9,
        //    615, 9,
        //    619, 10)]
        //[InlineData(
        //    "indexer",
        //    "public Task <  SortedList < \r\n            int, (  string fred,  \r\n            int joe ) > > this\r\n              [int i\r\n            ]",
        //    "public Task<SortedList<int, (string fred, int joe)>> this[int i]",
        //    "this",
        //    "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
        //    636, 10,
        //    642, 9,
        //    631, 9,
        //    636, 10)]
        public void IndexerMatches(
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
                fullName,
                componentType
            );

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

            Assert.Equal(false, block?.Flags.Contains("partial"));
        }

        [Theory]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "double HighTemp, double LowTemp",
        //    1, 31,
        //    1, 62)]
        //[InlineData(
        //    "Code",
        //    "Curly",
        //    "{",
        //    "}",
        //    "\r\n    public double Mean => (HighTemp + LowTemp) / 2.0;\r\n\r\n    public double Add(int 20)\r\n    {\r\n        return new DailyTemperature(HighTemp + 20, LowTemp);\r\n    }\r\n",
        //    2, 2,
        //    9, 1)]
        //[InlineData(
        //    "Code",
        //    "Lambda",
        //    "=>",
        //    ";",
        //    " (HighTemp + LowTemp) / 2.0",
        //    3, 26,
        //    3, 53)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "HighTemp + LowTemp",
        //    3, 28,
        //    3, 46)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "int 20",
        //    5, 23,
        //    5, 29)]
        //[InlineData(
        //    "Code",
        //    "Curly",
        //    "{",
        //    "}",
        //    "\r\n        return new DailyTemperature(HighTemp + 20, LowTemp);\r\n    ",
        //    6, 6,
        //    8, 5)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "HighTemp + 20, LowTemp",
        //    7, 37,
        //    7, 59)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "double BaseTemperature, IEnumerable<DailyTemperature> TempRecords",
        //    11, 35,
        //    11, 100)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "double BaseTemperature, IEnumerable<DailyTemperature> TempRecords",
        //    13, 40,
        //    13, 105)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "BaseTemperature, TempRecords",
        //    14, 18,
        //    14, 46)]
        //[InlineData(
        //    "Code",
        //    "Curly",
        //    "{",
        //    "}",
        //    "\r\n    public double DegreeDays => TempRecords.Where(s => { return s.Mean < BaseTemperature; }).Sum(s => BaseTemperature - s.Mean);\r\n",            15, 2,
        //    17, 1)]
        //[InlineData(
        //    "Code",
        //    "Lambda",
        //    "=>",
        //    ";",
        //    " TempRecords.Where(s => { return s.Mean < BaseTemperature; }).Sum(s => BaseTemperature - s.Mean)",
        //    16, 32,
        //    16, 128)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "s => { return s.Mean < BaseTemperature; }",
        //    16, 51,
        //    16, 92)]
        //[InlineData(
        //    "Code",
        //    "Lambda",
        //    "=>",
        //    ";",
        //    " { return s.Mean < BaseTemperature; }",
        //    16, 55,
        //    16, 92)]
        //[InlineData(
        //    "Code",
        //    "Curly",
        //    "{",
        //    "}",
        //    " return s.Mean < BaseTemperature; ",
        //    16, 57,
        //    16, 91)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "s => BaseTemperature - s.Mean",
        //    16, 98,
        //    16, 127)]
        //[InlineData(
        //    "Code",
        //    "Lambda",
        //    "=>",
        //    ";",
        //    " BaseTemperature - s.Mean",
        //    16, 102,
        //    16, 127)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "\r\n    double BaseTemperature,\r\n    IEnumerable<DailyTemperature> TempRecords\r\n    ",
        //    20, 6,
        //    23, 5)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "BaseTemperature,\r\n        TempRecords",
        //    24, 18,
        //    25, 20)]
        //[InlineData(
        //    "Code",
        //    "Curly",
        //    "{",
        //    "}",
        //    "\r\n    public double DegreeDays\r\n    {\r\n        get => TempRecords.Where(s => s.Mean > BaseTemperature).Sum(s => s.Mean - BaseTemperature);\r\n    }\r\n",
        //    26, 2,
        //    31, 1)]
        //[InlineData(
        //    "Code",
        //    "Curly",
        //    "{",
        //    "}",
        //    "\r\n        get => TempRecords.Where(s => s.Mean > BaseTemperature).Sum(s => s.Mean - BaseTemperature);\r\n    ",
        //    28, 6,
        //    30, 5)]
        //[InlineData(
        //    "Code",
        //    "Lambda",
        //    "=>",
        //    ";",
        //    " TempRecords.Where(s => s.Mean > BaseTemperature).Sum(s => s.Mean - BaseTemperature)",
        //    29, 15,
        //    29, 99)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "s => s.Mean > BaseTemperature",
        //    29, 34,
        //    29, 63)]
        //[InlineData(
        //    "Code",
        //    "Lambda",
        //    "=>",
        //    ";",
        //    " s.Mean > BaseTemperature",
        //    29, 38,
        //    29, 63)]
        //[InlineData(
        //    "Parameter",
        //    "Round",
        //    "(",
        //    ")",
        //    "s => s.Mean - BaseTemperature",
        //    29, 69,
        //    29, 98)]
        //[InlineData(
        //    "Code",
        //    "Lambda",
        //    "=>",
        //    ";",
        //    " s.Mean - BaseTemperature",
        //    29, 73,
        //    29, 98)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n    /// <summary>\r\n    /// Retrieve or manipulate individual nodes\r\n    /// </summary>\r\n    public class PreProcessorTest\r\n    {\r\n#pragma warning disable CA1056 // Uri properties should not be strings\r\n        public string Url { get; set; }\r\n#pragma warning restore CA1056 // Uri properties should not be strings\r\n        /// <summary>\r\n        /// Test\r\n        /// </summary>\r\n        /// <param name=\"name\">Name</param>\r\n        public void method1(string name)\r\n        {\r\n            Func<(string, bool)> fred = () =>\r\n            {\r\n                string h = \"\";\r\n                bool intime = true;\r\n\r\n                return (h, intime);\r\n            };\r\n\r\n#if DEBUG\r\n            Console.WriteLine($\"{DateTime.Now.ToString(\"yyyy/MM/dd_HH:mm:ss::fff\")} - ProcessID = {_processState.ID}\");\r\n#endif\r\n        }\r\n    }\r\n\r\n\r\n",
            200, 2,
            230, 1)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n#pragma warning disable CA1056 // Uri properties should not be strings\r\n        public string Url { get; set; }\r\n#pragma warning restore CA1056 // Uri properties should not be strings\r\n        /// <summary>\r\n        /// Test\r\n        /// </summary>\r\n        /// <param name=\"name\">Name</param>\r\n        public void method1(string name)\r\n        {\r\n            Func<(string, bool)> fred = () =>\r\n            {\r\n                string h = \"\";\r\n                bool intime = true;\r\n\r\n                return (h, intime);\r\n            };\r\n\r\n#if DEBUG\r\n            Console.WriteLine($\"{DateTime.Now.ToString(\"yyyy/MM/dd_HH:mm:ss::fff\")} - ProcessID = {_processState.ID}\");\r\n#endif\r\n        }\r\n    ",
            205, 6,
            227, 5)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            " get; set; ",
            207, 28,
            207, 39)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "string name",
            213, 29,
            213, 40)]

        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n            Func<(string, bool)> fred = () =>\r\n            {\r\n                string h = \"\";\r\n                bool intime = true;\r\n\r\n                return (h, intime);\r\n            };\r\n\r\n#if DEBUG\r\n            Console.WriteLine($\"{DateTime.Now.ToString(\"yyyy/MM/dd_HH:mm:ss::fff\")} - ProcessID = {_processState.ID}\");\r\n#endif\r\n        ",
            214, 10,
            226, 9)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "string, bool",
            215, 19,
            215, 31)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "",
            215, 42,
            215, 42)]
        [InlineData(
            "Code",
            "Lambda",
            "=>",
            ";",
            "\r\n            {\r\n                string h = \"\";\r\n                bool intime = true;\r\n\r\n                return (h, intime);\r\n            }",
            215, 46,
            221, 14)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n                string h = \"\";\r\n                bool intime = true;\r\n\r\n                return (h, intime);\r\n            ",
            216, 14,
            221, 13)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "h, intime",
            220, 25,
            220, 34)]
        public void ParameterBlockMatches(
            string blockType,
            string blockName,
            string open,
            string close,
            string blockContents,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn)
        {
            bool found = false;

            foreach (BlockStats block in _blockStatsCache.BlockStats)
            {
                if (block.BlockStartLocation.Line == blockStartLine
                    && block.BlockStartLocation.Column == blockStartColumn)
                {
                    Assert.Equal(blockEndLine, block.BlockEndLocation.Line);
                    Assert.Equal(blockEndColumn, block.BlockEndLocation.Column);
                    Assert.Equal(blockType, block.Settings.BlockType);
                    Assert.Equal(blockName, block.Settings.Name);
                    Assert.Equal(open, block.Settings.Open);
                    Assert.Equal(close, block.Settings.Close);
                    Assert.Equal(blockContents, block.Content);

                    found = true;
                }
            }

            Assert.True(found);
        }

        [Fact]
        public void OpenAndCloseBlockMatches()
        {
            foreach (BlockStats block in _blockStatsCache.BlockStats)
            {
                string open = _full.Substring(block.AdjustedOpenIndex - block.Settings.Open.Length, block.Settings.Open.Length);
                string close = _full.Substring(block.AdjustedCloseIndex, block.Settings.Close.Length);

                Assert.Equal(block.Settings.Open, open);

                if (block.Settings.ExplicitClose)
                {
                    Assert.Equal(block.Settings.Close, close);
                }
            }
        }

        private BlockStats? GetBlockBySignature(
            string signature,
            string fullName,
            string type)
        {
            foreach (BlockStats block in _blockStatsCache.BlockStats)
            {
                if (String.Equals(block?.Signature, signature, StringComparison.Ordinal)
                    && String.Equals(block?.FullName, fullName, StringComparison.Ordinal)
                    && String.Equals(block?.Type, type, StringComparison.Ordinal))
                {
                    return block;
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
    }
}
