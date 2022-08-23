using Microsoft.ApplicationInspector.RulesEngine;

namespace MuddySpud.RulesEngine
{
    public interface IBoundaryCounter
    {
        int GetFullIndexFromCodeIndex(int codeIndex);
        Location GetLocation(int index);
    }
}