namespace MvvmKit.Abstractions.Threading;

public interface IMainThreadAsyncDispatcher
{
	Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true);

	Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true);

	bool IsOnMainThread { get; }
}