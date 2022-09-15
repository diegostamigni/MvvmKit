using MvvmKit.Abstractions.Presenters;

namespace MvvmKit.Abstractions.View;

public interface IViewPresenterResolver
{
	IViewPresenter Resolve(ViewModelRequest request);
}