using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    public class UnitOfWorkSettings
    {
        public List<PatternSettings> NonBlockPatterns { get; set; } = new();

        public UnitOfWorkSettings()
        {
        }
    }
}