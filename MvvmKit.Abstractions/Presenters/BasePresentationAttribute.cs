namespace MvvmKit.Abstractions.Presenters;

[AttributeUsage(AttributeTargets.Class)]
public abstract class BasePresentationAttribute : Attribute, IPresentationAttribute
{
	/// <inheritdoc />
	public Type? ViewModelType { get; set; }

	/// <inheritdoc />
	public Type? ViewType { get; set; }
}