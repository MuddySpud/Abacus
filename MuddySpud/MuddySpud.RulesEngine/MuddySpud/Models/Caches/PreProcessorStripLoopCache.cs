using Microsoft.ApplicationInspector.RulesEngine;
using MuddySpud.RulesEngine;
using MuddySpud.RulesEngine.Commands;
using System.Collections.Generic;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public class PreProcessorStripLoopCache
    {
        public StringBuilder PreProcessorContent { get; }
        public int InputCounter { get; set; }
        public string[] PreProcessors { get; }
        public char[] PreProcessorFirstChars { get; }
        public OutputBoundaries OutputBoundaries { get; }

        public PreProcessorStripLoopCache(
            StringBuilder preProcessorContent,
            string[] preProcessors,
            OutputBoundaries outputBoundaries)
        {
            PreProcessorContent = preProcessorContent;
            PreProcessors = preProcessors;
            OutputBoundaries = outputBoundaries;

            List<char> preProcessorFirstChars = new();

            for (int i = 0; i < preProcessors.Length; i++)
            {
                if (!preProcessorFirstChars.Contains(preProcessors[i][0]))
                {
                    preProcessorFirstChars.Add(preProcessors[i][0]);
                }
            }

            PreProcessorFirstChars = preProcessorFirstChars.ToArray();
        }
    }
}
