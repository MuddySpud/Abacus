

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace TreeSolve.CompositeFlows.Export.JsonParsers
{
    public class JsonEdgesParser : IJsonEdgesParser
    {
        internal async Task<string> GetNodeOptionsAndParent2(KeyGuid keyGuid)
        {
            string test1 = "Fred was here";
            string test2 = "Fred was \"NOT here";
            string test3 = "Fred was NOT\" here";
            string test4 = "Fred was \"NOT\" here";

            string test5 = @"Fred
was
here ""LATE""";

            string test6 = @"Fred was here";
            string test7 = @"Fred was here" + " today";
            string test8 = @"Fred was ""NOT\ here";
            string test9 = @"Fred was NOT"" here";
            string test10 = @"Fred was ""NOT"" here";
            string test11 = $"Fred was here: {DateTime.Now.ToString()} 2 \"hours\" late";
            string test12 = @$"Fred was here: {DateTime.Now.ToString()} 2 hours "" late";
            string test13 = @$"Fred was here: {DateTime.Now.ToString()} 2 ""hours "" late";
            string test14 = $@"Fred was here: {DateTime.Now.ToString()} 2 ""hours "" late";
            string test15 = $"Fred was here: {DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")} 2 hours late";
            string test16 = $"Fred was here: {DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")}";
            string test17 = $"{DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")} late";

            string test18 = $"Fred was here: { String.IsNullOrWhiteSpace(test1) }";



            return await _compositeFlowsClient365
                .PostJsonAsync(query)
                .ConfigureAwait(false);



    }


}



}


// 8 test inline comment
