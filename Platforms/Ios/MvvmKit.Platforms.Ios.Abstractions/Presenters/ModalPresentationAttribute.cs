using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

[ViewPresenter(typeof(IModalViewControllerViewPresenter))]
public class ModalPresentationAttribute : BasePresentationAttribute, IHasWrapInNavigationController
{
	public bool WrapInNavigationController { get; set; }

	public UIModalPresentationStyle ModalPresentationStyle { get; set; } = UIModalPresentationStyle.FullScreen;

	public UIModalTransitionStyle ModalTransitionStyle { get; set; } = UIModalTransitionStyle.CoverVertical;

	public CGSize PreferredContentSize { get; set; } = CGSize.Empty;

	public bool Animated { get; set; } = true;

	public ModalPresentationAttribute(Type viewModelType) : base(viewModelType)
	{
	}
}