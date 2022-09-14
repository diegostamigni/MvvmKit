using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Threading;

namespace MvvmKit.Platforms.Ios.Views;

public class IosViewDispatcher : IosUIThreadDispatcher, IViewDispatcher
{
	private readonly IViewPresenterResolver viewPresenterResolver;

	public IosViewDispatcher(IViewPresenterResolver viewPresenterResolver)
		=> this.viewPresenterResolver = viewPresenterResolver;

	public async Task<bool> ShowViewModelAsync(ViewModelRequest request)
	{
		var presenter = this.viewPresenterResolver.Resolve(request);

		Task Action() => presenter.ShowAsync(request);

		await ExecuteOnMainThreadAsync(Action);

		return true;
	}

	public async Task<bool> CloseViewModelAsync(IViewModel viewModel)
	{
		var presenter = this.viewPresenterResolver.Resolve(new(viewModel.GetType()));

		Task Action() => presenter.CloseAsync(viewModel);

		await ExecuteOnMainThreadAsync(Action);

		return true;
	}
}