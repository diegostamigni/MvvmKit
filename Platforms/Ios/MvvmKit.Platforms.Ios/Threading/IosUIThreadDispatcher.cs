using MvvmKit.Threading;

namespace MvvmKit.Platforms.Ios.Threading;

// ReSharper disable once InconsistentNaming
public abstract class IosUIThreadDispatcher : MainThreadAsyncDispatcher
{
	private readonly SynchronizationContext? uiSynchronizationContext;

	protected IosUIThreadDispatcher()
	{
		this.uiSynchronizationContext = SynchronizationContext.Current;

		if (this.uiSynchronizationContext == null)
		{
			throw new InvalidOperationException(
				"SynchronizationContext must not be null - check to make sure Dispatcher is created on UI thread");
		}
	}

	public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
	{
		if (this.IsOnMainThread)
		{
			ExceptionMaskedAction(action, maskExceptions);
		}
		else
		{
			UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
			{
				ExceptionMaskedAction(action, maskExceptions);
			});
		}

		return true;
	}

	public override bool IsOnMainThread => this.uiSynchronizationContext == SynchronizationContext.Current;
}