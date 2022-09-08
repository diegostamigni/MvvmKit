using MvvmKit.Abstractions.Navigation;
using MvvmKit.Abstractions.View;

namespace MvvmKit.Abstractions.ViewModels;

public interface IViewModelLoader
{
	IViewModel LoadViewModel(ViewModelRequest request, NavigateArgs? navigationArgs = null);

	IViewModel LoadViewModel<TParameter>(
		ViewModelRequest request,
		TParameter param,
		NavigateArgs? navigationArgs = null);

	IViewModel ReloadViewModel(
		IViewModel viewModel,
		ViewModelRequest request,
		NavigateArgs? navigationArgs = null);

	IViewModel ReloadViewModel<TParameter>(
		IViewModel<TParameter> viewModel,
		TParameter param,
		ViewModelRequest request,
		NavigateArgs? navigationArgs = null);
}
