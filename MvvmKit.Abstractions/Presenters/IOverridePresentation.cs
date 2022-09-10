using MvvmKit.Abstractions.View;

namespace MvvmKit.Abstractions.Presenters;

public interface IOverridePresentation
{
	BasePresentationAttribute PresentationAttribute(ViewModelRequest request);
}