using System.Reflection;
using System.Runtime.CompilerServices;
using MvvmKit.Abstractions.Navigation;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Enums.Navigation;
using MvvmKit.ViewModels;

namespace MvvmKit.Navigation;

public class NavigationService : INavigationService
{
	private ConditionalWeakTable<IViewModel, TaskCompletionSource<object?>> taskCompletionResults = new();

	private readonly IViewDispatcher viewDispatcher;
	private readonly IViewModelLoader viewModelLoader;
	private readonly IViewsContainer viewsContainer;

	public NavigationService(
		IViewModelLoader viewModelLoader,
		IViewDispatcher viewDispatcher,
		IViewsContainer viewsContainer)
	{
		this.viewModelLoader = viewModelLoader;
		this.viewDispatcher = viewDispatcher;
		this.viewsContainer = viewsContainer;
	}

	public event EventHandler<NavigateArgs>? WillNavigate;

	public event EventHandler<NavigateArgs>? DidNavigate;

	public event EventHandler<NavigateArgs>? WillClose;

	public event EventHandler<NavigateArgs>? DidClose;

	public void LoadRoutes(IEnumerable<Assembly> assemblies)
	{
		throw new NotImplementedException();
	}

	public Task<bool> NavigateAsync(IViewModel viewModel, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<bool> NavigateAsync<TParameter>(
		IViewModel<TParameter> viewModel,
		TParameter param,
		CancellationToken cancellationToken = default) where TParameter : notnull
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TResult>(
		IViewModelResult<TResult> viewModel,
		CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TParameter, TResult>(
		IViewModel<TParameter, TResult> viewModel,
		TParameter param,
		CancellationToken cancellationToken = default) where TParameter : notnull
	{
		throw new NotImplementedException();
	}

	public Task<bool> NavigateAsync(Type viewModelType, CancellationToken cancellationToken = default)
	{
		var request = new ViewModelInstanceRequest(viewModelType);

		request.ViewModelInstance = this.viewModelLoader.LoadViewModel(request);

		return NavigateAsync(request, request.ViewModelInstance, cancellationToken);
	}

	public Task<bool> NavigateAsync<TParameter>(
		Type viewModelType,
		TParameter param,
		CancellationToken cancellationToken = default) where TParameter : notnull
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TResult>(
		Type viewModelType,
		CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TParameter, TResult>(
		Type viewModelType,
		TParameter param,
		CancellationToken cancellationToken = default) where TParameter : notnull
	{
		throw new NotImplementedException();
	}

	public Task<bool> NavigateAsync<TViewModel>(CancellationToken cancellationToken = default)
		where TViewModel : IViewModel => NavigateAsync(typeof(TViewModel), cancellationToken);

	public Task<bool> NavigateAsync<TViewModel, TParameter>(
		TParameter param,
		CancellationToken cancellationToken = default)
		where TViewModel : IViewModel<TParameter> where TParameter : notnull
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TViewModel, TResult>(
		CancellationToken cancellationToken = default)
		where TViewModel : IViewModelResult<TResult>
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TViewModel, TParameter, TResult>(
		TParameter param,
		CancellationToken cancellationToken = default)
		where TViewModel : IViewModel<TParameter, TResult> where TParameter : notnull
	{
		throw new NotImplementedException();
	}

	public virtual Task<bool> CanNavigateAsync<TViewModel>() where TViewModel : IViewModel
		=> Task.FromResult(this.viewsContainer.GetViewType(typeof(TViewModel)) is not null);

	public virtual Task<bool> CanNavigateAsync(Type viewModelType)
		=> Task.FromResult(this.viewsContainer.GetViewType(viewModelType) is not null);

	public virtual async Task<bool> CloseAsync(IViewModel viewModel, CancellationToken cancellationToken = default)
	{
		var args = new NavigateArgs
		{
			ViewModel = viewModel,
			Mode = NavigationMode.Close,
			CancellationToken = cancellationToken
		};

		this.WillClose?.Invoke(this, args);

		if (args.Cancel == true)
		{
			return false;
		}

		var close = await this.viewDispatcher.CloseViewModelAsync(viewModel);

		this.DidClose?.Invoke(this, args);

		return close;
	}

	public virtual async Task<bool> CloseAsync<TResult>(
		IViewModelResult<TResult> viewModel,
		TResult? result,
		CancellationToken cancellationToken = default)
	{
		this.taskCompletionResults.TryGetValue(viewModel, out var tcs);

		// Disable cancellation of the Task when closing ViewModel through the service
		viewModel.CloseCompletionSource = null;

		try
		{
			var closeResult = await CloseAsync(viewModel, cancellationToken);
			if (closeResult)
			{
				tcs?.TrySetResult(result);
				this.taskCompletionResults.Remove(viewModel);
			}
			else
			{
				viewModel.CloseCompletionSource = tcs;
			}

			return closeResult;
		}
		catch (Exception ex)
		{
			tcs?.TrySetException(ex);
			return false;
		}
	}

	protected virtual async Task<bool> NavigateAsync(
		ViewModelRequest request,
		IViewModel viewModel,
		CancellationToken cancellationToken = default)
	{
		var args = new NavigateArgs()
		{
			ViewModel = viewModel,
			ViewModelRequest = request,
			Mode = NavigationMode.Show,
			CancellationToken = cancellationToken
		};

		this.WillNavigate?.Invoke(this, args);

		if (args.Cancel == true)
		{
			return false;
		}

		var hasNavigated = await this.viewDispatcher.ShowViewModelAsync(request);

		await viewModel.InitializeAsync();

		this.DidNavigate?.Invoke(this, args);

		return hasNavigated;
	}
}