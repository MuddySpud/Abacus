namespace MuddySpud.RulesEngine
{
    public class BlockBoundarySettings : IBoundarySettings
    {
        public string Name { get; } = string.Empty;
        public string BlockType { get; } = string.Empty;
        public string Open { get; } = string.Empty;
        public string Close { get; } = string.Empty;
        public string SearchType { get; } = string.Empty;
        public bool ExplicitClose { get; } = true;
        public string Model { get; } = "Block";

        public BlockBoundarySettings(
            string name,
            string type,
            string prefix,
            string suffix,
            string searchType,
            bool explicitClose = true)
        {
            Name = name;
            BlockType = type;
            Open = prefix;
            Close = suffix;
            SearchType = searchType;
            ExplicitClose = explicitClose;
        }
    }
}