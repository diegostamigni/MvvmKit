using MvvmKit.Abstractions.Threading;

namespace MvvmKit.Threading;

public abstract class MainThreadAsyncDispatcher : IMainThreadAsyncDispatcher
{
	public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
	{
		var asyncAction = new Func<Task>(() =>
		{
			action();
			return Task.CompletedTask;
		});

		return ExecuteOnMainThreadAsync(asyncAction, maskExceptions);
	}

	public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
	{
		var completion = new TaskCompletionSource<bool>();

		async void Action()
		{
			await action();

			completion.SetResult(true);
		}

		var syncAction = new Action(Action);

		RequestMainThreadAction(syncAction, maskExceptions);

		// If we're already on main thread, then the action will
		// have already completed at this point, so can just return
		if (completion.Task.IsCompleted)
		{
			return;
		}

		// Make sure we don't introduce weird locking issues
		// blocking on the completion source by jumping onto
		// a new thread to wait
		await Task.Run(async () => await completion.Task);
	}

	public static void ExceptionMaskedAction(Action action, bool maskExceptions)
	{
		try
		{
			action();
		}
		catch (Exception)
		{
			if (!maskExceptions)
			{
				throw;
			}
		}
	}

	public abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);

	public abstract bool IsOnMainThread { get; }
}