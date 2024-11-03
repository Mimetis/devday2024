using Octokit;

namespace IAsyncEnumerable
{
    internal class Program
    {
        private const string PagedIssueQuery =
            @"query ($repo_name: String!,  $start_cursor:String) {
              repository(owner: ""Mimetis"", name: $repo_name) {
                issues(last: 25, before: $start_cursor)
                 {
                    totalCount
                    pageInfo {
                      hasPreviousPage
                      startCursor
                    }
                    nodes {
                      title
                      number
                      createdAt
                    }
                  }
                }
              }
            ";

        private static async Task Main(string[] args)
        {
            await GetIssuesAsync();
        }


        private static async Task GetIssuesAsync()
        {
            var cancellationSource = new CancellationTokenSource();
            var key = "";

            var client = new GitHubClient(new ProductHeaderValue("IAsyncEnumerableDemo"))
            {
                Credentials = new Credentials(key)
            };

            var progressReporter = new ProgressStatus((num) => Console.WriteLine($"Received {num} issues in total"));


            try
            {
                var results = await GithubRepository.RunPagedQueryAsync(client, PagedIssueQuery, "Dotmim.Sync", cancellationSource.Token, progressReporter);
                foreach (var issue in results)
                    Console.WriteLine(issue);

                //await foreach (var issue in GithubRepository2.RunPagedQueryAsync(client, PagedIssueQuery, "Dotmim.Sync").WithCancellation(cancellationSource.Token))
                //{
                //    Console.WriteLine(issue);
                //}

            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Work has been cancelled");
            }
        }
    }
}
