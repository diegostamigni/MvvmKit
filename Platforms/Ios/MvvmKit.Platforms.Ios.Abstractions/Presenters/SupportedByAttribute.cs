namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SupportedByAttribute : Attribute
{
	public UIUserInterfaceIdiom UserInterfaceIdiom { get; }

	public SupportedByAttribute(UIUserInterfaceIdiom userInterfaceIdiom)
		=> this.UserInterfaceIdiom = userInterfaceIdiom;
}