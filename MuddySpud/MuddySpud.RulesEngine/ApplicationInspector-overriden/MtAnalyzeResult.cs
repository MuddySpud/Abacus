using Microsoft.ApplicationInspector.Commands;
using Microsoft.ApplicationInspector.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuddySpud.RulesEngine.Commands
{
    /// <summary>
    /// Result of Analyze command GetResult() operation
    /// </summary>
    public class MtAnalyzeResult : Result
    {
        public enum ExitCode
        {
            Success = 0,
            NoMatches = 1,
            CriticalError = Utils.ExitCode.CriticalError, //ensure common value for final exit log mention
            Canceled = 3,
            TimedOut = 4
        }

        [JsonProperty(Order = 2, PropertyName = "resultCode")]
        public ExitCode ResultCode { get; set; }

        /// <summary>
        /// Analyze command result object containing scan properties
        /// </summary>
        [JsonProperty(Order = 3, PropertyName = "metaData")]
        public MtMetaData Metadata { get; set; }

        public MtAnalyzeResult()
        {
            Metadata = new("", "");//needed for serialization for other commands; replaced later
        }
    }
}
