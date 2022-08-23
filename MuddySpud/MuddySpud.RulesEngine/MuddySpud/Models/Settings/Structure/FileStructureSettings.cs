
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    public class FileStructureSettings
    {
        public string Type { get; } = string.Empty;
        public string NameJoiner { get; } = string.Empty;
        public string Language { get; } = string.Empty;
        public List<ComponentSettings> Components { get; } = new();
        public List<GroupSettings> Groups { get; } = new();
        public List<UnMaskSettings> UnMasking { get; } = new();
        public UnitOfWorkSettings UnitsOfWork { get; } = new();

        public FileStructureSettings(
            string language,
            string type,
            string nameJoiner)
        {
            Language = language;
            Type = type;
            NameJoiner = nameJoiner;
        }
    }
}