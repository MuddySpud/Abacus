
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    public class FacetSettings
    {
        public string Name { get; }
        public string BlockType { get; } = string.Empty;
        public List<PatternSettings> Patterns { get; } = new();

        public FacetSettings(
            string name,
            string blockType)
        {
            Name = name;
            BlockType = blockType;
        }
    }
}