namespace ThreadPool01
{
    public class CustomTask
    {
        private Exception? exception;
        private Action? continuation;
        private ExecutionContext? context;

        /// <summary>
        /// Gets a value indicating whether the task has completed.
        /// Be careful, we need to kind of lock(this) but as simplicity, we will not here
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Set the result of the task
        /// </summary>
        public void SetResult() => Complete(null);


        /// <summary>
        /// Set the exception of the task
        /// </summary>
        /// <param name="error"></param>
        public void SetException(Exception error) => Complete(error);


        /// <summary>
        /// Run an action asynchronously
        /// </summary>
        public static CustomTask Run(Action action)
        {
            var task = new CustomTask();
            CustomThreadPoolEnd.QueueUserWorkItem(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    task.SetException(ex);
                    return;
                }

                task.SetResult();
            });
            return task;
        }


        public static CustomTask WhenAll(params CustomTask[] works)
        {
            var task = new CustomTask();
            var count = works.Length;

            if (works.Length == 0)
            {
                task.SetResult();
            }
            else
            {
                Action continuation = () =>
                {
                    // Would be better to use Interlocked.Decrement(ref count) to avoid two differents threads to decrement the count at the same time
                    // or just a lock(this) but as simplicity, we will not here
                    count--;

                    if (count == 0)
                        task.SetResult();
                };

                foreach (var w in works)
                    w.ContinueWith(continuation);

            }
            return task;
        }

        public CustomTask Delay(int milliseconds)
        {
            var task = new CustomTask();

            var timer = new Timer(_ => task.SetResult(), null, milliseconds, Timeout.Infinite);

            return task;
        }

        //public static Work Iterate(IEnumerable<Work> works)
        //{
        //    var task = new Work();
        //    var enumerator = works.GetEnumerator();

        //    while (enumerator.MoveNext())
        //    {

        //        enumerator.Current.ContinueWith(() =>
        //        {
        //            if (!enumerator.MoveNext())
        //                task.SetResult();
        //        });
        //    };

        //    return task;
        //}

        /// <summary>
        /// Compkete the task with an optional exception 
        /// </summary>
        /// <param name="error"></param>
        private void Complete(Exception? error)
        {
            if (IsCompleted)
                throw new InvalidOperationException("Already completed");

            exception = error;
            IsCompleted = true;

            if (continuation is not null)
            {
                CustomThreadPoolEnd.QueueUserWorkItem(() =>
                {
                    if (context is not null)
                        ExecutionContext.Run(context, _ => continuation(), null);
                    else
                        continuation();
                });
            }


        }


        public void Wait()
        {
            // block until the task is completed
            ManualResetEventSlim? mres = null;

            if (!IsCompleted)
            {
                mres = new ManualResetEventSlim();
                ContinueWith(mres.Set);
            }

            mres?.Wait();

            if (exception is not null)
            {
                // better to use ExceptionDispatchInfo.Throw(exception); to get the original stack trace
                // but for simplicity, we will just throw the exception
                throw exception;
            }


        }
        public CustomTask ContinueWith(Action action)
        {

            var work = new CustomTask();

            Action callback = () =>
            {
                try
                {
                    action();
                    work.SetResult();
                }
                catch (Exception ex)
                {
                    work.SetException(ex);
                }
            };

            if (IsCompleted)
            {
                CustomThreadPoolEnd.QueueUserWorkItem(callback);
            }
            else if (continuation is not null)
            {
                throw new InvalidOperationException("Unlike Task, this implementation only supports a single continuation.");
            }
            else
            {
                // get continuation for later
                continuation = callback;
                // capture the current execution context for later
                context = ExecutionContext.Capture();
            }

            return work;
        }
    }
}