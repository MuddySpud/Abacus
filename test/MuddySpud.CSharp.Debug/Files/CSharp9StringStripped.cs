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
            string test1 = "";
            string test2 = "";
            string test3 = "";
            string test4 = "";
            string test5 = @"";
            string test6 = @"";
            string test7 = @"" + "";
            string test8 = @"";
            string test9 = @"";
            string test10 = @"";
            string test11 = $"{DateTime.Now.ToString()}";
            string test12 = @$"{DateTime.Now.ToString()}";
            string test13 = @$"{DateTime.Now.ToString()}";
            string test14 = $@"{DateTime.Now.ToString()}";
            string test15 = $"{DateTime.Now.ToString("")}";
            string test16 = $"{DateTime.Now.ToString("")}";
            string test17 = $"{DateTime.Now.ToString("")}";
            string test18 = $"{ String.IsNullOrWhiteSpace(test1) }";
            return await _compositeFlowsClient365
                .PostJsonAsync(query)
                .ConfigureAwait(false);
    }
}
}
