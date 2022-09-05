namespace MvvmKit.Abstractions.ViewModels;

public interface IMvkViewModelResult<TResult> : IMvkViewModel
{
	TaskCompletionSource<object?>? CloseCompletionSource { get; set; }
}