using MvvmKit.Abstractions.Threading;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Abstractions.View;

public interface IViewDispatcher : IMainThreadAsyncDispatcher
{
	Task<bool> ShowViewModelAsync(ViewModelRequest request);

	Task<bool> CloseViewModelAsync(IViewModel viewModel);
}