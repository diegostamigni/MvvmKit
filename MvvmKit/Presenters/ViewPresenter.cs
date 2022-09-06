using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Presenters;

public abstract class ViewPresenter : IViewPresenter
{
	public abstract Task<bool> ShowAsync(ViewModelRequest request);

	public abstract Task<bool> CloseAsync(IViewModel viewModel);
}