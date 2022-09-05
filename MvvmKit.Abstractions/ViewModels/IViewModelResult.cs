namespace MvvmKit.Abstractions.ViewModels;

public interface IViewModelResult<TResult> : IViewModel
{
	TaskCompletionSource<object?>? CloseCompletionSource { get; set; }
}