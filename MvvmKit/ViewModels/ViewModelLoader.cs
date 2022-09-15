using Autofac;
using MvvmKit.Abstractions.Navigation;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.ViewModels;

public class ViewModelLoader : IViewModelLoader
{
	private readonly ILifetimeScope lifetimeScope;

	public ViewModelLoader(ILifetimeScope lifetimeScope)
	{
		this.lifetimeScope = lifetimeScope;
	}

	public IViewModel LoadViewModel(
		ViewModelRequest request,
		NavigateArgs? navigationArgs = null)
	{
		if (request.ViewModelType is null)
		{
			throw new ArgumentNullException(nameof(request), $"{nameof(request.ViewModelType)} is null");
		}

		var viewModel = (IViewModel)this.lifetimeScope.Resolve(request.ViewModelType);

		RunViewModelLifecycle(viewModel, navigationArgs);

		return viewModel;
	}

	public IViewModel LoadViewModel<TParameter>(
		ViewModelRequest request,
		TParameter param,
		NavigateArgs? navigationArgs = null)
	{
		throw new NotImplementedException();
	}

	public IViewModel ReloadViewModel(
		IViewModel viewModel,
		ViewModelRequest request,
		NavigateArgs? navigationArgs = null)
	{
		throw new NotImplementedException();
	}

	public IViewModel ReloadViewModel<TParameter>(
		IViewModel<TParameter> viewModel,
		TParameter param,
		ViewModelRequest request,
		NavigateArgs? navigationArgs = null)
	{
		throw new NotImplementedException();
	}

	private static void RunViewModelLifecycle(IViewModel viewModel, NavigateArgs? navigationArgs)
	{
		if (viewModel == null)
		{
			throw new ArgumentNullException(nameof(viewModel));
		}

		try
		{
			viewModel.Start();
			if (navigationArgs?.Cancel == true)
			{
				return;
			}

			viewModel.Prepare();
		}
		catch (Exception exception)
		{
			throw new InvalidOperationException(
				$"Problem running viewModel lifecycle of type {viewModel.GetType().Name}", exception);
		}
	}

	private static void RunViewModelLifecycle<TParameter>(
		IViewModel<TParameter> viewModel,
		TParameter param,
		NavigateArgs? navigationArgs)
		where TParameter : notnull
	{
		if (viewModel == null)
		{
			throw new ArgumentNullException(nameof(viewModel));
		}

		try
		{
			viewModel.Start();
			if (navigationArgs?.Cancel == true)
			{
				return;
			}

			viewModel.Prepare();
			if (navigationArgs?.Cancel == true)
			{
				return;
			}

			viewModel.Prepare(param);
		}
		catch (Exception exception)
		{
			throw new InvalidOperationException(
				$"Problem running viewModel lifecycle of type {viewModel.GetType().Name}", exception);
		}
	}
}