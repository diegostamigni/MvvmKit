using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

[ViewPresenter(typeof(INavigationControllerViewPresenter))]
public class NavigationPresentationAttribute : BasePresentationAttribute, IHasWrapInNavigationController
{
	public bool WrapInNavigationController { get; set; }

	public NavigationPresentationAttribute(Type viewModelType) : base(viewModelType)
	{
	}
}