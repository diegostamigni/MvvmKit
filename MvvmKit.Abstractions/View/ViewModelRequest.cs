using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Abstractions.View;

public class ViewModelRequest
{
	public Type? ViewModelType { get; set; }

	public IDictionary<string, string>? ParameterValues { get; set; }

	public IDictionary<string, string>? PresentationValues { get; set; }

	public ViewModelRequest(Type viewModelType)
	{
		this.ViewModelType = viewModelType;
	}

	public static ViewModelRequest GetDefaultRequest(Type viewModelType) => new(viewModelType);
}

public class ViewModelRequest<TViewModel> : ViewModelRequest where TViewModel : IViewModel
{
	public ViewModelRequest()
		: base(typeof(TViewModel))
	{
	}

	public ViewModelRequest GetDefaultRequest() => GetDefaultRequest(typeof(TViewModel));
}