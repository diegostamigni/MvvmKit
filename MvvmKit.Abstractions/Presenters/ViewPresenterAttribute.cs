namespace MvvmKit.Abstractions.Presenters;

[AttributeUsage(AttributeTargets.Class)]
public class ViewPresenterAttribute : Attribute
{
	public Type PresenterType { get; }

	public ViewPresenterAttribute(Type presenterType)
	{
		this.PresenterType = presenterType;
	}
}