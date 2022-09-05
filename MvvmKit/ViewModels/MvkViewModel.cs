using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.ViewModels;

public abstract class MvkViewModel : IMvkViewModel
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

public abstract class MvkViewModel<TParameter> : MvkViewModel, IMvkViewModel<TParameter>
	where TParameter : notnull
{
	public abstract void Prepare(TParameter parameter);
}

public abstract class MvkViewModel<TParameter, TResult> : MvkViewModelResult<TResult>,
	IMvkViewModel<TParameter, TResult>
	where TParameter : notnull
	where TResult : notnull
{
	public abstract void Prepare(TParameter parameter);
}
