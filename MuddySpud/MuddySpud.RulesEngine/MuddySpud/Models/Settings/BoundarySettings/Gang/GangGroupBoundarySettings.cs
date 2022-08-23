namespace MuddySpud.RulesEngine
{
    public class GangGroupBoundarySettings : IBoundarySettings
    {
        public string Name { get; } = string.Empty;
        public string BlockType { get; } = "Code";
        public string Close { get; } = string.Empty;
        public string SearchType { get; } = string.Empty;
        public bool ExplicitClose => true;
        public string Open => string.Empty;
        public string Model { get; } = "GangGroup";
        public string GangName { get; } = string.Empty;
        public string Gang { get; } = string.Empty;
        public int Position { get; } = -1;

        public GangGroupBoundarySettings(
            string gangName,
            string gang,
            int position)
        {
            GangName = gangName;
            Gang = gang;
            Position = position;
        }
    }
}