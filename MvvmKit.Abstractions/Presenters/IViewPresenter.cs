using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Abstractions.Presenters;

public interface IViewPresenter
{
	Task<bool> ShowAsync(ViewModelRequest request);

	Task<bool> CloseAsync(IViewModel viewModel);
}