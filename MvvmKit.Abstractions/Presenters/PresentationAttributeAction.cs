using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Abstractions.Presenters;

public class PresentationAttributeAction
{
	public Func<Type, IPresentationAttribute, ViewModelRequest, Task<bool>>? ShowAction { get; set; }

	public Func<IViewModel, IPresentationAttribute, Task<bool>>? CloseAction { get; set; }
}