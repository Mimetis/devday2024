using Newtonsoft.Json.Linq;
using Octokit;
using System.Runtime.CompilerServices;

namespace IAsyncEnumerable
{
    internal class GithubRepository2
    {
        internal static async IAsyncEnumerable<JToken> RunPagedQueryAsync(GitHubClient client, string queryText, string repoName, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {

            JObject issues(JObject result) => (JObject)result["data"]!["repository"]!["issues"]!;
            JObject pageInfo(JObject result) => (JObject)issues(result)["pageInfo"]!;

            var issueAndPRQuery = new GraphQLRequest
            {
                Query = queryText
            };
            issueAndPRQuery.Variables["repo_name"] = repoName;

            bool hasMorePages = true;
            int pagesReturned = 0;
            int issuesReturned = 0;

            // Stop with 10 pages, because these are large repos:
            while (hasMorePages && (pagesReturned++ < 10))
            {
                var postBody = issueAndPRQuery.ToJsonText();
                var response = await client.Connection.Post<string>(new Uri("https://api.github.com/graphql"),
                    postBody, "application/json", "application/json");

                JObject results = JObject.Parse(response.HttpResponse.Body.ToString()!);

                int totalCount = (int)issues(results)["totalCount"]!;
                hasMorePages = (bool)pageInfo(results)["hasPreviousPage"]!;
                issueAndPRQuery.Variables["start_cursor"] = pageInfo(results)["startCursor"]!.ToString();
                issuesReturned += issues(results)["nodes"]!.Count();

                foreach (JObject issue in issues(results)["nodes"]!)
                    yield return issue;
            }
        }
    }
}
