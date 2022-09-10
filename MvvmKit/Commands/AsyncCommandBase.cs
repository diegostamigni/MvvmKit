#pragma warning disable CS1998
#pragma warning disable CA1822

namespace MvvmKit.Commands;

public abstract class AsyncCommandBase : CommandBase
{
	private readonly object syncRoot = new();
	private readonly bool allowConcurrentExecutions;
	private CancellationTokenSource? cts;
	private int concurrentExecutions;

	protected AsyncCommandBase(bool allowConcurrentExecutions = false)
	{
		this.allowConcurrentExecutions = allowConcurrentExecutions;
	}

	public bool IsRunning => this.concurrentExecutions > 0;

	protected CancellationToken CancelToken => this.cts?.Token ?? CancellationToken.None;

	protected abstract bool CanExecuteImpl(object? parameter);

	protected abstract Task ExecuteAsyncImpl(object? parameter);

	public void Cancel()
	{
		lock (this.syncRoot)
		{
			this.cts?.Cancel();
		}
	}

	public bool CanExecute() => CanExecute(null);

	public bool CanExecute(object? parameter)
		=> (this.allowConcurrentExecutions || !this.IsRunning) && CanExecuteImpl(parameter);

	public async void Execute(object? parameter) => await ExecuteAsync(parameter, true);

	public void Execute() => Execute(null);

	protected async Task ExecuteAsync(object? parameter, bool hideCanceledException)
	{
		if (CanExecuteImpl(parameter))
		{
			await ExecuteConcurrentAsync(parameter, hideCanceledException);
		}
	}

	private async Task ExecuteConcurrentAsync(object? parameter, bool hideCanceledException)
	{
		var started = false;
		try
		{
			lock (this.syncRoot)
			{
				if (this.concurrentExecutions == 0)
				{
					InitCancellationTokenSource();
				}
				else if (!this.allowConcurrentExecutions)
				{
					return;
				}
				this.concurrentExecutions++;
				started = true;
			}

			if (!this.CancelToken.IsCancellationRequested)
			{
				try
				{
					// With configure await false, the CanExecuteChanged raised in finally clause might run in another thread.
					// This should not be an issue as long as ShouldAlwaysRaiseCECOnUserInterfaceThread is true.
					await ExecuteAsyncImpl(parameter);
				}
				catch (OperationCanceledException e)
				{
					// Rethrow if the exception does not come from the current cancellation token
					if (!hideCanceledException || e.CancellationToken != this.CancelToken)
					{
						throw;
					}
				}
			}
		}
		finally
		{
			if (started)
			{
				lock (this.syncRoot)
				{
					this.concurrentExecutions--;
					if (this.concurrentExecutions == 0)
					{
						ClearCancellationTokenSource();
					}
				}
			}
		}
	}

	private void ClearCancellationTokenSource()
	{
		if (this.cts is null)
		{
			return;
		}

		this.cts.Dispose();
		this.cts = null;
	}

	private void InitCancellationTokenSource()
	{
		if (this.cts is not null)
		{
			return;
		}

		this.cts = new();
	}
}