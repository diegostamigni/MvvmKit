using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Abstractions.Views;

namespace MvvmKit.Platforms.Ios.Presenters;

public class TabBarViewControllerViewPresenter
	: ViewControllerViewPresenter<TabPresentationAttribute>, ITabBarViewControllerViewPresenter
{
	public TabBarViewControllerViewPresenter(
		IIosViewCreator viewCreator,
		IAttributeViewPresenterHelper attributeViewPresenterHelper) : base(viewCreator, attributeViewPresenterHelper)
	{
	}

	protected override Task<bool> HandleShowAsync(UIViewController view, TabPresentationAttribute presentationAttribute)
		=> throw new NotImplementedException();

	protected override Task<bool> HandleCloseAsync(IViewModel viewModel, TabPresentationAttribute presentationAttribute)
		=> throw new NotImplementedException();
}