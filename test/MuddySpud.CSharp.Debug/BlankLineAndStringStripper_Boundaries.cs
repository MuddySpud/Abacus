//using Microsoft.ApplicationInspector.RulesEngine;
//using MuddySpud.Metrics.Engine;
//using MuddySpud.RulesEngine;
//using MuddySpud.RulesEngine.Commands;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using System.Text;
//using Xunit;

//namespace MuddySpud.RegularExpression.Tests
//{
//    public class BlankLineAndStringStripper_Boundaries
//    {
//        private readonly string _code = String.Empty;
//        private readonly string _result = String.Empty;
//        private readonly List<MtBoundary> _outputBoundaries = new();
//        private readonly List<int> _lineEnds = new List<int>() { 0 };
//        private readonly List<int> _lineStarts = new List<int>() { 0, 0 };

//        public BlankLineAndStringStripper_Boundaries()
//        {
//            Assembly thisAssembly = Assembly.GetExecutingAssembly();
//            string? dirPath = Path.GetDirectoryName(thisAssembly.Location);

//            if (dirPath is null
//                || String.IsNullOrWhiteSpace(dirPath))
//            {
//                throw new ArgumentNullException(nameof(dirPath));
//            }

//            string path = Path.Combine(dirPath, "Files\\LineStripping.cs");
//            _code = File.ReadAllText(path);

//            int pos = 0;

//            while (pos > -1 && pos < _code.Length)
//            {
//                if (++pos < _code.Length)
//                {
//                    pos = _code.IndexOf('\n', pos);
//                    _lineEnds.Add(pos);

//                    if (pos > 0 && pos + 1 < _code.Length)
//                    {
//                        _lineStarts.Add(pos + 1);
//                    }
//                }
//            }

//            StringBuilder stringBuilder = new(_code);

//            List<MtBoundary> inputBoundaries = new();
//            List<MtBoundary> blankLineBoundaries = new();
//            StringSettings stringSettings = MtLanguage.GetStringSettings("csharp");


//            BlankLineStripLoopCache blankLineCache = new(
//                inputBoundaries,
//                blankLineBoundaries);

//            stringBuilder.StripBlankLines(blankLineCache);

//            StringStripLoopCache stringCache = new(
//                blankLineBoundaries,
//                _outputBoundaries,
//                new(),
//                stringSettings
//            );

//            stringBuilder.StripStrings(stringCache);
//            _result = stringBuilder.ToString();
//        }

//        [Theory]
//        [InlineData(19, 17, "throw1")]
//        [InlineData(24, 17, "throw2")]
//        [InlineData(27, 13, "string url1")]
//        [InlineData(29, 13, "string url2")]
//        [InlineData(30, 13, "string body3")]
//        [InlineData(31, 13, "string url4")]
//        [InlineData(32, 13, "string url5")]
//        [InlineData(33, 13, "string body6")]
//        [InlineData(34, 13, "string url7")]
//        [InlineData(35, 13, "string url8")]
//        [InlineData(36, 13, "string body9")]
//        [InlineData(37, 13, "string url10")]
//        [InlineData(38, 13, "string url11")]
//        [InlineData(39, 13, "string url12")]
//        [InlineData(40, 13, "string url13")]
//        [InlineData(41, 13, "string url14")]
//        [InlineData(42, 13, "string url15")]
//        [InlineData(43, 13, "string body16")]
//        [InlineData(60, 13, "string url17")]
//        public void LineColumnNumbers(
//            int line,
//            int column,
//            string needle)
//        {
//            int index = _result.IndexOf(needle);
//            int adjustedIndex = AdjustLineNumber(index);
//            Location location = GetLocation(adjustedIndex);

//            Assert.Equal(line, location.Line);
//            Assert.Equal(column, location.Column);
//        }

//        private int AdjustLineNumber(int strippedIndex)
//        {
//            MtBoundary inputBoundary;
//            int adjustedIndex = strippedIndex;

//            for (int i = 0; i < _outputBoundaries.Count; i++)
//            {
//                inputBoundary = _outputBoundaries[i];

//                if (inputBoundary.Index > adjustedIndex)
//                {
//                    return adjustedIndex;
//                }

//                adjustedIndex += inputBoundary.Length;
//            }

//            return adjustedIndex;
//        }


//        private Location GetLocation(int index)
//        {
//            Location result = new Location();

//            if (index == 0)
//            {
//                result.Line = 1;
//                result.Column = 1;
//            }
//            else
//            {
//                for (int i = 0; i < _lineEnds.Count; i++)
//                {
//                    if (_lineEnds[i] >= index)
//                    {
//                        result.Line = i;
//                        result.Column = index - _lineEnds[i - 1];

//                        break;
//                    }
//                }
//            }

//            return result;
//        }
//    }
//}
