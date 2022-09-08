using MvvmKit.Abstractions.Commands;

namespace MvvmKit.Commands;

public class AsyncCommand : AsyncCommandBase, IAsyncCommand
{
	private readonly Func<CancellationToken, Task> execute;
	private readonly Func<bool>? canExecute;

	public AsyncCommand(
		Func<Task> execute,
		Func<bool>? canExecute = null,
		bool allowConcurrentExecutions = false) : base(allowConcurrentExecutions)
	{
		if (execute is null)
		{
			throw new ArgumentNullException(nameof(execute));
		}

		this.execute = _ => execute();
		this.canExecute = canExecute;
	}

	public AsyncCommand(
		Func<CancellationToken, Task> execute,
		Func<bool>? canExecute = null,
		bool allowConcurrentExecutions = false) : base(allowConcurrentExecutions)
	{
		this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
		this.canExecute = canExecute;
	}

	protected override bool CanExecuteImpl(object? parameter) => this.canExecute is null || this.canExecute();

	protected override Task ExecuteAsyncImpl(object? parameter) => this.execute(this.CancelToken);

	public static AsyncCommand<T> CreateCommand<T>(
		Func<T, Task> execute,
		Func<T, bool>? canExecute = null,
		bool allowConcurrentExecutions = false) => new(execute, canExecute, allowConcurrentExecutions);

	public static AsyncCommand<T> CreateCommand<T>(
		Func<T, CancellationToken, Task> execute,
		Func<T, bool>? canExecute = null,
		bool allowConcurrentExecutions = false) => new(execute, canExecute, allowConcurrentExecutions);

	public Task ExecuteAsync(object? parameter = null) => ExecuteAsync(parameter, false);
}
public class AsyncCommand<T>
	: AsyncCommandBase, ICommand, IAsyncCommand<T>
{
	private readonly Func<T, CancellationToken, Task> execute;
	private readonly Func<T, bool>? canExecute;

	public AsyncCommand(
		Func<T, Task> execute,
		Func<T, bool>? canExecute = null,
		bool allowConcurrentExecutions = false) : base(allowConcurrentExecutions)
	{
		if (execute is null)
		{
			throw new ArgumentNullException(nameof(execute));
		}

		this.execute = (p, _) => execute(p);
		this.canExecute = canExecute;
	}

	public AsyncCommand(
		Func<T, CancellationToken, Task> execute,
		Func<T, bool>? canExecute = null,
		bool allowConcurrentExecutions = false) : base(allowConcurrentExecutions)
	{
		this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
		this.canExecute = canExecute;
	}

	public Task ExecuteAsync(T parameter) => ExecuteAsync(parameter, false);

	public void Execute(T parameter) => base.Execute(parameter);

	public bool CanExecute(T parameter) => base.CanExecute(parameter);

	protected override bool CanExecuteImpl(object? parameter)
		=> this.canExecute is null || this.canExecute((T)(parameter ?? throw new InvalidOperationException()));

	protected override Task ExecuteAsyncImpl(object? parameter)
		=> this.execute((T)(parameter ?? throw new InvalidOperationException()), this.CancelToken);
}