namespace MuddySpud.RulesEngine
{
    public class UnMaskBoundarySettings : IBoundarySettings
    {
        public string Name { get; } = string.Empty;
        public string BlockType { get; } = string.Empty;
        public string Close { get; } = string.Empty;
        public string SearchType { get; } = string.Empty;
        public bool ExplicitClose => true;
        public string Open => string.Empty;
        public string Model { get; } = "UnMask";

        public UnMaskBoundarySettings(string blockType)
        {
            BlockType = blockType;
        }
    }
}