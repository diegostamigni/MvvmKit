namespace MvvmKit.Abstractions.Presenters;

public interface IPresentationAttribute
{
	/// <summary>
	/// That shall be used only if you are using non generic views.
	/// </summary>
	Type? ViewModelType { get; set; }

	/// <summary>
	/// Type of the view
	/// </summary>
	Type? ViewType { get; set; }
}