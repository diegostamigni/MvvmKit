using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

[ViewPresenter(typeof(ITabBarViewControllerViewPresenter))]
public class TabPresentationAttribute : BasePresentationAttribute
{
	public string? TabName { get; set; }

	public string? TabIconName { get; set; }

	public string? TabSelectedIconName { get; set; }

	public const bool DefaultWrapInNavigationController = true;
	public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

	public string? TabAccessibilityIdentifier { get; set; }

	public TabPresentationAttribute(Type viewModelType) : base(viewModelType)
	{
	}
}