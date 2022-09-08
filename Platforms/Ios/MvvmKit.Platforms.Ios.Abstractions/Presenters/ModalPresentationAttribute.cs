using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

public class ModalPresentationAttribute : BasePresentationAttribute
{
	public const bool DefaultWrapInNavigationController = false;
	public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

	public const UIModalPresentationStyle DefaultModalPresentationStyle = UIModalPresentationStyle.FullScreen;
	public UIModalPresentationStyle ModalPresentationStyle { get; set; } = DefaultModalPresentationStyle;

	public const UIModalTransitionStyle DefaultModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
	public UIModalTransitionStyle ModalTransitionStyle { get; set; } = DefaultModalTransitionStyle;

	public static readonly CGSize DefaultPreferredContentSize = CGSize.Empty;
	public CGSize PreferredContentSize { get; set; } = DefaultPreferredContentSize;

	public const bool DefaultAnimated = true;
	public bool Animated { get; set; } = DefaultAnimated;

	public ModalPresentationAttribute(Type viewModelType) : base(viewModelType)
	{
	}
}