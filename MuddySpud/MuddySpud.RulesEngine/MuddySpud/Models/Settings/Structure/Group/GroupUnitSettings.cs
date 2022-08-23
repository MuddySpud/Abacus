using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    public class GroupUnitSettings
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string SearchType { get; set; } = string.Empty;
        public List<PatternSettings> Patterns { get; } = new();

        public GroupUnitSettings(
            string name,
            string type,
            string searchType)
        {
            Name = name;
            Type = type;
            SearchType = searchType;
        }
    }
}