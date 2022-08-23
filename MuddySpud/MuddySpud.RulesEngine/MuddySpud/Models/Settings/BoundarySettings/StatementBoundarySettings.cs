namespace MuddySpud.RulesEngine
{
    public class StatementBoundarySettings: IBoundarySettings
    {
        public string Name { get; } = string.Empty;
        public string BlockType { get; } = string.Empty;
        public string Close { get; } = string.Empty;
        public string SearchType { get; } = string.Empty;
        public bool ExplicitClose => true;
        public string Open => string.Empty;
        public string Model { get; } = "Statement";

        public StatementBoundarySettings(
            string name,
            string type,
            string suffix,
            string searchType)
        {
            Name = name;
            BlockType = type;
            Close = suffix;
            SearchType = searchType;
        }
    }
}