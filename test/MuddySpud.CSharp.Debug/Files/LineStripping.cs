

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
            string token = keyGuid.Token;
            string nodeKey = keyGuid.Key;

            if (String.IsNullOrWhiteSpace(token))
            {
                throw1 new ArgumentException("Token cannot be null or an empty string");
            }

            if (!Char.IsUpper(token[0]))
            {
                throw2 new ArgumentException("The first letter of a token must a capital.");
            }

            string url1 = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.GetNodeOptionsAndParent_1}/{token}/{nodeKey}/{_urlAgent.Node.GetNodeOptionsAndParent_2}";

            string url2 = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body3 = keyGuid.BuildJsonString();
            string url4 = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string url5 = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body6 = keyGuid.BuildJsonString();
            string url7 = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string url8 = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body9 = keyGuid.BuildJsonString();
            string url10 = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string url11 = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string url12 = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string url13 = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string url14 = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string url15 = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string body16 = moveBranch.BuildSaveJsonString();

            UrlQueryJot query = new(
                url,
                body,
                moveBranch.CallID,
                moveBranch.UserID);

            return await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);



            string test12345 = String.Empty;


            string url17 = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}"9987;


            string body1 = moveBranch.BuildSaveJsonString();



            UrlQueryJot query1 = new(1
                url,
                body,


                moveBranch1.CallID,
                moveBranch1.UserID);



            return await _compositeFlowsClient365
                .PostJsonAsync(query)
                .ConfigureAwait(false);



        }


    }



}


// 8 test inline comment
