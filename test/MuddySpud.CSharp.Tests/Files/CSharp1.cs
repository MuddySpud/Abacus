

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

    public class JsonEdgesParser : IJsonEdgesParser
    {
        private int _bal = 0;

        public int Property1 { get; set; } = 0;

        public int Property2
        {
            get { return 3; }
            set { _bal = value; }
        }

        public int Property3
        {
            get => _bal;
            set { _bal = value; }
        }

        public int Property4
        {
            set { _bal = value; }
        }


        public int Property5
        {
            private set { _bal = value; }
            get => _bal;
        }


        public int Property6
        {
            get => _bal;
            private set { _bal = value; }
        }

        public string Property7 => "";

        public JsonEdgesParser()
        {

        }


        JsonEdgesParser(string fred)
        {

        }

        JsonEdgesParser((string fred, int joe) gaz, double kez = "kez1")
        {

        }

        JsonEdgesParser(Dictionary<string fred, int joe> gaz, double kez)
        {

        }

        ~JsonEdgesParser()
        {
        }

        // 1 test inline comment
        public ISortedEdges BuildEdges1(
            string filePath,
            IGraphKeyParser graphKeyParser)
        {
            List<Edge> edges = null;

            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                edges = (List<Edge>)serializer.Deserialize(file, typeof(List<Edge>));
            }

            /* 0.1 test prefix comment */

            int[] toKeys = new int[edges.Count];
            int[] fromKeys = new int[edges.Count];
            IEdge[] toEdges = new IEdge[edges.Count];
            IEdge[] fromEdges = new IEdge[edges.Count];/* 0.2 test prefix comment */
            IEdge edge = null;
            string toKey = String.Empty;
            string fromKey = /* 0.3 test prefix comment */String.Empty;
            int to = 0;
            int from = 0;
            /* 0.4 test prefix comment */
            for (int i = 0; i < edges.Count; i++) // 2 test inline comment
            {
                edge /* 0.5 test prefix comment */ = edges[i];
                toKey = graphKeyParser.GetNodeKey(edge._to);
                to = Int32.Parse(toKey);
                edge.To = to;
                toKeys[i] = to;
                fromKey = graphKeyParser.GetNodeOrRootKey(edge._from);
                from = Int32.Parse(fromKey);
                edge.From = 1;
                fromKeys[i] = from;
                toEdges[i] = edge;
                fromEdges[i] = edge;
            }
            // 3 test inline comment
            // 4 test inline comment

            // 5 test inline comment

            Array.Sort(toKeys, toEdges);
            Array.Sort(fromKeys, fromEdges);
            /* 0.6 test prefix comment 



            */
            SortedList<int, IEdge> sortedToEdges = new SortedList<int, IEdge>();

            for (int i = 0; i < toEdges.Length; i++)
            {
                sortedToEdges.Add(toKeys[i], toEdges[i]);
            }

            SortedList<int, List<IEdge>> sortedFromEdges = new SortedList<int, List<IEdge>>();
            List<IEdge> little = null;
            int lastFrom = 0;
            from = 0;

            for (int i = 0; i < fromEdges.Length; i++)
            {
                from = fromKeys[i];     // 6 test inline comment

                if (from != lastFrom)
                {
                    little = new List<IEdge>();
                    sortedFromEdges.Add(from, little);
                }// 7 test inline comment

                little.Add(fromEdges[i]);
                lastFrom = from;
            }

            return new SortedEdges(sortedFromEdges, sortedToEdges);
        }

        internal async Task<string> GetNodeOptionsAndParent2(KeyGuid keyGuid)
        {
            string token = keyGuid.Token;
            string nodeKey = keyGuid.Key;

            if (String.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or an empty string");
            }

            if (!Char.IsUpper(token[0]))
            {
                throw new ArgumentException("The first letter of a token must a capital.");
            }

            string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.GetNodeOptionsAndParent_1}/{token}/{nodeKey}/{_urlAgent.Node.GetNodeOptionsAndParent_2}";

            return await _compositeFlowsClient
                .GetAsync(
                    url,
                    keyGuid.CallID,
                    keyGuid.UserID)
                .ConfigureAwait(false);
        }

        //internal async Task SaveDiscussionJson(NodeFull node)
        //{
        //    //if (node.Bin?.Discussion?.Type != DiscussionType.MarkdownJson
        //    //        && node.Bin?.Discussion?.Type != DiscussionType.MarkdownJson
        //    //        && node.Bin?.Discussion?.Type != DiscussionType.MarkdownJson)
        //    //{
        //    //    return;
        //    //}

        //    string discussionJson = Markdown.ToJson(node.Discussion);
        //    string url = $"{node.SubscriptionID}/treeSolve/{_urlAgent.Node.SavePublishJson}";
        //    string body = node.BuildPublishJsonString(discussionJson);

        //    UrlQueryJot query = new(
        //        url,
        //        body,
        //        node.CallID,
        //        node.UserID);

        //    await _compositeFlowsClient
        //        .PostJsonAsync(query)
        //        .ConfigureAwait(false);
        //}

        // internal async Task<string> SaveDiscussionJson(
        //    string nodeID,
        //    KeyGuid keyGuid,
        //    string discussionJson,
        //    string discussion,
        //    string bin)
        //{
        //    string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.SavePublishJson}";

        //    JObject jsonObject = new()
        //    {
        //        { "token", keyGuid.Token },
        //        { "discussion", discussion },
        //        { "discussionJson", discussionJson },
        //        { "nodeID", nodeID },
        //        { "bin", JObject.Parse(bin) }
        //    };

        //    string body = jsonObject.ToString();

        //    UrlQueryJot query = new(
        //        url,
        //        body,
        //        keyGuid.CallID,
        //        keyGuid.UserID);

        //    return await _compositeFlowsClient
        //        .PostJsonAsync(query)
        //        .ConfigureAwait(false);
        //}

        internal static async Task<string> ValidateKey3(KeyGuid keyGuid)
        {
            string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body = keyGuid.BuildJsonString();

            UrlQueryJot query = new(
                url,
                body,
                keyGuid.CallID,
                keyGuid.UserID);

            string response = await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);

            return response.AddCallID(keyGuid.CallID);
        }

        internal async static Task<(string fred, List<int> joe)> ValidateKey4(KeyGuid keyGuid,
            string harry,
            Dictionary<string, int> bert)
        {
            string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body = keyGuid.BuildJsonString();

            UrlQueryJot query = new(
                url,
                body,
                keyGuid.CallID,
                keyGuid.UserID);

            string response = await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);

            return response.AddCallID(keyGuid.CallID);
        }

        async static Task<(string fred, List<int> joe)> ValidateKey5(
            KeyGuid keyGuid,
            string harry,
            Dictionary<string, int> bert)
        {
            string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body = keyGuid.BuildJsonString();

            UrlQueryJot query = new(
                url,
                body,
                keyGuid.CallID,
                keyGuid.UserID);

            string response = await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);

            return response.AddCallID(keyGuid.CallID);
        }

        Task<(string fred, string[] joe)> ValidateKey6(
            KeyGuid keyGuid,
            string harry,
            Dictionary<string, int> bert)
        {
            string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body = keyGuid.BuildJsonString();

            UrlQueryJot query = new(
                url,
                body,
                keyGuid.CallID,
                keyGuid.UserID);

            string response = await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);

            return response.AddCallID(keyGuid.CallID);
        }

        protected Task < ( string  fred , List < int [   ] > joe ) > ValidateKey7 (
            KeyGuid keyGuid,
            string harry,
            Dictionary < string , int [ ] > bert ) 
        {
            string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body = keyGuid.BuildJsonString();

            UrlQueryJot query = new(
                url,
                body,
                keyGuid.CallID,
                keyGuid.UserID);

            string response = await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);

            return response.AddCallID(keyGuid.CallID);
        }


        private Task<(string fred, (string fred, List<int> joe) )[   ]> ValidateKey8(
            KeyGuid keyGuid,
            string[  ] harry,
            Dictionary<string, int> bert)
        {
            string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body = keyGuid.BuildJsonString();

            UrlQueryJot query = new(
                url,
                body,
                keyGuid.CallID,
                keyGuid.UserID);

            string response = await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);

            return response.AddCallID(keyGuid.CallID);
        }

        private (string fred, List<int> joe) ValidateKey9(
            KeyGuid keyGuid,
            string harry,
            Dictionary<string, int> bert)
        {
            string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body = keyGuid.BuildJsonString();

            UrlQueryJot query = new(
                url,
                body,
                keyGuid.CallID,
                keyGuid.UserID);

            string response = await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);

            return response.AddCallID(keyGuid.CallID);
        }

        (string fred, List<int> joe) ValidateKey10(
            KeyGuid keyGuid,
            string harry,
            Dictionary<string, int> bert)
        {
            string url = $"{keyGuid.SubscriptionID}/treeSolve/{_urlAgent.Node.ValidateKey}";
            string body = keyGuid.BuildJsonString();

            UrlQueryJot query = new(
                url,
                body,
                keyGuid.CallID,
                keyGuid.UserID);

            string response = await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);

            return response.AddCallID(keyGuid.CallID);
        }

        public override string ToString11() => $"{fname} {lname}".Trim();

        public override string ToString12()
        {
            string url = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string body = moveBranch.BuildSaveJsonString();

            UrlQueryJot query = new(
                url,
                body,
                moveBranch.CallID,
                moveBranch.UserID);

            return await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);
        }

        internal abstract static Task<string> MoveBranch13(MoveBranch moveBranch = 1)
        {
            string url = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string body = moveBranch.BuildSaveJsonString();

            UrlQueryJot query = new(
                url,
                body,
                moveBranch.CallID,
                moveBranch.UserID);

            return await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);
        }

        internal abstract static Task<string> MoveBranch14()
        {
            string url = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string body = moveBranch.BuildSaveJsonString();

            UrlQueryJot query = new(
                url,
                body,
                moveBranch.CallID,
                moveBranch.UserID);

            return await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);
        }

        internal abstract static Task<string> MoveBranch15<T, U>()
        {
            string url = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string body = moveBranch.BuildSaveJsonString();

            UrlQueryJot query = new(
                url,
                body,
                moveBranch.CallID,
                moveBranch.UserID);

            return await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);



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

    internal class JsonEdgesParser2 : IJsonEdgesParser
    {
        public A this[int i]
        {
            get { return arr[i]; }
            set { arr[i] = value; }
        }

        public B this 
            [ 
            int i 
            ]
        {
            get { return arr[i]; }
            set { arr[i] = value; }
        }

        // Declare an array to store the data elements.
        private T[] arr = new T[100];
        int nextIndex = 0;

        // Define the indexer to allow client code to use [] notation.
        public C this[int i] => arr[i];

        public D this[int i] =>
            arr[i];

        public E this[int i]
            =>
            arr[i];

        public F this[int i]
            => arr[i];

        public void Add(T value)
        {
            if (nextIndex >= arr.Length)
                throw new IndexOutOfRangeException($"The collection can hold only {arr.Length} elements.");
            arr[nextIndex++] = value;
        }

        // Declare an array to store the data elements.
        private T[] arr = new T[100];

        // Define the indexer to allow client code to use [] notation.
        public G this[string i]
        {
            get => arr[i];
            set => arr[i] = value;
        }

        public Task<SortedList<int, string>> this[string i]
        {
            get => temp[i];
            set
            {
                temp[i] = value;
            }
        }

        // Declare an array to store the data elements.
        private Task<SortedList<int, string>>[] temp = new Task<SortedList<int, string>>[100];

        public Task < SortedList < int , ( string fred , int joe ) > > this [ string i ]
        {
            get { return temp2[i]; }
            set
            {
                temp2[i] = value;
            }
        }

        public Task<SortedList<int, (string fred, int[] joe)>> this[int i]
        {
            get => arr[i];
            set => arr[i] = value;
        }

        public Task<SortedList<int, (string fred, 
            int [
            ] 
            joe)>> this[int i]
        {
            get => arr[i];
            set => arr[i] = value;
        }

        public void Fred(T value)
        {
            if (nextIndex >= arr.Length)
                throw new IndexOutOfRangeException($"The collection can hold only {arr.Length} elements.");
            arr[nextIndex++] = value;
        }

        public Task <  SortedList < 
            int, (  string fred,  
            int joe ) > > this
              [int i
            ]
        {
            get { return temp2[i]; }
            set
            {
                temp2[i] = value;
            }
        }

        public event MouseEventHandler MouseUp
        {
            add { AddEventHandler(mouseUpEventKey, value); }
            remove { RemoveEventHandler(mouseUpEventKey, value); }
        }

        public   
             event   
              MouseEventHandler   

            MouseDown

        {
            add { AddEventHandler(mouseUpEventKey, value); }
            remove { RemoveEventHandler(mouseUpEventKey, value); }
        }

        public static implicit operator double(Vector v)
        {
            return v.Length;
        }

        public static implicit operator AuthorDto(Author author)
        {
            AuthorDto authorDto = new AuthorDto();
            authorDto.Id = author.Id.ToString();
            authorDto.FirstName = author.FirstName;
            authorDto.LastName = author.LastName;
            return authorDto;
        }

        public static explicit operator AuthorDto(Author author)
        {
            AuthorDto authorDto = new AuthorDto();
            authorDto.Id = author.Id.ToString();
            authorDto.FirstName = author.FirstName;
            authorDto.LastName = author.LastName;
            return authorDto;
        }
    }


    public class JsonEdgesParser3 : IJsonEdgesParser2
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

        public (int fred,
            string joe) Property12 => (12, "fred");
        public Task<SortedList<int, string>> Property13 { get; }

        public Task<SortedList<int, (string fred, int[   ] joe)>> this[int i]
        {
            get { return temp2[i]; }
            set
            {
                temp2[i] = value;
            }
        }

        public Task<SortedList<int, (string fred, int[
] joe)>> this[int i]
        {
            get { return temp2[i]; }
            set
            {
                temp2[i] = value;
            }
        }

        public SortedList<int, string> Property14 => new SortedList<int, string>();

        public SortedList<int, string> Property15
        {
            get => new SortedList<int, string>();
            init => _bal = 0;
        }

        public JsonEdgesParser()
        {

        }

    }

    public readonly struct Fraction
    {
        private readonly int num;
        private readonly int den;

        public int Property8
        {
            private set { _bal = value; }
            get => _bal;
        }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            num = numerator;
            den = denominator;
        }

        public static Fraction operator +(Fraction a) => a;
        public static Fraction operator -(Fraction a) => new Fraction(-a.num, a.den);

        public static Fraction operator +(Fraction a, Fraction b)
            => new Fraction(a.num * b.den + b.num * a.den, a.den * b.den);

        public static Fraction operator - ( Fraction a, Fraction b )
            => a + (-b);

        public static 
            Fraction 
            operator *
            (
            Fraction a, 
            Fraction b
            ) => new Fraction(a.num * b.num, a.den * b.den);

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.num == 0)
            {
                throw new DivideByZeroException();
            }
            return new Fraction(a.num * b.den, a.den * b.num);
        }

        public override string ToString() => $"{num} / {den}";

        public static implicit operator double(Vector v)
        {
            return v.Length;
        }

        public static implicit operator AuthorDto(Author author)
        {
            AuthorDto authorDto = new AuthorDto();
            authorDto.Id = author.Id.ToString();
            authorDto.FirstName = author.FirstName;
            authorDto.LastName = author.LastName;
            return authorDto;
        }

        public static explicit operator AuthorDto(Author author)
        {
            AuthorDto authorDto = new AuthorDto();
            authorDto.Id = author.Id.ToString();
            authorDto.FirstName = author.FirstName;
            authorDto.LastName = author.LastName;
            return authorDto;
        }

        internal abstract static Task < string> MoveBranch14 (  )
        {
            string url = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string body = moveBranch.BuildSaveJsonString();

            UrlQueryJot query = new(
                url,
                body,
                moveBranch.CallID,
                moveBranch.UserID);

            return await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);
        }
    }

    public static class OperatorOverloading
    {
        public static void Main ( )
        {
            var a = new Fraction(5, 4);
            var b = new Fraction(1, 2);
            Console.WriteLine(-a);   // output: -5 / 4
            Console.WriteLine(a + b);  // output: 14 / 8
            Console.WriteLine(a - b);  // output: 6 / 8
            Console.WriteLine(a * b);  // output: 5 / 8
            Console.WriteLine(a / b);  // output: 10 / 4
        }
    }

    public record DailyTemperature ( double HighTemp , double LowTemp )
    {
        public double Mean => (HighTemp + LowTemp) / 2.0;

        public double Add(int 20)
        {
            return new DailyTemperature(HighTemp + 20, LowTemp);
        }
    }

    public abstract record DegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords = 3);

    public sealed record HeatingDegreeDays(double BaseTemperature, IEnumerable<DailyTemperature> TempRecords)
        : DegreeDays(BaseTemperature, TempRecords)
    {
        public double DegreeDays => TempRecords.Where(s => { return s.Mean < BaseTemperature; }).Sum(s => BaseTemperature - s.Mean);
    }

    public sealed record CoolingDegreeDays
        (
        double BaseTemperature, 
        IEnumerable < DailyTemperature > TempRecords
        )
        : DegreeDays ( BaseTemperature, 
            TempRecords ) 
    {
        public double DegreeDays
        {
            get => TempRecords.Where(s => s.Mean > BaseTemperature).Sum(s => s.Mean - BaseTemperature);
        }
    }



}

class BaseClassWithoutNamespace { }


public 
    static 
    class 
    ClassWithoutNamespace <
    T
    > : 
    BaseClassWithoutNamespace
{
    public static void Main()
    {
        Console.WriteLine(a / b);  // output: 10 / 4
    }
}

// 8 test inline comment

namespace TreeSolve.CompositeFlows.Export
{
    class BaseNode { }
    class BaseNodeGeneric<T> { }

    // concrete type
    class NodeConcrete<T> : BaseNode { }

    //closed constructed type
    class NodeClosed<T> : BaseNodeGeneric<int> { }

    //open constructed type
    class NodeOpen<T> : BaseNodeGeneric<T> { }

    class BaseNodeMultiple<T, U> { }

    //No error
    class Node4<T> : BaseNodeMultiple<T, int> { }

    //No error
    class Node5<T, U> : BaseNodeMultiple<T, U> { }

    class NodeItem<T> where T : System.IComparable<T>, new() { }
    class SpecialNodeItem<T> : NodeItem<T> where T : System.IComparable<T>, new() { }

    class SuperKeyType<K, V, U>
        where U : System.IComparable<U>
        where V : new()
    { }

    namespace Nested
    {
        namespace Two
        {
            class ParentConcrete<T> : BaseNode
            {
                class NestedConcrete
                {
                    internal Task<string> TestNested()
                    {
                        string url = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
                        string body = moveBranch.BuildSaveJsonString();

                        UrlQueryJot query = new(
                            url,
                            body,
                            moveBranch.CallID,
                            moveBranch.UserID);

                        return await _compositeFlowsClient
                            .PostJsonAsync(query)
                            .ConfigureAwait(false);
                    }

                    // This constructor will call BaseClass.BaseClass(int i)
                    public NestedConcrete(int i) : base(i)
                    {

                    }
                }
            }
        }
    }
}


namespace TreeSolve.CompositeFlows.Export2
{
    internal interface IFred
    {
        int Property { get; set; }
    }

    internal partial interface IBert
    {
        int Property { get; set; }
    }

    partial class BaseNode { }
    class BaseNodeGeneric<T> { }

    // concrete type
    partial class NodeConcrete<T> : BaseNode { }

    //closed constructed type
    class NodeClosed<T> : BaseNodeGeneric<int> { }

    //open constructed type
    partial class NodeOpen<T> : BaseNodeGeneric<T> { }

    class BaseNodeMultiple<T, U> { }

    //No error
    partial class Node4<T> : BaseNodeMultiple<T, int> { }

    //No error
    class Node5<T, U> : BaseNodeMultiple<T, U> { }

    partial class NodeItem<T> where T : System.IComparable<T>, new() { }
    class SpecialNodeItem<T> : NodeItem<T> where T : System.IComparable<T>, new() { }

    partial class SuperKeyType<K, V, U>
        where U : System.IComparable<U>
        where V : new()
    { }

    namespace Nested
    {
        namespace Two
        {
            class ParentConcrete<T> : BaseNode
            {
                partial class NestedConcrete
                {
                    internal partial Task<string> TestNested()
                    {
                        string url = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
                        string body = moveBranch.BuildSaveJsonString();

                        UrlQueryJot query = new(
                            url,
                            body,
                            moveBranch.CallID,
                            moveBranch.UserID);

                        return await _compositeFlowsClient
                            .PostJsonAsync(query)
                            .ConfigureAwait(false);
                    }

                    // This constructor will call BaseClass.BaseClass(int i)
                    public NestedConcrete(int i) : base(i)
                    {

                    }
                }
            }
        }
    }
}

#region Test regions

namespace Nested
{
    /// <summary>
    /// Retrieve or manipulate individual nodes
    /// </summary>
    public class PreProcessorTest
    {
        #pragma warning disable CA1056 // Uri properties should not be strings
        public string Url { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings
        /// <summary>
        /// Test
        /// </summary>
        /// <param name="name">Name</param>
        public void method1(string name)
        {
            Func<(string, bool)> fred = () =>
                {
                    string h = "";
                    bool intime = true;

                    return (h, intime);
                };

#if DEBUG
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")} - ProcessID = {_processState.ID}");
#endif
        }
    }


}

#endregion

namespace StringTest
{
    public class Stripping
    {
        string Method1() => "Fred was here";
        string Method2() => "Fred was \"NOT here";
        string Method3() => "Fred was NOT\" here";
        string Method4() => "Fred was \"NOT\" here";

        string Method5() => @"Fred
was
here ""LATE""";

        string Method6() => @"Fred was here";
        string Method7() => @"Fred was here" + " today";
        string Method8() => @"Fred was ""NOT\ here";
        string Method9() => @"Fred was NOT"" here";
        string Method10() => @"Fred was ""NOT"" here";
        string Method11() => $"Fred was here: {DateTime.Now.ToString()} 2 \"hours\" late";
        string Method12() => @$"Fred was here: {DateTime.Now.ToString()} 2 hours "" late";
        string Method13() => @$"Fred was here: {DateTime.Now.ToString()} 2 ""hours "" late";
        string Method14() => $@"Fred was here: {DateTime.Now.ToString()} 2 ""hours "" late";
        string Method15() => $"Fred was here: {DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")} 2 hours late";
        string Method16() => $"Fred was here: {DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")}";
        string Method17() => $"{DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")} late";

        string Method18() => $"Fred was here: { String.IsNullOrWhiteSpace(test1) }";
    }
}

namespace Recursion
{
    class Concrete
    {
        internal partial Task<string> TestNested(string fred)
        {
            string url = $"{moveBranch.SubscriptionID}/treeSolve/{_urlAgent.Node.MoveBranch}";
            string body = moveBranch.BuildSaveJsonString();

            UrlQueryJot query = new(
                url,
                body,
                moveBranch.CallID,
                moveBranch.UserID);

            TestNested("today");

            return await _compositeFlowsClient
                .PostJsonAsync(query)
                .ConfigureAwait(false);
        }
    }
}

