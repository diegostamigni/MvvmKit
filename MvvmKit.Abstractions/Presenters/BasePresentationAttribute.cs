namespace MvvmKit.Abstractions.Presenters;

[AttributeUsage(AttributeTargets.Class)]
public abstract class BasePresentationAttribute : Attribute, IPresentation
{
	/// <inheritdoc />
	public Type ViewModelType { get; }

	/// <inheritdoc />
	public Type? ViewType { get; set; }

	protected BasePresentationAttribute(Type viewModelType)
	{
		this.ViewModelType = viewModelType;
	}
}