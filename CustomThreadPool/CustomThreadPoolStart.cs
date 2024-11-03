using System.Collections.Concurrent;

namespace ThreadPool01
{
    /// <summary>
    /// A custom thread pool implementation.
    /// </summary>
    public class CustomThreadPoolStart
    {

        private static readonly ConcurrentQueue<Action> queue = new();

        /// <summary>
        /// Queues an action to be executed by a my custom thread pool thread.
        /// </summary>
        public static void QueueUserWorkItem(Action action)
        {
            queue.Enqueue(action);
        }

        static CustomThreadPoolStart()
        {
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var thread = new Thread(() =>
                {
                    while (true)
                    {

                        if (!queue.TryDequeue(out Action? item) || item is null)
                        {
                            Thread.Sleep(1);
                            continue;
                        }

                        item();
                    }
                })
                {
                    IsBackground = true
                };

                thread.Start();
            }
        }

    }

}
