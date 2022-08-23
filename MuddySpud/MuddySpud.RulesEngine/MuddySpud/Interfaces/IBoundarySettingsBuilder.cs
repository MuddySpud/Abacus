namespace MuddySpud.RulesEngine
{
    public interface IBoundarySettingsBuilder
    {
        IBoundarySettings Build(GroupSettings settings);
    }
}