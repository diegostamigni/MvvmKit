using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.ViewModels;

public abstract class MvkViewModelResult<TResult> : MvkViewModel, IMvkViewModelResult<TResult>
	where TResult : notnull
{
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