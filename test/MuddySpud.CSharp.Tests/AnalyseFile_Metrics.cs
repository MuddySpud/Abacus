using Microsoft.ApplicationInspector.Commands;
using MuddySpud.Metrics.Engine;
using MuddySpud.RulesEngine.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MuddySpud.RegularExpression.Tests
{
    public class AnalyseFile_Metrics
    {
        public MtAnalyzeResult? _analyzeResult = null;
        public AnalyseFile_Metrics()
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
        public void CheckCSharp1MetricCounts()
        {
            Assert.Equal(MtAnalyzeResult.ExitCode.Success, _analyzeResult?.ResultCode);

            List<AbacusRecord>? abacusRecords = _analyzeResult?.Metadata.AbacusRecords;
            Assert.Equal(9, abacusRecords?.Count);

            AbacusRecord? abacusRecord = GetFileMetrics(
                abacusRecords,
                "CSharp1.cs");

            Assert.NotNull(abacusRecord);

            Assert.Equal(6, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.abstract"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.as"));
            Assert.Equal(2, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.base"));
            Assert.Equal(3, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.bool"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.break"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.byte"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.case"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.catch"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.char"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.checked"));
            Assert.Equal(35, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.class"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.const"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.continue"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.decimal"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.default"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.delegate"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.do"));
            Assert.Equal(13, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.double"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.else"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.enum"));
            Assert.Equal(2, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.event"));
            Assert.Equal(2, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.explicit"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.extern"));
            Assert.Equal(18, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.false"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.finally"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.fixed"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.float"));
            Assert.Equal(3, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.for"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.foreach"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.goto"));
            Assert.Equal(7, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.if"));
            Assert.Equal(4, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.implicit"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.in"));
            Assert.Equal(95, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.int"));
            Assert.Equal(5, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.interface"));
            Assert.Equal(17, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.internal"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.is"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.lock"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.long"));
            Assert.Equal(10, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.namespace"));
            Assert.Equal(54, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.new"));
            Assert.Equal(3, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.null"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.object"));
            Assert.Equal(12, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.operator"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.out"));
            Assert.Equal(3, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.override"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.params"));
            Assert.Equal(15, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.private"));
            Assert.Equal(1, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.protected"));
            Assert.Equal(81, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.public"));
            Assert.Equal(3, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.readonly"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.ref"));
            Assert.Equal(37, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.return"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.sbyte"));
            Assert.Equal(2, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.sealed"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.short"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.sizeof"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.stackalloc"));
            Assert.Equal(24, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.static"));
            Assert.Equal(135, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.string"));
            Assert.Equal(1, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.struct"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.switch"));
            Assert.Equal(14, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.this"));
            Assert.Equal(6, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.throw"));
            Assert.Equal(2, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.true"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.try"));
            Assert.Equal(1, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.typeof"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.uint"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.ulong"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.unchecked"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.unsafe"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.ushort"));
            Assert.Equal(5, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.using"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.virtual"));
            Assert.Equal(5, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.void"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.volatile"));
            Assert.Equal(0, GetTagCount(abacusRecord?.Metrics.Structure.TagCounts, "C-m.Code.CSharp.while"));
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

