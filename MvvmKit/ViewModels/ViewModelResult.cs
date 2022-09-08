using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.ViewModels;

public abstract class ViewModelResult<TResult> : ViewModel, IViewModelResult<TResult>
	where TResult : notnull
{
	public TaskCompletionSource<object?>? CloseCompletionSource { get; set; }

	public override void ViewDestroy(bool viewFinishing = true)
	{
		if (viewFinishing && this.CloseCompletionSource is not null &&
		    !this.CloseCompletionSource.Task.IsCompleted &&
		    !this.CloseCompletionSource.Task.IsFaulted)
		{
			this.CloseCompletionSource.TrySetCanceled();
		}

		base.ViewDestroy(viewFinishing);
	}
}