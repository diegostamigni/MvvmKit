using MvvmKit.Abstractions.Navigation;
using MvvmKit.ViewModels;

namespace HelloMvvmKit.Core.ViewModels;

public class FirstChildViewModel : NavigationViewModel
{
	public FirstChildViewModel(INavigationService navigationService)
		: base(navigationService)
	{
	}
}