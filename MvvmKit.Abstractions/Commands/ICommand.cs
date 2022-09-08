namespace MvvmKit.Abstractions.Commands;

public interface ICommand
{
	void Execute();

	bool CanExecute();
}

public interface ICommand<in TParameter>
{
	void Execute(TParameter parameter);

	bool CanExecute();
}