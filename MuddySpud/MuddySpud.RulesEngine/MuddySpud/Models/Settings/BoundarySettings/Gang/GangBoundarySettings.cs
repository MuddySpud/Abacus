namespace MuddySpud.RulesEngine
{
    public class GangBoundarySettings : IBoundarySettings
    {
        public string Name { get; } = string.Empty;
        public string BlockType { get; } = "Code";
        public string Close { get; } = string.Empty;
        public string SearchType { get; } = string.Empty;
        public bool ExplicitClose => true;
        public string Open => string.Empty;
        public string Model { get; } = "Gang";

        public GangBoundarySettings(string name)
        {
            Name = name;
        }
    }
}