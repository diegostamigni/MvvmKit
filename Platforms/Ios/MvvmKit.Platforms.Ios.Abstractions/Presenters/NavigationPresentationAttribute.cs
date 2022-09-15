using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

[ViewPresenter(typeof(INavigationControllerViewPresenter))]
public class NavigationPresentationAttribute : BasePresentationAttribute
{
	public float AnimationDuration { get; set; } = 1.0f;

	public UIViewAnimationOptions AnimationOptions { get; set; } = UIViewAnimationOptions.TransitionNone;

	public bool WrapInNavigationController { get; set; }

	public NavigationPresentationAttribute(Type viewModelType) : base(viewModelType)
	{
	}
}