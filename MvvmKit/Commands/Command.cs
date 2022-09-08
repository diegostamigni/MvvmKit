using MvvmKit.Abstractions.Commands;

namespace MvvmKit.Commands;

public class Command : CommandBase, ICommand
{
	private readonly Func<bool>? canExecute;
	private readonly Action execute;

	public Command(Action execute, Func<bool>? canExecute = null)
	{
		this.execute = execute;
		this.canExecute = canExecute;
	}

	public bool CanExecute(object? parameter) => this.canExecute is null || this.canExecute();

	public bool CanExecute() => CanExecute(null);

	public void Execute(object? parameter)
	{
		if (CanExecute(parameter))
		{
			this.execute();
		}
	}

	public void Execute() => Execute(null);
}

public class Command<T> : CommandBase, ICommand, ICommand<T>
{
	private readonly Func<T, bool>? canExecute;
	private readonly Action<T> execute;

	public Command(Action<T> execute, Func<T, bool>? canExecute = null)
	{
		this.execute = execute;
		this.canExecute = canExecute;
	}

	public bool CanExecute(object parameter)
		=> this.canExecute is null || this.canExecute((T)parameter);

	public bool CanExecute()
		=> CanExecute(default(T)!);

	public bool CanExecute(T parameter)
		=> this.canExecute is null || this.canExecute(parameter);

	public void Execute(object parameter)
	{
		if (!CanExecute(parameter))
		{
			return;
		}

		this.execute((T)parameter);
	}

	public void Execute() => Execute(default(T)!);

	public void Execute(T parameter)
	{
		if (!CanExecute(parameter))
		{
			return;
		}

		this.execute(parameter);
	}
}