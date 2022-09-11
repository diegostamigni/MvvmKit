using MvvmKit.Abstractions.Navigation;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.ViewModels;

public abstract class NavigationViewModel : ViewModel
{
	protected NavigationViewModel(INavigationService navigationService)
	{
		this.NavigationService = navigationService;
	}

	protected virtual INavigationService NavigationService { get; }
}

public abstract class NavigationViewModel<TParameter> : NavigationViewModel, IViewModel<TParameter>
	where TParameter : class
{
	protected NavigationViewModel(INavigationService navigationService)
		: base(navigationService)
	{
	}

	public abstract void Prepare(TParameter parameter);
}

public abstract class NavigationViewModel<TParameter, TResult> : NavigationViewModelResult<TResult>,
	IViewModel<TParameter, TResult>
	where TParameter : class
	where TResult : class
{
	protected NavigationViewModel(INavigationService navigationService)
		: base(navigationService)
	{
	}

	public abstract void Prepare(TParameter parameter);
}