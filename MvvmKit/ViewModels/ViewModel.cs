using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.ViewModels;

public abstract class ViewModel : IViewModel
{
	public virtual void ViewCreated()
	{
	}

	public virtual void ViewAppearing()
	{
	}

	public virtual void ViewAppeared()
	{
	}

	public virtual void ViewDisappearing()
	{
	}

	public virtual void ViewDisappeared()
	{
	}

	public virtual void ViewDestroy(bool viewFinishing = true)
	{
	}

	public virtual void Prepare()
	{
	}

	public virtual Task InitializeAsync() => Task.FromResult(true);
}

public abstract class ViewModel<TParameter> : ViewModel, IViewModel<TParameter>
	where TParameter : notnull
{
	public abstract void Prepare(TParameter parameter);
}

public abstract class ViewModel<TParameter, TResult> : ViewModelResult<TResult>,
	IViewModel<TParameter, TResult>
	where TParameter : notnull
	where TResult : notnull
{
	public abstract void Prepare(TParameter parameter);
}