using MuddySpud.RulesEngine;

namespace MuddySpud.Metrics.Engine
{
    public class StringState
    {
        public StringEscape Escape { get; }
        public StringFormat Format { get; }
        public StringState? Parent { get; }
        public bool StringStarted { get; set; }
        public bool MultiLine { get; }
        public int StartIndex { get; set; }

        public StringState(
            StringEscape escape,
            bool formatEnabled,
            StringState? parent,
            int startIndex,
            bool multiLine)
        {
            Escape = escape;
            Format = new StringFormat(formatEnabled);
            Parent = parent;
            StringStarted = true;
            StartIndex = startIndex;
            MultiLine = multiLine;
        }
    }
}
