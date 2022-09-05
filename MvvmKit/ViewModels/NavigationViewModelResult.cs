using MvvmKit.Abstractions.Navigation;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.ViewModels;

public abstract class NavigationViewModelResult<TResult> : NavigationViewModel, IViewModelResult<TResult>
	where TResult : class
{
	protected NavigationViewModelResult(INavigationService navigationService)
		: base(navigationService)
	{
	}

	public TaskCompletionSource<object?>? CloseCompletionSource { get; set; }

	public override void ViewDestroy(bool viewFinishing = true)
	{
		if (viewFinishing && this.CloseCompletionSource != null &&
		    !this.CloseCompletionSource.Task.IsCompleted &&
		    !this.CloseCompletionSource.Task.IsFaulted)
		{
			this.CloseCompletionSource.TrySetCanceled();
		}

		base.ViewDestroy(viewFinishing);
	}
}