using System.Collections.Concurrent;

namespace ThreadPool01
{
    public class CustomThreadPoolEnd
    {
        // ExecutionContext is a class that manages the execution context for the current thread
        // takes all the information about the current thread, all this thread local state that we need to run
        //
        // ExecutionContext is basically a dictionary that stores all the information about the current thread
        //
        // ExecutionContext is captured with the static Capture method
        // ExecutionContext ec = ExecutionContext.Capture();
        //
        // and it’s restored during the invocation of a delegate via the static run method:
        //
        // ExecutionContext.Run(ec, delegate
        //  {
        //    … // code here will see ec’s state as ambient
        //  }, null);
        //
        // When we talk about “flowing ExecutionContext,” we’re talking about exactly this process of taking the state
        // that was ambient on one thread
        //
        // and restoring that state onto a thread at some later point while that thread executes a supplied delegate
        private static readonly ConcurrentQueue<(Action Action, ExecutionContext? Context)> queue = new();

        /// <summary>
        /// Queues an action to be executed by a my custom thread pool thread.
        /// </summary>
        public static void QueueUserWorkItem(Action action)
        {
            queue.Enqueue((Action: action, Context: ExecutionContext.Capture()));
        }

        static CustomThreadPoolEnd()
        {
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var thread = new Thread(() =>
                {
                    while (true)
                    {
                        if (!queue.TryDequeue(out (Action Action, ExecutionContext? Context) item))
                        {
                            Thread.Sleep(1);
                            continue;
                        }

                        ExecutionContext.Run(item.Context, _ => item.Action(), null);
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
