using MvvmKit.Abstractions.Navigation;
using MvvmKit.ViewModels;

namespace HelloMvvmKit.Core.ViewModels;

public class HomeViewModel : NavigationViewModel
{
	public HomeViewModel(INavigationService navigationService)
		: base(navigationService)
	{
	}
}