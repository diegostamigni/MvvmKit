using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Abstractions.Views;

namespace MvvmKit.Platforms.Ios.Presenters;

public class ModalViewControllerViewPresenter
	: ViewControllerViewPresenter<ModalPresentationAttribute>, IModalViewControllerViewPresenter
{
	public ModalViewControllerViewPresenter(
		IIosViewCreator viewCreator,
		IAttributeViewPresenterHelper attributeViewPresenterHelper) : base(viewCreator, attributeViewPresenterHelper)
	{
	}

	protected override Task<bool> HandleShowAsync(UIViewController view, ModalPresentationAttribute presentationAttribute)
		=> throw new NotImplementedException();

	protected override Task<bool> HandleCloseAsync(IViewModel viewModel, ModalPresentationAttribute presentationAttribute)
		=> throw new NotImplementedException();
}