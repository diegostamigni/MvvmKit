using System.Reflection;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Abstractions.Navigation;

public interface INavigationService
{
	event EventHandler<NavigateArgs>? WillNavigate;

	/// <summary>
	/// Event that triggers right after navigation did occur
	/// </summary>
	event EventHandler<NavigateArgs>? DidNavigate;

	/// <summary>
	/// Event that triggers right before Closing
	/// </summary>
	event EventHandler<NavigateArgs>? WillClose;

	/// <summary>
	/// Event that triggers right after did happen
	/// </summary>
	event EventHandler<NavigateArgs>? DidClose;

	/// <summary>
	/// Loads all navigation routes based on the referenced assemblies
	/// </summary>
	/// <param name="assemblies">The assemblies that should be indexed for routes</param>
	void LoadRoutes(IEnumerable<Assembly> assemblies);

	/// <summary>
	/// Navigates to an instance of a ViewModel
	/// </summary>
	/// <param name="viewModel">ViewModel to navigate to</param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <returns>Boolean indicating successful navigation</returns>
	Task<bool> NavigateAsync(IViewModel viewModel, CancellationToken cancellationToken = default);

	/// <summary>
	/// Navigates to an instance of a ViewModel and passes TParameter
	/// </summary>
	/// <typeparam name="TParameter"></typeparam>
	/// <param name="viewModel">ViewModel to navigate to</param>
	/// <param name="param">ViewModel parameter</param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <returns>Boolean indicating successful navigation</returns>
	Task<bool> NavigateAsync<TParameter>(
		IViewModel<TParameter> viewModel,
		TParameter param,
		CancellationToken cancellationToken = default) where TParameter : notnull;

	/// <summary>
	/// Navigates to an instance of a ViewModel and returns TResult
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="viewModel"></param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <returns></returns>
	Task<TResult?> NavigateAsync<TResult>(
		IViewModelResult<TResult> viewModel,
		CancellationToken cancellationToken = default) where TResult : class;

	/// <summary>
	/// Navigates to an instance of a ViewModel passes TParameter and returns TResult
	/// </summary>
	/// <typeparam name="TParameter"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="viewModel"></param>
	/// <param name="param"></param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <returns></returns>
	Task<TResult?> NavigateAsync<TParameter, TResult>(
		IViewModel<TParameter, TResult> viewModel,
		TParameter param,
		CancellationToken cancellationToken = default)
		where TParameter : notnull
		where TResult : class;

	/// <summary>
	/// Navigates to a ViewModel Type
	/// </summary>
	/// <param name="viewModelType"></param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <returns>Boolean indicating successful navigation</returns>
	Task<bool> NavigateAsync(Type viewModelType, CancellationToken cancellationToken = default);

	/// <summary>
	/// Navigates to a ViewModel Type and passes TParameter
	/// </summary>
	/// <typeparam name="TParameter"></typeparam>
	/// <param name="viewModelType"></param>
	/// <param name="param"></param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <returns>Boolean indicating successful navigation</returns>
	Task<bool> NavigateAsync<TParameter>(
		Type viewModelType,
		TParameter param,
		CancellationToken cancellationToken = default)
		where TParameter : notnull;

	/// <summary>
	/// Navigates to a ViewModel Type passes and returns TResult
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="viewModelType"></param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <returns></returns>
	Task<TResult?> NavigateAsync<TResult>(Type viewModelType, CancellationToken cancellationToken = default)
		where TResult : class;

	/// <summary>
	/// Navigates to a ViewModel Type passes TParameter and returns TResult
	/// </summary>
	/// <typeparam name="TParameter"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="viewModelType"></param>
	/// <param name="param"></param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <returns></returns>
	Task<TResult?> NavigateAsync<TParameter, TResult>(
		Type viewModelType,
		TParameter param,
		CancellationToken cancellationToken = default)
		where TParameter : notnull
		where TResult : class;

	/// <summary>
	/// Navigate to a ViewModel determined by its type
	/// </summary>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <typeparam name="TViewModel">Type of <see cref="IViewModel"/></typeparam>
	/// <returns>Boolean indicating successful navigation</returns>
	Task<bool> NavigateAsync<TViewModel>(CancellationToken cancellationToken = default)
		where TViewModel : IViewModel;

	/// <summary>
	/// Navigate to a ViewModel determined by its type, with parameter
	/// </summary>
	/// <param name="param">ViewModel parameter</param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <typeparam name="TViewModel">Type of <see cref="IViewModel{TParameter}"/></typeparam>
	/// <typeparam name="TParameter">Parameter passed to ViewModel</typeparam>
	/// <returns>Boolean indicating successful navigation</returns>
	Task<bool> NavigateAsync<TViewModel, TParameter>(TParameter param, CancellationToken cancellationToken = default)
		where TViewModel : IViewModel<TParameter>
		where TParameter : notnull;

	/// <summary>
	/// Navigate to a ViewModel determined by its type, which returns a result.
	/// </summary>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <typeparam name="TViewModel">Type of <see cref="IViewModel"/></typeparam>
	/// <typeparam name="TResult">Result from the ViewModel</typeparam>
	/// <returns>Returns a <see cref="Task{Task}"/> with <see cref="TResult"/></returns>
	Task<TResult?> NavigateAsync<TViewModel, TResult>(CancellationToken cancellationToken = default)
		where TViewModel : IViewModelResult<TResult>
		where TResult : class;

	/// <summary>
	/// Navigate to a ViewModel determined by its type, with parameter and which returns a result.
	/// </summary>
	/// <param name="param">ViewModel parameter</param>
	/// <param name="cancellationToken">CancellationToken to cancel the navigation</param>
	/// <typeparam name="TViewModel">Type of <see cref="IViewModel{TParameter,TResult}"/></typeparam>
	/// <typeparam name="TParameter">Parameter passed to ViewModel</typeparam>
	/// <typeparam name="TResult">Result from the ViewModel</typeparam>
	/// <returns>Returns a <see cref="Task{Task}"/> with <see cref="TResult"/></returns>
	Task<TResult?> NavigateAsync<TViewModel, TParameter, TResult>(
		TParameter param,
		CancellationToken cancellationToken = default)
		where TViewModel : IViewModel<TParameter, TResult>
		where TParameter : notnull
		where TResult : class;

	/// <summary>
	/// Verifies if the provided viewmodel is available
	/// </summary>
	/// <returns>True if the ViewModel is available</returns>
	Task<bool> CanNavigateAsync<TViewModel>() where TViewModel : IViewModel;

	/// <summary>
	/// Verifies if the provided viewmodel is available
	/// </summary>
	/// <param name="viewModelType">ViewModel type to check</param>
	/// <returns>True if the ViewModel is available</returns>
	Task<bool> CanNavigateAsync(Type viewModelType);

	/// <summary>
	/// Closes the View attached to the ViewModel
	/// </summary>
	/// <param name="viewModel"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> CloseAsync(IViewModel viewModel, CancellationToken cancellationToken = default);

	/// <summary>
	/// Closes the View attached to the ViewModel and returns a result to the underlying ViewModel
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="viewModel"></param>
	/// <param name="result"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> CloseAsync<TResult>(IViewModelResult<TResult> viewModel, TResult? result,
		CancellationToken cancellationToken = default)
		where TResult : class;
}