using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

[ViewPresenter(typeof(ITabBarViewControllerViewPresenter))]
public class TabPresentationAttribute : BasePresentationAttribute, IHasWrapInNavigationController
{
	public string? TabName { get; set; }

	public string? TabIconName { get; set; }

	public string? TabSelectedIconName { get; set; }

	public bool WrapInNavigationController { get; set; } = true;

	public string? TabAccessibilityIdentifier { get; set; }

	public TabPresentationAttribute(Type viewModelType) : base(viewModelType)
	{
	}
}