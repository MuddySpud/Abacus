using System.Diagnostics;

namespace MuddySpud.RulesEngine
{
    [DebuggerDisplay("{Type}")]
    public class BlockStatsError
    {
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public BlockStatsError(
            string type,
            string message)
        {
            Type = type;
            Message = message;
        }
    }
}