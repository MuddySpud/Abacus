
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    public class GroupSettings
    {
        public string ID { get; } = string.Empty;
        public string Name { get; } = string.Empty;
        public string Type { get; } = string.Empty;
        public string Keyword { get; set; } = string.Empty;
        public List<string> Blocks { get; } = new();
        public List<string> SubBlocks { get; } = new();
        public GroupGangSettings? Gang { get; set; }
        public List<GroupUnitSettings> Units { get; } = new();

        public GroupSettings(
            string id,
            string name,
            string type,
            string keyword)
        {
            ID = id;
            Name = name;
            Type = type;
            Keyword = keyword;
        }
    }
}