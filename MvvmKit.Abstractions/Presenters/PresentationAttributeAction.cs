using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Abstractions.Presenters;

public class PresentationAttributeAction
{
	public Func<Type, IPresentation, ViewModelRequest, Task<bool>>? ShowAction { get; set; }

	public Func<IViewModel, IPresentation, Task<bool>>? CloseAction { get; set; }
}