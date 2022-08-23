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

                    private Task<(string fred, (string fred, List<int> joe))[]> ValidateKey8(
                        KeyGuid keyGuid,
                        string[] harry,
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
                }
            }
        }
    }
}
