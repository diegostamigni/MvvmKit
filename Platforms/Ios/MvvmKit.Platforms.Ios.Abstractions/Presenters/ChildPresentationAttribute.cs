using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

public class ChildPresentationAttribute : BasePresentationAttribute
{
	public const bool DefaultAnimated = true;
	public bool Animated { get; set; } = DefaultAnimated;
}