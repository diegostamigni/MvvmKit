using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

public class ChildPresentationAttribute : BasePresentationAttribute
{
	public bool Animated { get; set; } = true;

	public ChildPresentationAttribute(Type viewModelType) : base(viewModelType)
	{
	}
}