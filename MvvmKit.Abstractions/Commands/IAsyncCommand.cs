namespace MvvmKit.Abstractions.Commands;

public interface IAsyncCommand: ICommand
{
	Task ExecuteAsync(object? parameter = null);

	void Cancel();
}

public interface IAsyncCommand<in TParameter>: ICommand<TParameter>
{
	Task ExecuteAsync(TParameter parameter);

	void Cancel();
}