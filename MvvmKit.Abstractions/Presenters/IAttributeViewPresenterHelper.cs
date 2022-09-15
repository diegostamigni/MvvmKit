using MvvmKit.Abstractions.View;

namespace MvvmKit.Abstractions.Presenters;

public interface IAttributeViewPresenterHelper
{
	BasePresentationAttribute GetPresentationAttribute(ViewModelRequest request);
}