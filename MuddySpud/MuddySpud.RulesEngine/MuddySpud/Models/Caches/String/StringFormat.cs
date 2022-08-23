namespace MuddySpud.Metrics.Engine
{
    public class StringFormat
    {
        public bool Enabled { get; }
        public bool FormatStarted { get; set; } = false;

        public StringFormat(bool enabled)
        {
            Enabled = enabled;
        }
    }
}
