namespace MuddySpud.RulesEngine
{
    public class GroupGangSettings
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Position { get; set; } = -1;

        public GroupGangSettings(
            string name,
            string type,
            int position)
        {
            Name = name;
            Type = type;
            Position = position;
        }
    }
}