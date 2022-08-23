// 1 test inline comment

/* 0.1 test prefix comment */
/* 0.2 test prefix comment */
/* 0.3 test prefix comment */
/* 0.4 test prefix comment */
// 2 test inline comment

/* 0.5 test prefix comment */
// 3 test inline comment

// 4 test inline comment

// 5 test inline comment

/* 0.6 test prefix comment 



            */
// 6 test inline comment

// 7 test inline comment

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

// Declare an array to store the data elements.

// Define the indexer to allow client code to use [] notation.

// Declare an array to store the data elements.

// Define the indexer to allow client code to use [] notation.

// Declare an array to store the data elements.

// output: -5 / 4

// output: 14 / 8

// output: 6 / 8

// output: 5 / 8

// output: 10 / 4

// output: 10 / 4

// 8 test inline comment

// concrete type

//closed constructed type

//open constructed type

//No error

//No error

// This constructor will call BaseClass.BaseClass(int i)

// concrete type

//closed constructed type

//open constructed type

//No error

//No error

// This constructor will call BaseClass.BaseClass(int i)

/// <summary>

/// Retrieve or manipulate individual nodes

/// </summary>

// Uri properties should not be strings

// Uri properties should not be strings

/// <summary>

/// Test

/// </summary>

/// <param name="name">Name</param>

