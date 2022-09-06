using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

public interface IIosViewPresenter : IViewPresenter, ICanCreateIosView
{
	void ClosedPopoverViewController();

	Task<bool> ClosedModalViewControllerAsync(UIViewController viewController, ModalPresentationAttribute attribute);
}