namespace IAsyncEnumerable
{
    internal class ProgressStatus(Action<int> progressAction) : IProgress<int>
    {
        private readonly Action<int> action = progressAction;
        public void Report(int value) => action(value);
    }
}
