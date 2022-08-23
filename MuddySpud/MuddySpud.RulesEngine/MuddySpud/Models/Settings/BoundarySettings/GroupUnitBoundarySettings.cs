namespace MuddySpud.RulesEngine
{
    public class GroupUnitBoundarySettings : IBoundarySettings
    {
        public string Name { get; } = string.Empty;
        public string BlockType { get; } = "Code";
        public string Close { get; } = string.Empty;
        public string SearchType { get; } = string.Empty;
        public bool ExplicitClose => true;
        public string Open => string.Empty;
        public string Model { get; } = "GroupUnit";

        public GroupUnitBoundarySettings(string name)
        {
            Name = name;
        }
    }
}