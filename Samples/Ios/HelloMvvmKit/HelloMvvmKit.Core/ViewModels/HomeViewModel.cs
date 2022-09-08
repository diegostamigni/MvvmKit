using MvvmKit.Abstractions.Commands;
using MvvmKit.Abstractions.Navigation;
using MvvmKit.Commands;
using MvvmKit.ViewModels;

namespace HelloMvvmKit.Core.ViewModels;

public class HomeViewModel : NavigationViewModel
{
	public ICommand NavigateNextCommand { get; }

	public HomeViewModel(INavigationService navigationService)
		: base(navigationService)
	{
		this.NavigateNextCommand = new AsyncCommand(NavigateNextAsync, CanNextAsync);
	}

	private static bool CanNextAsync() => true;

	private Task NavigateNextAsync(CancellationToken token)
		=> this.NavigationService.NavigateAsync<FirstChildViewModel>(token);
}