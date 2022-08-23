namespace MuddySpud.RulesEngine
{
    public class GroupBoundarySettingsBuilder : IBoundarySettingsBuilder
    {
        public IBoundarySettings Build(GroupSettings settings)
        {
            return new GroupBoundarySettings(settings.Gang);
        }
    }
}
