//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ReturnValueTask
//{
//    internal class BadWithValueTask
//    {
//        // Given this ValueTask<int>-returning method…
//        public ValueTask<int> SomeValueTaskReturningMethodAsync();
//        // GOOD
//        int result = await SomeValueTaskReturningMethodAsync();
//        int result = await SomeValueTaskReturningMethodAsync().ConfigureAwait(false);
//        Task<int> t = SomeValueTaskReturningMethodAsync().AsTask();

//        // WARNING
//        ValueTask<int> vt = SomeValueTaskReturningMethodAsync();
//        // storing the instance into a local makes it much more likely it'll be misused,

//        // BAD: awaits multiple times
//        ValueTask<int> vt = SomeValueTaskReturningMethodAsync();
//        int result = await vt;
//        int result2 = await vt;

//        // BAD: awaits concurrently (and, by definition then, multiple times)
//        ValueTask<int> vt = SomeValueTaskReturningMethodAsync();
//        Task.Run(async () => await vt);
//        Task.Run(async () => await vt);

//        // BAD: uses GetAwaiter().GetResult() when it's not known to be done
//        ValueTask<int> vt = SomeValueTaskReturningMethodAsync();
//        int result = vt.GetAwaiter().GetResult();
//    }

//    public class Task
//    {
//        public Task WaitAsync(CancellationToken cancellationToken) => WaitAsync(Timeout.UnsignedInfinite, cancellationToken);

//        public Task WaitAsync(TimeSpan timeout) => WaitAsync(ValidateTimeout(timeout, ExceptionArgument.timeout), default);

//        public Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken) => WaitAsync(ValidateTimeout(timeout, ExceptionArgument.timeout), cancellationToken);
//    }

//}
