using System.Text.RegularExpressions;

namespace MuddySpud.RulesEngine.Models
{
    public class GroupSearchMatch
    {
        public int MatchIndex { get; init; }
        public string BlockType { get; init; }
        public GroupSettings GroupSettings { get; init; }
        public Match Match { get; init; }
        public BlockStats? GangBlock { get; set; } = null;

        public GroupSearchMatch(
            int matchIndex,
            string blockType,
            GroupSettings groupSettings,
            Match match)
        {
            MatchIndex = matchIndex;
            BlockType = blockType;
            GroupSettings = groupSettings;
            Match = match;
        }
    }
}
