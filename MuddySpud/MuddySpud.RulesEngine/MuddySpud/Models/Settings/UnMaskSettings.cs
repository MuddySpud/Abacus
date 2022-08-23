namespace MuddySpud.RulesEngine
{
    public class UnMaskSettings
    {
        public string GroupName { get; }
        public string ActionName { get; } = string.Empty;

        public UnMaskSettings(
            string groupName,
            string actionName)
        {
            GroupName = groupName;
            ActionName = actionName;
        }
    }
}