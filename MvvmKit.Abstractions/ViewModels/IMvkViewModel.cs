namespace MvvmKit.Abstractions.ViewModels;

public interface IMvkViewModel
{
	void ViewCreated();

	void ViewAppearing();

	void ViewAppeared();

	void ViewDisappearing();

	void ViewDisappeared();

	void ViewDestroy(bool viewFinishing = true);

	void Prepare();

	Task InitializeAsync();
}

public interface IMvkViewModel<in TParameter> : IMvkViewModel
{
	void Prepare(TParameter parameter);
}

public interface IMvkViewModel<in TParameter, TResult> : IMvkViewModel<TParameter>, IMvkViewModelResult<TResult>
{
}