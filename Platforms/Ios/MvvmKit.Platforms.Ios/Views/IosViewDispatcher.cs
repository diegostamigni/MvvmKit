using MvvmKit.Abstractions.View;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Threading;

namespace MvvmKit.Platforms.Ios.Views;

public class IosViewDispatcher : IosUIThreadDispatcher, IViewDispatcher
{
	private readonly IIosViewPresenter presenter;

	public IosViewDispatcher(IIosViewPresenter presenter)
	{
		this.presenter = presenter;
	}

	public async Task<bool> ShowViewModelAsync(ViewModelRequest request)
	{
		Task Action() => this.presenter.ShowAsync(request);

		await ExecuteOnMainThreadAsync(Action);

		return true;
	}
}