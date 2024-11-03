namespace ThreadPool01
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Sample01();

            Console.ReadLine();
        }

        /// <summary>
        /// Using the ThreadPool
        /// - Make a correction to use a captured value
        /// - Make a correction to use a captured value with Work<T>
        /// - Make a correction to use a captured value with AsyncLocal
        /// </summary>
        private static void Sample01()
        {
            // var asyncLocal = new Work<int>();
            var cpt = 0;
            for (int i = 0; i < 100; i++)
            {
                cpt = i;
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Console.WriteLine($"{cpt} - ID: {Environment.CurrentManagedThreadId}");
                    Thread.Sleep(1000);
                });
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Using CustomThreadPoolStart
        /// Using CustomThreadPoolEnd
        /// </summary>
        private static void Sample02()
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


        private static void Sample03()
        {
            Console.WriteLine($"Start.- ID: {Environment.CurrentManagedThreadId}");

            var task = CustomTask.Run(() =>
            {
                Console.WriteLine($"Hello - ID: {Environment.CurrentManagedThreadId}");
                Thread.Sleep(1000);
            }).ContinueWith(() =>
            {
                Console.WriteLine($"World - ID: {Environment.CurrentManagedThreadId}");
            });

            task.Wait();

            Console.WriteLine($"Done.- ID: {Environment.CurrentManagedThreadId}");
        }

        private static void Sample04()
        {
            Console.WriteLine($"Start.- ID: {Environment.CurrentManagedThreadId}");
            var task = CustomTask.Delay(3000);
            task.Wait();
            Console.WriteLine($"Done.- ID: {Environment.CurrentManagedThreadId}");
        }

        private static void Sample05()
        {
            Console.WriteLine($"Start.- ID: {Environment.CurrentManagedThreadId}");
            var tasks = new CustomTask[100];

            for (int i = 0; i < 100; i++)
            {
                var i1 = i;
                tasks[i] = CustomTask.Run(() =>
                {
                    Console.WriteLine(i1);
                    Thread.Sleep(1000);
                });
            }
            CustomTask.WhenAll(tasks).Wait();

            Console.WriteLine($"Done.- ID: {Environment.CurrentManagedThreadId}");
            Console.ReadLine();
        }

        public static async Task Sample06()
        {
            Console.WriteLine($"Start");
            await CustomTask2.Delay(3000);
            Console.WriteLine($"Done.");

        }

    }
}
