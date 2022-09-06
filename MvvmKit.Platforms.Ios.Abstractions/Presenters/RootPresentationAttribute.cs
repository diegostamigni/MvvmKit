using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

public class RootPresentationAttribute : BasePresentationAttribute
{
	public const float DefaultAnimationDuration = 1.0f;
	public float AnimationDuration { get; set; } = DefaultAnimationDuration;

	public const UIViewAnimationOptions DefaultAnimationOptions = UIViewAnimationOptions.TransitionNone;
	public UIViewAnimationOptions AnimationOptions { get; set; } = DefaultAnimationOptions;

	public const bool DefaultWrapInNavigationController = false;
	public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
}