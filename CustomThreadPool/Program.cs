namespace ThreadPool01
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Sample01();
        }

        /// <summary>
        /// Storing a value in a class
        /// </summary>
        public class Work<T> { public T? Value { get; set; } }


        /// <summary>
        /// Using the ThreadPool
        /// - Make a correction to use a captured value
        /// - Make a correction to use a captured value with Work<T>
        /// - Make a correction to use a captured value with AsyncLocal
        /// </summary>
        private static void Sample00()
        {
            var asyncLocal = new Work<int>();

            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Console.WriteLine($"{asyncLocal.Value} - ID: {Environment.CurrentManagedThreadId}");
                    Thread.Sleep(1000);
                });
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Using the custom thread pool
        /// Using CustomThreadPoolStart
        /// Using CustomThreadPoolEnd
        /// </summary>
        private static void Sample01()
        {

            AsyncLocal<int> asyncLocal = new();

            for (int i = 0; i < 100; i++)
            {
                asyncLocal.Value = i;

                CustomThreadPoolEnd.QueueUserWorkItem(delegate
                {
                    Console.WriteLine(asyncLocal.Value);
                    Thread.Sleep(1000);
                });

            }

            Console.ReadLine();
        }

    }



}
