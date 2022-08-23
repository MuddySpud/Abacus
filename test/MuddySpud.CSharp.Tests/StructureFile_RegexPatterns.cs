using MuddySpud.CSharp.Tests.Fixtures;
using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine;
using System;
using System.IO;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class StructureFile_RegexPatterns : IClassFixture<RegexFixture>
    {
        RegexFixture _fixture;

        public StructureFile_RegexPatterns(RegexFixture fixture)
        {
            _fixture = fixture;
        }

        #region namespace

        [Theory]
        [InlineData(
            "namespace",
            "namespace TreeSolve.CompositeFlows.Export.JsonParsers",
            "namespace TreeSolve.CompositeFlows.Export.JsonParsers",
            "TreeSolve.CompositeFlows.Export.JsonParsers",
            "TreeSolve.CompositeFlows.Export.JsonParsers",
            9, 2,
            909, 1,
            8, 1,
            9, 2)]
        [InlineData(
            "namespace",
            "namespace TreeSolve.CompositeFlows.Export",
            "namespace TreeSolve.CompositeFlows.Export",
            "TreeSolve.CompositeFlows.Export",
            "TreeSolve.CompositeFlows.Export",
            931, 2,
            993, 1,
            930, 1,
            931, 2)]
        [InlineData(
            "namespace",
            "namespace Nested",
            "namespace Nested",
            "Nested",
            "TreeSolve.CompositeFlows.Export.Nested",
            961, 6,
            992, 5,
            960, 5,
            961, 6)]
        [InlineData(
            "namespace",
            "namespace Two",
            "namespace Two",
            "Two",
            "TreeSolve.CompositeFlows.Export.Nested.Two",
            963, 10,
            991, 9,
            962, 9,
            963, 10)]
        public void NamespaceMatches(
            string componentType,
            string signature,
            string cleanedSignature,
            string componentName,
            string fullName,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn,
            int matchStartLine,
            int matchStartColumn,
            int matchEndLine,
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
        #endregion

        #region interface

        [Theory]
        [InlineData(
            "interface",
            "internal interface ISortedEdges",
            "internal interface ISortedEdges",
            "ISortedEdges",
            "TreeSolve.CompositeFlows.Export.JsonParsers.ISortedEdges",
            11, 6,
            13, 5,
            10, 5,
            11, 6)]
        [InlineData(
            "interface",
            "internal interface IGraphKeyParser",
            "internal interface IGraphKeyParser",
            "IGraphKeyParser",
            "TreeSolve.CompositeFlows.Export.JsonParsers.IGraphKeyParser",
            16, 6,
            18, 5,
            15, 5,
            16, 6)]
        [InlineData(
            "interface",
            "internal interface IJsonEdgesParser",
            "internal interface IJsonEdgesParser",
            "IJsonEdgesParser",
            "TreeSolve.CompositeFlows.Export.JsonParsers.IJsonEdgesParser",
            21, 6,
            27, 5,
            20, 5,
            21, 6)]
        public void InterfaceMatches(
            string componentType,
            string signature,
            string cleanedSignature,
            string componentName,
            string fullName,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn,
            int matchStartLine,
            int matchStartColumn,
            int matchEndLine,
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
        #endregion

        #region class

        [Theory]
        [InlineData(
            "class",
            "public class JsonEdgesParser",
            "public class JsonEdgesParser",
            "JsonEdgesParser",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser",
            30, 6,
            535, 5,
            29, 5,
            30, 6)]
        [InlineData(
            "class",
            "public \r\n    static \r\n    class \r\n    ClassWithoutNamespace <\r\n    T\r\n    >",
            "public static class ClassWithoutNamespace<T>",
            "ClassWithoutNamespace<T>",
            "ClassWithoutNamespace<T>",
            921, 2,
            926, 1,
            914, 1,
            921, 2)]
        [InlineData(
            "class",
            "class BaseNode",
            "class BaseNode",
            "BaseNode",
            "TreeSolve.CompositeFlows.Export.BaseNode",
            932, 21,
            932, 22,
            932, 5,
            932, 21)]
        [InlineData(
            "class",
            "class BaseNodeGeneric<T>",
            "class BaseNodeGeneric<T>",
            "BaseNodeGeneric<T>",
            "TreeSolve.CompositeFlows.Export.BaseNodeGeneric<T>",
            933, 31,
            933, 32,
            933, 5,
            933, 31)]
        [InlineData(
            "class",
            "class NodeConcrete<T>",
            "class NodeConcrete<T>",
            "NodeConcrete<T>",
            "TreeSolve.CompositeFlows.Export.NodeConcrete<T>",
            936, 39,
            936, 40,
            936, 5,
            936, 39)]
        [InlineData(
            "class",
            "class NodeClosed<T>",
            "class NodeClosed<T>",
            "NodeClosed<T>",
            "TreeSolve.CompositeFlows.Export.NodeClosed<T>",
            939, 49,
            939, 50,
            939, 5,
            939, 49)]
        [InlineData(
            "class",
            "class NodeOpen<T>",
            "class NodeOpen<T>",
            "NodeOpen<T>",
            "TreeSolve.CompositeFlows.Export.NodeOpen<T>",
            942, 45,
            942, 46,
            942, 5,
            942, 45)]
        [InlineData(
            "class",
            "class BaseNodeMultiple<T, U>",
            "class BaseNodeMultiple<T, U>",
            "BaseNodeMultiple<T, U>",
            "TreeSolve.CompositeFlows.Export.BaseNodeMultiple<T, U>",
            944, 35,
            944, 36,
            944, 5,
            944, 35)]
        [InlineData(
            "class",
            "class Node4<T>",
            "class Node4<T>",
            "Node4<T>",
            "TreeSolve.CompositeFlows.Export.Node4<T>",
            947, 48,
            947, 49,
            947, 5,
            947, 48)]
        [InlineData(
            "class",
            "class Node5<T, U>",
            "class Node5<T, U>",
            "Node5<T, U>",
            "TreeSolve.CompositeFlows.Export.Node5<T, U>",
            950, 49,
            950, 50,
            950, 5,
            950, 49)]
        [InlineData(
            "class",
            "class NodeItem<T>",
            "class NodeItem<T>",
            "NodeItem<T>",
            "TreeSolve.CompositeFlows.Export.NodeItem<T>",
            952, 63,
            952, 64,
            952, 5,
            952, 63)]
        [InlineData(
            "class",
            "class SpecialNodeItem<T>",
            "class SpecialNodeItem<T>",
            "SpecialNodeItem<T>",
            "TreeSolve.CompositeFlows.Export.SpecialNodeItem<T>",
            953, 84,
            953, 85,
            953, 5,
            953, 84)]
        [InlineData(
            "class",
            "class SuperKeyType<K, V, U>",
            "class SuperKeyType<K, V, U>",
            "SuperKeyType<K, V, U>",
            "TreeSolve.CompositeFlows.Export.SuperKeyType<K, V, U>",
            958, 6,
            958, 7,
            955, 5,
            958, 6)]
        public void ClassMatches(
            string componentType,
            string signature,
            string cleanedSignature,
            string componentName,
            string fullName,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn,
            int matchStartLine,
            int matchStartColumn,
            int matchEndLine,
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

        #region class constructor

        [Theory]
        [InlineData(
            "constructor",
            "public JsonEdgesParser()",
            "public JsonEdgesParser()",
            "JsonEdgesParser",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.JsonEdgesParser",
            69, 10,
            71, 9,
            68, 9,
            69, 10)]
        [InlineData(
            "constructor",
            "JsonEdgesParser(string fred)",
            "JsonEdgesParser(string fred)",
            "JsonEdgesParser",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.JsonEdgesParser",
            75, 10,
            77, 9,
            74, 9,
            75, 10)]
        [InlineData(
            "constructor",
            "JsonEdgesParser((string fred, int joe) gaz, double kez = \"\")",
            "JsonEdgesParser((string fred, int joe) gaz, double kez = \"\")",
            "JsonEdgesParser",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.JsonEdgesParser",
            80, 10,
            82, 9,
            79, 9,
            80, 10)]
        [InlineData(
            "constructor",
            "JsonEdgesParser(Dictionary<string fred, int joe> gaz, double kez)",
            "JsonEdgesParser(Dictionary<string fred, int joe> gaz, double kez)",
            "JsonEdgesParser",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.JsonEdgesParser",
            85, 10,
            87, 9,
            84, 9,
            85, 10)]
        [InlineData(
            "constructor",
            "public NestedConcrete(int i)",
            "public NestedConcrete(int i)",
            "NestedConcrete",
            "TreeSolve.CompositeFlows.Export.Nested.Two.ParentConcrete<T>.NestedConcrete.NestedConcrete",
            986, 22,
            988, 21,
            985, 21,
            986, 22)]
        public void ClassConstructorMatches(
            string componentType,
            string signature,
            string cleanedSignature,
            string componentName,
            string fullName,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn,
            int matchStartLine,
            int matchStartColumn,
            int matchEndLine,
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

        #endregion

        #region class destructor

        [Theory]
        [InlineData(
            "destructor",
            "~JsonEdgesParser()",
            "~JsonEdgesParser()",
            "~JsonEdgesParser",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.~JsonEdgesParser",
            90, 10,
            91, 9,
            89, 9,
            90, 10)]
        public void ClassDestructorMatches(
            string componentType,
            string signature,
            string cleanedSignature,
            string componentName,
            string fullName,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn,
            int matchStartLine,
            int matchStartColumn,
            int matchEndLine,
            int matchEndColumn)
        {
            BlockStats? block = GetBlockBySignature(
                signature,
                fullName,
                "destructor"
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
        #endregion
        #endregion

        #region struct

        [Theory]
        [InlineData(
            "struct",
            "public readonly struct Fraction",
            "public readonly struct Fraction",
            "Fraction",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction",
            773, 6,
            859, 5,
            772, 5,
            773, 6)]
        public void StructMatches(
            string componentType,
            string signature,
            string cleanedSignature,
            string componentName,
            string fullName,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn,
            int matchStartLine,
            int matchStartColumn,
            int matchEndLine,
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

        #region struct constructor

        [Theory]
        [InlineData(
            "constructor",
            "public Fraction(int numerator, int denominator)",
            "public Fraction(int numerator, int denominator)",
            "Fraction",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.Fraction",
            784, 10,
            791, 9,
            783, 9,
            784, 10)]
        public void StructConstructorMatches(
            string componentType,
            string signature,
            string cleanedSignature,
            string componentName,
            string fullName,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn,
            int matchStartLine,
            int matchStartColumn,
            int matchEndLine,
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

        #endregion
        #endregion

        #region record

        [Theory]
        [InlineData(
            "record",
            "public record DailyTemperature ( double HighTemp , double LowTemp )",
            "public record DailyTemperature(double HighTemp, double LowTemp)",
            "DailyTemperature",
            "TreeSolve.CompositeFlows.Export.JsonParsers.DailyTemperature",
            876, 6,
            883, 5,
            875, 5,
            876, 6)]
        [InlineData(
            "record",
            "public sealed record HeatingDegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords)",
            "public sealed record HeatingDegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords)",
            "HeatingDegreeDays",
            "TreeSolve.CompositeFlows.Export.JsonParsers.HeatingDegreeDays",
            889, 6,
            891, 5,
            887, 5,
            889, 6)]
        [InlineData(
            "record",
            "public sealed record CoolingDegreeDays\r\n        (\r\n        double BaseTemperature, \r\n        IEnumerable < DailyTemperature > TempRecords\r\n        )",
            "public sealed record CoolingDegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords)",
            "CoolingDegreeDays",
            "TreeSolve.CompositeFlows.Export.JsonParsers.CoolingDegreeDays",
            900, 6,
            905, 5,
            893, 5,
            900, 6)]
        public void RecordMatches(
            string componentType,
            string signature,
            string cleanedSignature,
            string componentName,
            string fullName,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn,
            int matchStartLine,
            int matchStartColumn,
            int matchEndLine,
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

        #endregion

        #region method

        [Theory]
        [InlineData(
            "method",
            "public ISortedEdges BuildEdges1(\r\n            string filePath,\r\n            IGraphKeyParser graphKeyParser)",
            "public ISortedEdges BuildEdges1(string filePath, IGraphKeyParser graphKeyParser)",
            "BuildEdges1",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.BuildEdges1",
            97, 10,
            171, 9,
            94, 9,
            97, 10)]
        [InlineData(
            "method",
            "internal async Task<string> GetNodeOptionsAndParent2(KeyGuid keyGuid)",
            "internal async Task<string> GetNodeOptionsAndParent2(KeyGuid keyGuid)",
            "GetNodeOptionsAndParent2",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.GetNodeOptionsAndParent2",
            174, 10,
            196, 9,
            173, 9,
            174, 10)]
        [InlineData(
            "method",
            "internal static async Task<string> ValidateKey3(KeyGuid keyGuid)",
            "internal static async Task<string> ValidateKey3(KeyGuid keyGuid)",
            "ValidateKey3",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.ValidateKey3",
            254, 10,
            269, 9,
            253, 9,
            254, 10)]
        [InlineData(
            "method",
            "internal async static Task<(string fred, List<int> joe)> ValidateKey4(KeyGuid keyGuid,\r\n            string harry,\r\n            Dictionary<string, int> bert)",
            "internal async static Task<(string fred, List<int> joe)> ValidateKey4(KeyGuid keyGuid, string harry, Dictionary<string, int> bert)",
            "ValidateKey4",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.ValidateKey4",
            274, 10,
            289, 9,
            271, 9,
            274, 10)]
        [InlineData(
            "method",
            "protected Task < ( string  fred , List < int [   ] > joe ) > ValidateKey7 (\r\n            KeyGuid keyGuid,\r\n            string harry,\r\n            Dictionary < string , int [ ] > bert )",
            "protected Task<(string fred, List<int[]> joe)> ValidateKey7(KeyGuid keyGuid, string harry, Dictionary<string, int[]> bert)",
            "ValidateKey7",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.ValidateKey7",
            337, 10,
            352, 9,
            333, 9,
            337, 10)]
        [InlineData(
            "method",
            "private Task<(string fred, (string fred, List<int> joe) )[   ]> ValidateKey8(\r\n            KeyGuid keyGuid,\r\n            string[  ] harry,\r\n            Dictionary<string, int> bert)",
            "private Task<(string fred, (string fred, List<int> joe))[]> ValidateKey8(KeyGuid keyGuid, string[] harry, Dictionary<string, int> bert)",
            "ValidateKey8",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.ValidateKey8",
            359, 10,
            374, 9,
            355, 9,
            359, 10)]
        [InlineData(
            "method",
            "private (string fred, List<int> joe) ValidateKey9(\r\n            KeyGuid keyGuid,\r\n            string harry,\r\n            Dictionary<string, int> bert)",
            "private (string fred, List<int> joe) ValidateKey9(KeyGuid keyGuid, string harry, Dictionary<string, int> bert)",
            "ValidateKey9",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.ValidateKey9",
            380, 10,
            395, 9,
            376, 9,
            380, 10)]
        [InlineData(
            "method",
            "(string fred, List<int> joe) ValidateKey10(\r\n            KeyGuid keyGuid,\r\n            string harry,\r\n            Dictionary<string, int> bert)",
            "(string fred, List<int> joe) ValidateKey10(KeyGuid keyGuid, string harry, Dictionary<string, int> bert)",
            "ValidateKey10",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.ValidateKey10",
            401, 10,
            416, 9,
            397, 9,
            401, 10)]
        [InlineData(
            "method",
            "public override string ToString11()",
            "public override string ToString11()",
            "ToString11",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.ToString11",
            418, 47,
            418, 73,
            418, 9,
            418, 47)]
        [InlineData(
            "method",
            "public override string ToString12()",
            "public override string ToString12()",
            "ToString12",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.ToString12",
            421, 10,
            434, 9,
            420, 9,
            421, 10)]
        [InlineData(
            "method",
            "internal abstract static Task<string> MoveBranch13(MoveBranch moveBranch = 1)",
            "internal abstract static Task<string> MoveBranch13(MoveBranch moveBranch = 1)",
            "MoveBranch13",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.MoveBranch13",
            437, 10,
            450, 9,
            436, 9,
            437, 10)]
        [InlineData(
            "method",
            "internal abstract static Task<string> MoveBranch14()",
            "internal abstract static Task<string> MoveBranch14()",
            "MoveBranch14",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.MoveBranch14",
            453, 10,
            466, 9,
            452, 9,
            453, 10)]
        [InlineData(
            "method",
            "internal abstract static Task<string> MoveBranch15<T, U>()",
            "internal abstract static Task<string> MoveBranch15<T, U>()",
            "MoveBranch15<T, U>",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.MoveBranch15<T, U>",
            469, 10,
            485, 9,
            468, 9,
            469, 10)]
        [InlineData(
            "method",
            "internal abstract static Task<   string   > MoveBranch16   <    T,    U   >     (     \r\n            string      \r\n            fred     \r\n                  )",
            "internal abstract static Task<string> MoveBranch16<T, U>(string fred)",
            "MoveBranch16<T, U>",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.MoveBranch16<T, U>",
            499, 10,
            532, 9,
            489, 9,
            499, 10)]
        [InlineData(
            "method",
            "internal abstract static Task < string> MoveBranch14 (  )",
            "internal abstract static Task<string> MoveBranch14()",
            "MoveBranch14",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.MoveBranch14",
            845, 10,
            858, 9,
            844, 9,
            845, 10)]
        [InlineData(
            "method",
            "public double Add(int 20)",
            "public double Add(int 20)",
            "Add",
            "TreeSolve.CompositeFlows.Export.JsonParsers.DailyTemperature.Add",
            880, 10,
            882, 9,
            879, 9,
            880, 10)]
        [InlineData(
            "method",
            "public static void Main ( )",
            "public static void Main()",
            "Main",
            "TreeSolve.CompositeFlows.Export.JsonParsers.OperatorOverloading.Main",
            864, 10,
            872, 9,
            863, 9,
            864, 10)]
        [InlineData(
            "method",
            "internal Task<string> TestNested()",
            "internal Task<string> TestNested()",
            "TestNested",
            "TreeSolve.CompositeFlows.Export.Nested.Two.ParentConcrete<T>.NestedConcrete.TestNested",
            969, 22,
            982, 21,
            968, 21,
            969, 22)]
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

        #region method recursion

        [Theory]
        [InlineData(
            "method",
            "internal partial Task<string> TestNested(string fred)",
            "internal partial Task<string> TestNested(string fred)",
            "TestNested",
            "Recursion.Concrete.TestNested",
            1143, 10,
            1158, 9,
            1142, 9,
            1143, 10,
            1153, 24,
            1153, 31)]
        public void RecusrionMatches(
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
            int matchEndColumn,
            int recursionBlockStartLine, //
            int recursionBlockStartColumn,
            int recursionBlockEndLine,   //
            int recursionBlockEndColumn)
        {
            BlockStats? block = GetBlockBySignature(
                signature,
                fullName,
                componentType
            );

            Assert.NotNull(block);
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

            if (block is { })
            {
                BlockStats? recursionBlock = FindDescendantBlock(
                    block,
                    recursionBlockStartLine,
                    recursionBlockStartColumn,
                    recursionBlockEndLine,
                    recursionBlockEndColumn
                );

                Assert.NotNull(recursionBlock);
                Assert.Equal("recursion", recursionBlock?.Type);
                Assert.Equal(componentName, recursionBlock?.Name);
            }
        }
        #endregion

        #endregion

        #region properties

        [Theory]
        [InlineData(
            "property",
            "public int Property1",
            "public int Property1",
            "Property1",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.Property1",
            33, 31,
            33, 42,
            33, 9,
            33, 31)]
        [InlineData(
            "property",
            "public int Property2",
            "public int Property2",
            "Property2",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.Property2",
            36, 10,
            39, 9,
            35, 9,
            36, 10)]
        [InlineData(
            "property",
            "public int Property3",
            "public int Property3",
            "Property3",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.Property3",
            42, 10,
            45, 9,
            41, 9,
            42, 10)]
        [InlineData(
            "property",
            "public int Property4",
            "public int Property4",
            "Property4",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.Property4",
            48, 10,
            50, 9,
            47, 9,
            48, 10)]
        [InlineData(
            "property",
            "public int Property5",
            "public int Property5",
            "Property5",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.Property5",
            54, 10,
            57, 9,
            53, 9,
            54, 10)]
        [InlineData(
            "property",
            "public int Property6",
            "public int Property6",
            "Property6",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.Property6",
            61, 10,
            64, 9,
            60, 9,
            61, 10)]
        [InlineData(
            "property",
            "public string Property7",
            "public string Property7",
            "Property7",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser.Property7",
            66, 35,
            66, 38,
            66, 9,
            66, 35)]
        [InlineData(
            "property",
            "public int Property8",
            "public int Property8",
            "Property8",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.Property8",
            778, 10,
            781, 9,
            777, 9,
            778, 10)]
        [InlineData(
            "property",
            "public int Property8",
            "public int Property8",
            "Property8",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser3.Property8",
            714, 10,
            717, 9,
            713, 9,
            714, 10)]
        [InlineData(
            "property",
            "public int Property9",
            "public int Property9",
            "Property9",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser3.Property9",
            720, 10,
            723, 9,
            719, 9,
            720, 10)]
        [InlineData(
            "property",
            "public int Property10",
            "public int Property10",
            "Property10",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser3.Property10",
            727, 10,
            730, 9,
            726, 9,
            727, 10)]
        [InlineData(
            "property",
            "public string Property11",
            "public string Property11",
            "Property11",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser3.Property11",
            732, 36,
            732, 39,
            732, 9,
            732, 36)]
        [InlineData(
            "property",
            "public (int fred,\r\n            string joe) Property12",
            "public (int fred, string joe) Property12",
            "Property12",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser3.Property12",
            735, 38,
            735, 51,
            734, 9,
            735, 38)]
        [InlineData(
            "property",
            "public Task<SortedList<int, string>> Property13",
            "public Task<SortedList<int, string>> Property13",
            "Property13",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser3.Property13",
            736, 58,
            736, 64,
            736, 9,
            736, 58)]
        [InlineData(
            "property",
            "public SortedList<int, string> Property14",
            "public SortedList<int, string> Property14",
            "Property14",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser3.Property14",
            757, 53,
            757, 83,
            757, 9,
            757, 53)]
        [InlineData(
            "property",
            "public SortedList<int, string> Property15",
            "public SortedList<int, string> Property15",
            "Property15",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser3.Property15",
            760, 10,
            763, 9,
            759, 9,
            760, 10)]
        [InlineData(
            "property",
            "public double DegreeDays",
            "public double DegreeDays",
            "DegreeDays",
            "TreeSolve.CompositeFlows.Export.JsonParsers.HeatingDegreeDays.DegreeDays",
            890, 36,
            890, 132,
            890, 9,
            890, 36)]
        [InlineData(
            "property",
            "public double DegreeDays",
            "public double DegreeDays",
            "DegreeDays",
            "TreeSolve.CompositeFlows.Export.JsonParsers.CoolingDegreeDays.DegreeDays",
            902, 10,
            904, 9,
            901, 9,
            902, 10)]
        public void PropertyMatches(
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

        #endregion

        #region indexers

        [Theory]
        [InlineData(
            "indexer",
            "public A this[int i]",
            "public A this[int i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            540, 10,
            543, 9,
            539, 9,
            540, 10)]
        [InlineData(
            "indexer",
            "public B this \r\n            [ \r\n            int i \r\n            ]",
            "public B this[int i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            549, 10,
            552, 9,
            545, 9,
            549, 10)]
        [InlineData(
            "indexer",
            "public C this[int i]",
            "public C this[int i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            559, 32,
            559, 39,
            559, 9,
            559, 32)]
        [InlineData(
            "indexer",
            "public D this[int i]",
            "public D this[int i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            561, 32,
            562, 19,
            561, 9,
            561, 32)]
        [InlineData(
            "indexer",
            "public E this[int i]",
            "public E this[int i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            565, 15,
            566, 19,
            564, 9,
            565, 15)]
        [InlineData(
            "indexer",
            "public F this[int i]",
            "public F this[int i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            569, 15,
            569, 22,
            568, 9,
            569, 15)]
        [InlineData(
            "indexer",
            "public G this[string i]",
            "public G this[string i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            583, 10,
            586, 9,
            582, 9,
            583, 10)]
        [InlineData(
            "indexer",
            "public Task<SortedList<int, string>> this[string i]",
            "public Task<SortedList<int, string>> this[string i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            589, 10,
            595, 9,
            588, 9,
            589, 10)]
        [InlineData(
            "indexer",
            "public Task < SortedList < int , ( string fred , int joe ) > > this [ string i ]",
            "public Task<SortedList<int, (string fred, int joe)>> this[string i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            601, 10,
            607, 9,
            600, 9,
            601, 10)]
        [InlineData(
            "indexer",
            "public Task<SortedList<int, (string fred, int[] joe)>> this[int i]",
            "public Task<SortedList<int, (string fred, int[] joe)>> this[int i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            610, 10,
            613, 9,
            609, 9,
            610, 10)]
        [InlineData(
            "indexer",
            "public Task<SortedList<int, (string fred, \r\n            int [\r\n            ] \r\n            joe)>> this[int i]",
            "public Task<SortedList<int, (string fred, int[] joe)>> this[int i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            619, 10,
            622, 9,
            615, 9,
            619, 10)]
        [InlineData(
            "indexer",
            "public Task <  SortedList < \r\n            int, (  string fred,  \r\n            int joe ) > > this\r\n              [int i\r\n            ]",
            "public Task<SortedList<int, (string fred, int joe)>> this[int i]",
            "this",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.this",
            636, 10,
            642, 9,
            631, 9,
            636, 10)]
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

        #endregion

        #region events

        [Theory]
        [InlineData(
            "event",
            "public event MouseEventHandler MouseUp",
            "public event MouseEventHandler MouseUp",
            "MouseUp",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.MouseUp",
            645, 10,
            648, 9,
            644, 9,
            645, 10)]
        [InlineData(
            "event",
            "public   \r\n             event   \r\n              MouseEventHandler   \r\n            MouseDown",
            "public event MouseEventHandler MouseDown",
            "MouseDown",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.MouseDown",
            656, 10,
            659, 9,
            650, 9,
            656, 10)]
        public void EventMatches(
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

        #endregion

        #region operators

        [Theory]
        [InlineData(
            "operator",
            "public static implicit operator double(Vector v)",
            "public static implicit operator double(Vector v)",
            "double",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.double",
            662, 10,
            664, 9,
            661, 9,
            662, 10)]
        [InlineData(
            "operator",
            "public static implicit operator AuthorDto(Author author)",
            "public static implicit operator AuthorDto(Author author)",
            "AuthorDto",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.AuthorDto",
            667, 10,
            673, 9,
            666, 9,
            667, 10)]
        [InlineData(
            "operator",
            "public static explicit operator AuthorDto(Author author)",
            "public static explicit operator AuthorDto(Author author)",
            "AuthorDto",
            "TreeSolve.CompositeFlows.Export.JsonParsers.JsonEdgesParser2.AuthorDto",
            676, 10,
            682, 9,
            675, 9,
            676, 10)]

        [InlineData(
            "operator",
            "public static Fraction operator +(Fraction a)",
            "public static Fraction operator +(Fraction a)",
            "+",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.+",
            793, 57,
            793, 59,
            793, 9,
            793, 57)]
        [InlineData(
            "operator",
            "public static Fraction operator -(Fraction a)",
            "public static Fraction operator -(Fraction a)",
            "-",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.-",
            794, 57,
            794, 85,
            794, 9,
            794, 57)]
        [InlineData(
            "operator",
            "public static Fraction operator +(Fraction a, Fraction b)",
            "public static Fraction operator +(Fraction a, Fraction b)",
            "+",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.+",
            797, 15,
            797, 74,
            796, 9,
            797, 15)]
        [InlineData(
            "operator",
            "public static Fraction operator - ( Fraction a, Fraction b )",
            "public static Fraction operator -(Fraction a, Fraction b)",
            "-",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.-",
            800, 15,
            800, 24,
            799, 9,
            800, 15)]
        [InlineData(
            "operator",
            "public static \r\n            Fraction \r\n            operator *\r\n            (\r\n            Fraction a, \r\n            Fraction b\r\n            )",
            "public static Fraction operator *(Fraction a, Fraction b)",
            "*",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.*",
            808, 17,
            808, 60,
            802, 9,
            808, 17)]
        [InlineData(
            "operator",
            "public static Fraction operator /(Fraction a, Fraction b)",
            "public static Fraction operator /(Fraction a, Fraction b)",
            "/",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction./",
            811, 10,
            817, 9,
            810, 9,
            811, 10)]

        [InlineData(
            "operator",
            "public static implicit operator double(Vector v)",
            "public static implicit operator double(Vector v)",
            "double",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.double",
            822, 10,
            824, 9,
            821, 9,
            822, 10)]
        [InlineData(
            "operator",
            "public static implicit operator AuthorDto(Author author)",
            "public static implicit operator AuthorDto(Author author)",
            "AuthorDto",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.AuthorDto",
            827, 10,
            833, 9,
            826, 9,
            827, 10)]
        [InlineData(
            "operator",
            "public static explicit operator AuthorDto(Author author)",
            "public static explicit operator AuthorDto(Author author)",
            "AuthorDto",
            "TreeSolve.CompositeFlows.Export.JsonParsers.Fraction.AuthorDto",
            836, 10,
            842, 9,
            835, 9,
            836, 10)]
        public void OperatorMatches(
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

        #endregion

        #region partial

        [Theory]
        [InlineData(
            "interface",
            "internal interface IFred",
            "TreeSolve.CompositeFlows.Export2.IFred",
            false)]
        [InlineData(
            "interface",
            "internal partial interface IBert",
            "TreeSolve.CompositeFlows.Export2.IBert",
            true)]
        [InlineData(
            "class",
            "partial class BaseNode",
            "TreeSolve.CompositeFlows.Export2.BaseNode",
            true)]
        [InlineData(
            "class",
            "class BaseNodeGeneric<T>",
            "TreeSolve.CompositeFlows.Export2.BaseNodeGeneric<T>",
            false)]
        [InlineData(
            "class",
            "partial class NodeConcrete<T>",
            "TreeSolve.CompositeFlows.Export2.NodeConcrete<T>",
            true)]
        [InlineData(
            "class",
            "class NodeClosed<T>",
            "TreeSolve.CompositeFlows.Export2.NodeClosed<T>",
            false)]
        [InlineData(
            "class",
            "partial class NodeOpen<T>",
            "TreeSolve.CompositeFlows.Export2.NodeOpen<T>",
            true)]
        [InlineData(
            "class",
            "class BaseNodeMultiple<T, U>",
            "TreeSolve.CompositeFlows.Export2.BaseNodeMultiple<T, U>",
            false)]
        [InlineData(
            "class",
            "partial class Node4<T>",
            "TreeSolve.CompositeFlows.Export2.Node4<T>",
            true)]
        [InlineData(
            "class",
            "class Node5<T, U>",
            "TreeSolve.CompositeFlows.Export2.Node5<T, U>",
            false)]
        [InlineData(
            "class",
            "partial class NodeItem<T>",
            "TreeSolve.CompositeFlows.Export2.NodeItem<T>",
            true)]
        [InlineData(
            "class",
            "class SpecialNodeItem<T>",
            "TreeSolve.CompositeFlows.Export2.SpecialNodeItem<T>",
            false)]
        [InlineData(
            "class",
            "partial class SuperKeyType<K, V, U>",
            "TreeSolve.CompositeFlows.Export2.SuperKeyType<K, V, U>",
            true)]
        [InlineData(
            "method",
            "internal partial Task<string> TestNested()",
            "TreeSolve.CompositeFlows.Export2.Nested.Two.ParentConcrete<T>.NestedConcrete.TestNested",
            true)]
        public void PartialMatches(
            string componentType,
            string cleanedSignature,
            string fullName,
            bool isPartial)
        {
            BlockStats? block = GetBlockByCleanedSignature(
                cleanedSignature,
                fullName
            );

            Assert.Equal(componentType, block?.Type);
            Assert.Equal(isPartial, block?.Flags.Contains("partial"));
        }

        #endregion

        #region open close indices

        [Fact]
        public void OpenAndCloseBlockMatches()
        {
            Assert.NotNull(_fixture.BlockStatsCache);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (BlockStats block in _fixture.BlockStatsCache.BlockStats)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                string open = _fixture.Code.Substring(block.AdjustedOpenIndex - block.Settings.Open.Length, block.Settings.Open.Length);
                string close = _fixture.Code.Substring(block.AdjustedCloseIndex, block.Settings.Close.Length);

                Assert.Equal(block.Settings.Open, open);

                if (block.Settings.ExplicitClose)
                {
                    Assert.Equal(block.Settings.Close, close);
                }

                Assert.True(block.CloseIndex >= block.OpenIndex);
                Assert.True(block.AdjustedCloseIndex >= block.AdjustedOpenIndex);
            }
        }
        #endregion

        #region blocks

        [Theory]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            " double HighTemp , double LowTemp ",
            1, 33,
            1, 67)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n        public double Mean => (HighTemp + LowTemp) / 2.0;\r\n\r\n        public double Add(int 20)\r\n        {\r\n            return new DailyTemperature(HighTemp + 20, LowTemp);\r\n        }\r\n    ",
            2, 2,
            9, 1)]
        [InlineData(
            "Code",
            "Lambda",
            "=>",
            ";",
            " (HighTemp + LowTemp) / 2.0",
            3, 26,
            3, 53)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "HighTemp + LowTemp",
            3, 28,
            3, 46)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "int 20",
            5, 23,
            5, 29)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n            return new DailyTemperature(HighTemp + 20, LowTemp);\r\n        ",
            6, 6,
            8, 5)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "HighTemp + 20, LowTemp",
            7, 37,
            7, 59)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "double BaseTemperature, IEnumerable<DailyTemperature> TempRecords = 3",
            11, 35,
            11, 104)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "double BaseTemperature, IEnumerable<DailyTemperature> TempRecords",
            13, 40,
            13, 105)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "BaseTemperature, TempRecords",
            14, 18,
            14, 46)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n        public double DegreeDays => TempRecords.Where(s => { return s.Mean < BaseTemperature; }).Sum(s => BaseTemperature - s.Mean);\r\n    ", 15, 2,
            17, 1)]
        [InlineData(
            "Code",
            "Lambda",
            "=>",
            ";",
            " TempRecords.Where(s => { return s.Mean < BaseTemperature; }).Sum(s => BaseTemperature - s.Mean)",
            16, 32,
            16, 128)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "s => { return s.Mean < BaseTemperature; }",
            16, 51,
            16, 92)]
        [InlineData(
            "Code",
            "Lambda",
            "=>",
            ";",
            " { return s.Mean < BaseTemperature; }",
            16, 55,
            16, 92)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            " return s.Mean < BaseTemperature; ",
            16, 57,
            16, 91)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "s => BaseTemperature - s.Mean",
            16, 98,
            16, 127)]
        [InlineData(
            "Code",
            "Lambda",
            "=>",
            ";",
            " BaseTemperature - s.Mean",
            16, 102,
            16, 127)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "\r\n        double BaseTemperature, \r\n        IEnumerable < DailyTemperature > TempRecords\r\n        ",
            20, 6,
            23, 5)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            " BaseTemperature, \r\n            TempRecords ",
            24, 19,
            25, 21)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n        public double DegreeDays\r\n        {\r\n            get => TempRecords.Where(s => s.Mean > BaseTemperature).Sum(s => s.Mean - BaseTemperature);\r\n        }\r\n    ",
            26, 2,
            31, 1)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n            get => TempRecords.Where(s => s.Mean > BaseTemperature).Sum(s => s.Mean - BaseTemperature);\r\n        ",
            28, 6,
            30, 5)]
        [InlineData(
            "Code",
            "Lambda",
            "=>",
            ";",
            " TempRecords.Where(s => s.Mean > BaseTemperature).Sum(s => s.Mean - BaseTemperature)",
            29, 15,
            29, 99)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "s => s.Mean > BaseTemperature",
            29, 34,
            29, 63)]
        [InlineData(
            "Code",
            "Lambda",
            "=>",
            ";",
            " s.Mean > BaseTemperature",
            29, 38,
            29, 63)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "s => s.Mean - BaseTemperature",
            29, 69,
            29, 98)]
        [InlineData(
            "Code",
            "Lambda",
            "=>",
            ";",
            " s.Mean - BaseTemperature",
            29, 73,
            29, 98)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n    /// <summary>\r\n    /// Retrieve or manipulate individual nodes\r\n    /// </summary>\r\n    public class PreProcessorTest\r\n    {\r\n        #pragma warning disable CA1056 // Uri properties should not be strings\r\n        public string Url { get; set; }\r\n#pragma warning restore CA1056 // Uri properties should not be strings\r\n        /// <summary>\r\n        /// Test\r\n        /// </summary>\r\n        /// <param name=\"name\">Name</param>\r\n        public void method1(string name)\r\n        {\r\n            Func<(string, bool)> fred = () =>\r\n                {\r\n                    string h = \"\";\r\n                    bool intime = true;\r\n\r\n                    return (h, intime);\r\n                };\r\n\r\n#if DEBUG\r\n            Console.WriteLine($\"{DateTime.Now.ToString(\"yyyy/MM/dd_HH:mm:ss::fff\")} - ProcessID = {_processState.ID}\");\r\n#endif\r\n        }\r\n    }\r\n\r\n\r\n",
            200, -2,
            230, -3)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n        #pragma warning disable CA1056 // Uri properties should not be strings\r\n        public string Url { get; set; }\r\n#pragma warning restore CA1056 // Uri properties should not be strings\r\n        /// <summary>\r\n        /// Test\r\n        /// </summary>\r\n        /// <param name=\"name\">Name</param>\r\n        public void method1(string name)\r\n        {\r\n            Func<(string, bool)> fred = () =>\r\n                {\r\n                    string h = \"\";\r\n                    bool intime = true;\r\n\r\n                    return (h, intime);\r\n                };\r\n\r\n#if DEBUG\r\n            Console.WriteLine($\"{DateTime.Now.ToString(\"yyyy/MM/dd_HH:mm:ss::fff\")} - ProcessID = {_processState.ID}\");\r\n#endif\r\n        }\r\n    ",
            205, 2,
            227, 1)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            " get; set; ",
            207, 24,
            207, 35)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "string name",
            213, 25,
            213, 36)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n            Func<(string, bool)> fred = () =>\r\n                {\r\n                    string h = \"\";\r\n                    bool intime = true;\r\n\r\n                    return (h, intime);\r\n                };\r\n\r\n#if DEBUG\r\n            Console.WriteLine($\"{DateTime.Now.ToString(\"yyyy/MM/dd_HH:mm:ss::fff\")} - ProcessID = {_processState.ID}\");\r\n#endif\r\n        ",
            214, 6,
            226, 5)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "string, bool",
            215, 15,
            215, 27)]
        [InlineData(
            "Parameter",
            "Round",
            "(",
            ")",
            "",
            215, 38,
            215, 38)]
        [InlineData(
            "Code",
            "Lambda",
            "=>",
            ";",
            "\r\n                {\r\n                    string h = \"\";\r\n                    bool intime = true;\r\n\r\n                    return (h, intime);\r\n                }",
            215, 42,
            221, 14)]
        [InlineData(
            "Code",
            "Curly",
            "{",
            "}",
            "\r\n                    string h = \"\";\r\n                    bool intime = true;\r\n\r\n                    return (h, intime);\r\n                ",
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
            Assert.NotNull(_fixture.BlockStatsCache);

            // Adjust for this file
            blockStartLine += 874;
            blockEndLine += 874;
            blockStartColumn += 4;
            blockEndColumn += 4;

            bool found = false;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (BlockStats block in _fixture.BlockStatsCache.BlockStats)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                if (block.BlockStartLocation.Line == blockStartLine
                    && block.BlockStartLocation.Column == blockStartColumn
                    && block.Settings.Model == "Block")
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

        #endregion

        #region strings

        [Theory]
        [InlineData(
            "method",
            "string Method1()",
            "StringTest.Stripping.Method1",
            " \"\"")]
        [InlineData(
            "method",
            "string Method2()",
            "StringTest.Stripping.Method2",
            " \"\"")]
        [InlineData(
            "method",
            "string Method3()",
            "StringTest.Stripping.Method3",
            " \"\"")]
        [InlineData(
            "method",
            "string Method4()",
            "StringTest.Stripping.Method4",
            " \"\"")]
        [InlineData(
            "method",
            "string Method5()",
            "StringTest.Stripping.Method5",
            " @\"\"")]
        [InlineData(
            "method",
            "string Method6()",
            "StringTest.Stripping.Method6",
            " @\"\"")]
        [InlineData(
            "method",
            "string Method7()",
            "StringTest.Stripping.Method7",
            " @\"\" + \"\"")]
        [InlineData(
            "method",
            "string Method8()",
            "StringTest.Stripping.Method8",
            " @\"\"")]
        [InlineData(
            "method",
            "string Method9()",
            "StringTest.Stripping.Method9",
            " @\"\"")]
        [InlineData(
            "method",
            "string Method10()",
            "StringTest.Stripping.Method10",
            " @\"\"")]
        [InlineData(
            "method",
            "string Method11()",
            "StringTest.Stripping.Method11",
            " $\"{DateTime.Now.ToString()}\"")]
        [InlineData(
            "method",
            "string Method12()",
            "StringTest.Stripping.Method12",
            " @$\"{DateTime.Now.ToString()}\"")]
        [InlineData(
            "method",
            "string Method13()",
            "StringTest.Stripping.Method13",
            " @$\"{DateTime.Now.ToString()}\"")]
        [InlineData(
            "method",
            "string Method14()",
            "StringTest.Stripping.Method14",
            " $@\"{DateTime.Now.ToString()}\"")]
        [InlineData(
            "method",
            "string Method15()",
            "StringTest.Stripping.Method15",
            " $\"{DateTime.Now.ToString(\"\")}\"")]
        [InlineData(
            "method",
            "string Method16()",
            "StringTest.Stripping.Method16",
            " $\"{DateTime.Now.ToString(\"\")}\"")]
        [InlineData(
            "method",
            "string Method17()",
            "StringTest.Stripping.Method17",
            " $\"{DateTime.Now.ToString(\"\")}\"")]
        public void StringMatches(
            string componentType,
            string signature,
            string fullName,
            string content)
        {
            BlockStats? block = GetBlockBySignature(
                signature,
                fullName,
                componentType
            );

            Assert.Equal(content, block?.StrippedContent);
        }
        #endregion

        #region content

        [Fact]
        public void StringContentMatches()
        {
            Assert.NotNull(_fixture.DirectoryPath);

#pragma warning disable CS8604 // Possible null reference argument.
            string path = Path.Combine(_fixture.DirectoryPath, "Files\\CSharp1StringContent.cs");
#pragma warning restore CS8604 // Possible null reference argument.

            string stringContent = File.ReadAllText(path);

            Assert.Equal(stringContent, _fixture.CodeContainer.StringContent.NormaliseLineEndings());
        }

        [Fact]
        public void PreProcessorContentMatches()
        {
            Assert.NotNull(_fixture.DirectoryPath);

#pragma warning disable CS8604 // Possible null reference argument.
            string path = Path.Combine(_fixture.DirectoryPath, "Files\\CSharp1PreProcessorContent.cs");
#pragma warning restore CS8604 // Possible null reference argument.

            string preProcessorContent = File.ReadAllText(path);

            Assert.Equal(preProcessorContent, _fixture.CodeContainer.PreProcessorContent.NormaliseLineEndings());
        }

        [Fact]
        public void CodeContentMatches()
        {
            Assert.NotNull(_fixture.DirectoryPath);

#pragma warning disable CS8604 // Possible null reference argument.
            string path = Path.Combine(_fixture.DirectoryPath, "Files\\CSharp1CodeContent.cs");
#pragma warning restore CS8604 // Possible null reference argument.

            string codeContent = File.ReadAllText(path);

            Assert.Equal(codeContent, _fixture.CodeContainer.CodeContent.NormaliseLineEndings());
        }

        [Fact]
        public void CommentContentMatches()
        {
            Assert.NotNull(_fixture.DirectoryPath);

#pragma warning disable CS8604 // Possible null reference argument.
            string path = Path.Combine(_fixture.DirectoryPath, "Files\\CSharp1CommentContent.cs");
#pragma warning restore CS8604 // Possible null reference argument.

            string commentContent = File.ReadAllText(path);

            Assert.Equal(commentContent, _fixture.CodeContainer.CommentContent.NormaliseLineEndings());
        }
        #endregion

        private BlockStats? GetBlockBySignature(
            string signature,
            string fullName,
            string type)
        {
            Assert.NotNull(_fixture.BlockStatsCache);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (BlockStats block in _fixture.BlockStatsCache.BlockStats)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
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

        private BlockStats? FindDescendantBlock(
            BlockStats block,
            int blockStartLine,
            int blockStartColumn,
            int blockEndLine,
            int blockEndColumn)
        {
            BlockStats? found;

            foreach (BlockStats child in block.ChildBlocks)
            {
                if (child.BlockStartLocation.Line == blockStartLine
                    && child.BlockStartLocation.Column == blockStartColumn
                    && child.BlockEndLocation.Line == blockEndLine
                    && child.BlockEndLocation.Column == blockEndColumn)
                {
                    return child;
                }

                found = FindDescendantBlock(
                    child,
                    blockStartLine,
                    blockStartColumn,
                    blockEndLine,
                    blockEndColumn
                );

                if (found is { })
                {
                    return found;
                }
            }

            return null;
        }

        private BlockStats? GetBlockByCleanedSignature(
            string cleanedSignature,
            string fullName)
        {
            Assert.NotNull(_fixture.BlockStatsCache);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (BlockStats block in _fixture.BlockStatsCache.BlockStats)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                if (String.Equals(block?.CleanedSignature, cleanedSignature, StringComparison.Ordinal)
                    && String.Equals(block?.FullName, fullName, StringComparison.Ordinal))
                {
                    return block;
                }
            }

            return null;
        }
    }
}
