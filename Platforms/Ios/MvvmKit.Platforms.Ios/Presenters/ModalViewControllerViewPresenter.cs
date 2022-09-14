using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Presenters;

public class ModalViewControllerViewPresenter : ViewControllerViewPresenter, IModalViewControllerViewPresenter
{
	public override Task<bool> ShowAsync(ViewModelRequest request) => throw new NotImplementedException();

	public override Task<bool> CloseAsync(IViewModel viewModel) => throw new NotImplementedException();
}