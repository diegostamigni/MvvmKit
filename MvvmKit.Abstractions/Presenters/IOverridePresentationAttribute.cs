using MvvmKit.Abstractions.View;

namespace MvvmKit.Abstractions.Presenters;

public interface IOverridePresentationAttribute
{
	BasePresentationAttribute PresentationAttribute(ViewModelRequest request);
}