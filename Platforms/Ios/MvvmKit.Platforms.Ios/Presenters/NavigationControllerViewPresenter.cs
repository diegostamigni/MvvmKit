using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Abstractions.Views;

namespace MvvmKit.Platforms.Ios.Presenters;

public class NavigationControllerViewPresenter
	: ViewControllerViewPresenter<NavigationPresentationAttribute>, INavigationControllerViewPresenter
{
	public NavigationControllerViewPresenter(
		IIosViewCreator viewCreator,
		IAttributeViewPresenterHelper attributeViewPresenterHelper) : base(viewCreator, attributeViewPresenterHelper)
	{
	}

	protected override Task<bool> HandleShowAsync(UIViewController view, NavigationPresentationAttribute presentationAttribute)
		=> Task.FromResult(true);

	protected override Task<bool> HandleCloseAsync(IViewModel viewModel, NavigationPresentationAttribute presentationAttribute)
		=> Task.FromResult(false);
}