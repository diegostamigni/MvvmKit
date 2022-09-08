using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Platforms.Ios.Abstractions.Views;

public interface IIosView : IView, ICanCreateIosView
{
	ViewModelRequest? Request { get; set; }
}

public interface IIosView<TViewModel> : IIosView, IView<TViewModel>
	where TViewModel : class, IViewModel
{
}