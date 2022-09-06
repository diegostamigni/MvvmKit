using MvvmKit.Abstractions.Base;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Abstractions.View;

public interface IView : IDataConsumer
{
	IViewModel? ViewModel { get; set; }
}

public interface IView<TViewModel> : IView
	where TViewModel : class, IViewModel
{
	new TViewModel? ViewModel { get; set; }
}