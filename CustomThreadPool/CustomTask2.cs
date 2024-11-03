using System.Runtime.CompilerServices;

namespace ThreadPool01
{
    public class CustomTask2
    {
        private Exception? exception;
        private Action? continuation;
        private ExecutionContext? context;


        public struct Awaiter : INotifyCompletion
        {
            private readonly CustomTask2 task;
            public Awaiter(CustomTask2 task) => this.task = task;
            public bool IsCompleted => task.IsCompleted;
            public void OnCompleted(Action continuation) => task.ContinueWith(continuation);
            public void GetResult()
            {
                task.Wait();
            }
            //public Awaiter GetAwaiter() => this;
        }

        public Awaiter GetAwaiter() => new(this);


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
        public static CustomTask2 Run(Action action)
        {
            var task = new CustomTask2();
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

        /// <summary>
        /// Compkete the task with an optional exception 
        /// </summary>
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

        /// <summary>
        /// Block until the task is completed
        /// </summary>
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

        /// <summary>
        /// Continue with an action
        /// </summary>
        public CustomTask2 ContinueWith(Action action)
        {

            var work = new CustomTask2();

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

            // if the task is completed, run the continuation directly
            if (IsCompleted)
            {
                CustomThreadPoolEnd.QueueUserWorkItem(callback);
            }
            // else queue the continuation for later once the current task is completed
            else
            {
                // get continuation for later
                continuation = callback;
                // capture the current execution context for later
                context = ExecutionContext.Capture();
            }

            return work;
        }


        /// <summary>
        /// When all tasks are completed
        /// </summary>
        public static CustomTask2 WhenAll(params CustomTask2[] tasks)
        {
            var task = new CustomTask2();
            var count = tasks.Length;

            if (tasks.Length == 0)
            {
                task.SetResult();
            }
            else
            {
                Action continuation = () =>
                {
                    if (Interlocked.Decrement(ref count) == 0)
                        task.SetResult();
                };

                foreach (var w in tasks)
                    w.ContinueWith(continuation);

            }
            return task;
        }


        /// <summary>
        /// Delay the task
        /// </summary>
        public static CustomTask2 Delay(int milliseconds)
        {
            var task = new CustomTask2();

            var timer = new Timer(_ => task.SetResult(), null, milliseconds, Timeout.Infinite);

            return task;
        }

    }
}