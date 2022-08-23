
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    public class ComponentSettings
    {
        public string ID { get; } = string.Empty;
        public string Name { get; } = string.Empty;
        public string BlockType { get; } = string.Empty;
        public bool PrintMetrics { get; } = false;
        public string Description { get; set; } = string.Empty;
        public bool QualifiesName { get; set; } = false;
        public List<string> Parents { get; } = new();
        public List<UnitSettings> Units { get; } = new();
        public List<PatternSettings> Patterns { get; } = new();

        public ComponentSettings(
            string id,
            string name,
            string blockType,
            bool? printMetrics = false)
        {
            ID = id;
            Name = name;
            BlockType = blockType;

            if (printMetrics == true)
            {
                PrintMetrics = true;
            }
        }
    }
}