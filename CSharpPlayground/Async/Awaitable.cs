using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayground.Async
{
    internal class BlockingAwaitable<T>
    {
        private BlockingAwaiter<T> _awaiter;

        public BlockingAwaitable(Task<T> task)
        {
            _awaiter = new BlockingAwaiter<T>(task);
        }

        public BlockingAwaiter<T> GetAwaiter() => _awaiter;
    }

    internal struct BlockingAwaiter<T>
    {
        private Task<T> _task;

        public BlockingAwaiter(Task<T> task) { _task = task; }

        public bool IsCompleted => true;

        public void OnCompleted(Action continuation) { }

        public T GetResult()
        {
            _task.Wait();
            return _task.Result;
        }
    }

    internal static class BlockingAwaitableExtensions
    {
        public static BlockingAwaitable<T> ToBlocking<T>(this Task<T> task)
        {
            return new BlockingAwaitable<T>(task);
        }
    }
}
