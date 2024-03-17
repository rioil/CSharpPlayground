var tcs = new TaskCompletionSource();
var task = tcs.Task;
_ = Task.Run(() => Thread.Sleep(TimeSpan.FromMinutes(1)));

// .NET 6 or higher
var timeout = TimeSpan.FromSeconds(5);
await task.WaitAsync(timeout);

// internal implementation of WaitAsync (for .NET Framework)
var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;
var timeoutTcs = new TaskCompletionSource<bool>();
using (new Timer(s => ((TaskCompletionSource<bool>)s).TrySetException(new TimeoutException()) /* MEMO: ここで元タスクのキャンセル処理もする */,
    timeoutTcs, timeout, Timeout.InfiniteTimeSpan))
using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetCanceled(), timeoutTcs))
{
    await (await Task.WhenAny(task, timeoutTcs.Task).ConfigureAwait(false)).ConfigureAwait(false);
}
