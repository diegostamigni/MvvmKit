using MvvmKit.Abstractions.Threading;

namespace MvvmKit.Abstractions.View;

public interface IViewDispatcher : IMainThreadAsyncDispatcher
{
	Task<bool> ShowViewModelAsync(ViewModelRequest request);
}