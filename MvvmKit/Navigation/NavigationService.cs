using System.Reflection;
using MvvmKit.Abstractions.Navigation;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Navigation;

public class NavigationService : INavigationService
{
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
		CancellationToken cancellationToken = default) where TResult : class
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TParameter, TResult>(
		IViewModel<TParameter, TResult> viewModel,
		TParameter param,
		CancellationToken cancellationToken = default) where TParameter : notnull where TResult : class
	{
		throw new NotImplementedException();
	}

	public Task<bool> NavigateAsync(Type viewModelType, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
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
		CancellationToken cancellationToken = default) where TResult : class
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TParameter, TResult>(
		Type viewModelType,
		TParameter param,
		CancellationToken cancellationToken = default) where TParameter : notnull where TResult : class
	{
		throw new NotImplementedException();
	}

	public Task<bool> NavigateAsync(string path, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<bool> NavigateAsync<TParameter>(
		string path,
		TParameter param,
		CancellationToken cancellationToken = default) where TParameter : notnull
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TResult>(
		string path,
		CancellationToken cancellationToken = default) where TResult : class
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TParameter, TResult>(
		string path,
		TParameter param,
		CancellationToken cancellationToken = default) where TParameter : notnull where TResult : class
	{
		throw new NotImplementedException();
	}

	public Task<bool> NavigateAsync<TViewModel>(CancellationToken cancellationToken = default)
		where TViewModel : IViewModel
	{
		throw new NotImplementedException();
	}

	public Task<bool> NavigateAsync<TViewModel, TParameter>(
		TParameter param,
		CancellationToken cancellationToken = default)
		where TViewModel : IViewModel<TParameter> where TParameter : notnull
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TViewModel, TResult>(
		CancellationToken cancellationToken = default)
		where TViewModel : IViewModelResult<TResult> where TResult : class
	{
		throw new NotImplementedException();
	}

	public Task<TResult?> NavigateAsync<TViewModel, TParameter, TResult>(
		TParameter param,
		CancellationToken cancellationToken = default)
		where TViewModel : IViewModel<TParameter, TResult> where TParameter : notnull where TResult : class
	{
		throw new NotImplementedException();
	}

	public Task<bool> CanNavigateAsync(string path)
	{
		throw new NotImplementedException();
	}

	public Task<bool> CanNavigateAsync<TViewModel>() where TViewModel : IViewModel
	{
		throw new NotImplementedException();
	}

	public Task<bool> CanNavigateAsync(Type viewModelType)
	{
		throw new NotImplementedException();
	}

	public Task<bool> CloseAsync(IViewModel viewModel, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<bool> CloseAsync<TResult>(
		IViewModelResult<TResult> viewModel,
		TResult? result,
		CancellationToken cancellationToken = default) where TResult : class
	{
		throw new NotImplementedException();
	}
}