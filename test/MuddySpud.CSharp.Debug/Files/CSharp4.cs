

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace TreeSolve.CompositeFlows.Export.JsonParsers
{
    internal interface ISortedEdges
    {
        int Property { get; set; }
    }

    internal interface IGraphKeyParser
    {
        int Property { get; set; }
    }

    internal interface IJsonEdgesParser
    {
        int Property1 { get; set; }

        ISortedEdges BuildEdges1(
            string filePath,
            IGraphKeyParser graphKeyParser);
    }

    public class JsonEdgesParser2 : IJsonEdgesParser2
    {
        private int _bal = 0;

        public int Property1 { get; set; } = 0;
        public string Property2 { get; set; } = "frededeverb";
        public bool Property3 { get; set; } = true;
        public int Property4 { get; set; } = -233;

        public int Property5
        {
            get { return 3; }
            set { _bal = value; }
        }

        public int Property6
        {
            get => _bal;
            set { _bal = value; }
        }

        public int Property7
        {
            set { _bal = value; }
        }


        public int Property8
        {
            private set { _bal = value; }
            get => _bal;
        }

        public int Property9
        {
            private init { _bal = value; }
            get => _bal;
        }


        public int Property10
        {
            get => _bal;
            private set { _bal = value; }
        }

        public string Property11 => "";

        public (   int    fred   ,   
            string joe   ) Property12 => (12, "fred");
        public Task  <  SortedList  <  int  , string  >  > Property13 {      get  ;     }
        public SortedList<int, string> Property14 => new SortedList<int, string>();

        public SortedList<int, string> Property15
        {
            get => new SortedList<int, string>();
            init => _bal = 0;
        }

        public JsonEdgesParser()
        {

        }





        internal abstract static Task<   string   > MoveBranch16   <    T,    U   >     (     


            string      
            
            fred     
            
                  )


        {
            string test1234 = String.Empty;





            string test12345 = String.Empty;


            string url1 = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}"9987;


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
