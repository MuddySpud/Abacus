using System.Collections.Generic;

namespace MuddySpud.Metrics.Engine
{
    public class ClassMetricsRecord
    {
        public string ClassName { get; set; }
        public List<TagCounter> TagCounts { get; } = new List<TagCounter>();

        public ClassMetricsRecord(string className)
        {
            ClassName = className;
        }
    }
}
