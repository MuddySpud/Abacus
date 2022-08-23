using Microsoft.ApplicationInspector.Commands;
using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class AnalyseFile_Structure
    {
        public MtAnalyzeResult? _analyzeResult = null;
        public AnalyseFile_Structure()
        {
            MtAnalyzeCommand command = new MtAnalyzeCommand(new MtAnalyzeOptions()
            {
                SourcePath = new String[] { "C:\\GitHub\\Abacus\\test\\MuddySpud.CSharp.Tests\\Files" },
                CustomRulesPath = "C:\\GitHub\\Abacus\\MuddySpud\\MuddySpud.Abacus\\CodemologieRules\\",
                IgnoreDefaultRules = true,
                ConfidenceFilters = "high,medium",
                FilePathExclusions = new String[] { "**/bin/**", "**/obj/**", "**/.vs/**", "**/.git/**", "*.json" },
                ConsoleVerbosityLevel = "medium",
                //Log = cliOptions.Log,
                SingleThread = true,
                NoShowProgress = false,
                FileTimeOut = 60000,
                ProcessingTimeOut = 0,
                ContextLines = 3,
                ScanUnknownTypes = false,
                TagsOnly = false,
                NoFileMetadata = false
            }); ;

            _analyzeResult = command.GetResult();
        }

        [Fact]
        public void CheckCSharp2StructureCounts()
        {
            Assert.Equal(MtAnalyzeResult.ExitCode.Success, _analyzeResult?.ResultCode);

            List<AbacusRecord>? abacusRecords = _analyzeResult?.Metadata.AbacusRecords;
            Assert.Equal(9, abacusRecords?.Count);

            AbacusRecord? abacusRecord = GetFileMetrics(
                abacusRecords,
                "CSharp2.cs");

            Assert.NotNull(abacusRecord);

            MetricsBlock? file = abacusRecord?.Metrics.Structure;
            Assert.NotNull(file);
            Assert.Equal("File", file?.FullName);
            Assert.Equal(1, file?.ChildBlocks.Count);
            RunFileTests(file);

            MetricsBlock? nameSpace = file?.ChildBlocks[0];
            Assert.NotNull(nameSpace);
            Assert.Equal("namespace MuddySpud.Metrics.Engine", nameSpace?.Signature);
            Assert.Equal(1, nameSpace?.ChildBlocks.Count);
            RunNamespaceTests(nameSpace);

            MetricsBlock? cSharp2 = nameSpace?.ChildBlocks[0];
            Assert.NotNull(cSharp2);
            Assert.Equal("public static class CSharp2", cSharp2?.Signature);
            Assert.Equal(2, cSharp2?.ChildBlocks.Count);
            RunClassTests(cSharp2);

            MetricsBlock? mergeTagCounts = cSharp2?.ChildBlocks[0];
            Assert.NotNull(mergeTagCounts);
            Assert.Equal("public static void MergeTagCounts(this List<TagCounter> a, List<TagCounter> b)", mergeTagCounts?.Signature);
            Assert.Equal(0, mergeTagCounts?.ChildBlocks.Count);
            Assert.Equal(1, mergeTagCounts?.Block?.ChildBlocks.Count);
            RunMergeTagCountsTests(mergeTagCounts);

            MetricsBlock? incrementTagCount = cSharp2?.ChildBlocks[1];
            Assert.NotNull(incrementTagCount);
            Assert.Equal("public static void IncrementTagCount(this List<TagCounter> tagCounts, string tag, int count = 1)", incrementTagCount?.Signature);
            Assert.Equal(0, incrementTagCount?.ChildBlocks.Count);
            Assert.Equal(4, incrementTagCount?.Block?.ChildBlocks.Count);
            RunIncrementTagCountTests(incrementTagCount);
        }

        private void RunFileTests(MetricsBlock? file)
        {
            Assert.Equal(15, file?.TagCounts.Count);

            Assert.Equal(3, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.using"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.namespace"));
            Assert.Equal(3, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.public"));
            Assert.Equal(3, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.static"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.class"));
            Assert.Equal(2, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.void"));
            Assert.Equal(2, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.this"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.string"));
            Assert.Equal(2, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.int"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.foreach"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.in"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.for"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.if"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.return"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.new"));
            Assert.Equal(1, GetTagCount(file?.TagCounts, "C-m.Code.CSharp.string"));
        }

        private void RunNamespaceTests(MetricsBlock? nameSpace)
        {
            Assert.Equal(14, nameSpace?.TagCounts.Count);

            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.namespace"));
            Assert.Equal(3, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.public"));
            Assert.Equal(3, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.static"));
            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.class"));
            Assert.Equal(2, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.void"));
            Assert.Equal(2, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.this"));
            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.string"));
            Assert.Equal(2, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.int"));
            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.foreach"));
            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.in"));
            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.for"));
            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.if"));
            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.return"));
            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.new"));
            Assert.Equal(1, GetTagCount(nameSpace?.TagCounts, "C-m.Code.CSharp.string"));
        }

        private void RunClassTests(MetricsBlock? cSharp2)
        {
            Assert.Equal(13, cSharp2?.TagCounts.Count);

            Assert.Equal(3, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.public"));
            Assert.Equal(3, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.static"));
            Assert.Equal(1, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.class"));
            Assert.Equal(2, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.void"));
            Assert.Equal(2, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.this"));
            Assert.Equal(1, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.string"));
            Assert.Equal(2, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.int"));
            Assert.Equal(1, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.foreach"));
            Assert.Equal(1, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.in"));
            Assert.Equal(1, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.for"));
            Assert.Equal(1, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.if"));
            Assert.Equal(1, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.return"));
            Assert.Equal(1, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.new"));
            Assert.Equal(1, GetTagCount(cSharp2?.TagCounts, "C-m.Code.CSharp.string"));
        }

        private void RunMergeTagCountsTests(MetricsBlock? mergeTagCounts)
        {
            Assert.Equal(6, mergeTagCounts?.TagCounts.Count);

            Assert.Equal(1, GetTagCount(mergeTagCounts?.TagCounts, "C-m.Code.CSharp.public"));
            Assert.Equal(1, GetTagCount(mergeTagCounts?.TagCounts, "C-m.Code.CSharp.static"));
            Assert.Equal(1, GetTagCount(mergeTagCounts?.TagCounts, "C-m.Code.CSharp.void"));
            Assert.Equal(1, GetTagCount(mergeTagCounts?.TagCounts, "C-m.Code.CSharp.this"));
            Assert.Equal(1, GetTagCount(mergeTagCounts?.TagCounts, "C-m.Code.CSharp.foreach"));
            Assert.Equal(1, GetTagCount(mergeTagCounts?.TagCounts, "C-m.Code.CSharp.in"));
        }

        private void RunIncrementTagCountTests(MetricsBlock? incrementTagCount)
        {
            Assert.Equal(10, incrementTagCount?.TagCounts.Count);

            Assert.Equal(1, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.public"));
            Assert.Equal(1, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.static"));
            Assert.Equal(1, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.void"));
            Assert.Equal(1, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.this"));
            Assert.Equal(2, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.int"));
            Assert.Equal(1, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.string"));
            Assert.Equal(1, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.for"));
            Assert.Equal(1, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.if"));
            Assert.Equal(1, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.return"));
            Assert.Equal(1, GetTagCount(incrementTagCount?.TagCounts, "C-m.Code.CSharp.new"));
        }

        private int GetTagCount(
            List<TagCounter>? tagCounters,
            string tag)
        {
            if (tagCounters is { })
            {
                foreach (TagCounter tagCounter in tagCounters)
                {
                    if (String.Equals(tagCounter.Tag, tag))
                    {
                        return tagCounter.Count;
                    }
                }
            }

            return 0;
        }

        private AbacusRecord? GetFileMetrics(
            List<AbacusRecord>? abacusRecords,
            string fileName)
        {
            if (abacusRecords is null
                || abacusRecords.Count == 0)
            {
                return null;
            }

            foreach (AbacusRecord abacusRecord in abacusRecords)
            {
                if (String.Equals(fileName, Path.GetFileName(abacusRecord.File.FileName)))
                {
                    return abacusRecord;
                }
            }

            return null;
        }
    }
}

