using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.ViewModels;

public class ViewModelInstanceRequest : ViewModelRequest
{
	public IViewModel? ViewModelInstance { get; set; }

	public ViewModelInstanceRequest(Type viewModelType)
		: base(viewModelType)
	{
	}

	public ViewModelInstanceRequest(IViewModel viewModelInstance)
		: base(viewModelInstance.GetType())
	{
		this.ViewModelInstance = viewModelInstance;
	}
}