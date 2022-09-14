namespace MvvmKit.Abstractions.ViewModels;

public interface IViewModel
{
	void ViewCreated();

	void ViewAppearing();

	void ViewAppeared();

	void ViewDisappearing();

	void ViewDisappeared();

	void ViewDestroy(bool viewFinishing = true);

	void Start();

	void Prepare();

	Task InitializeAsync();
}

public interface IViewModel<in TParameter> : IViewModel
{
	void Prepare(TParameter parameter);
}

public interface IViewModel<in TParameter, TResult> : IViewModel<TParameter>, IViewModelResult<TResult>
{
}