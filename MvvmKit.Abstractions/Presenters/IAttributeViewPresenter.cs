using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Abstractions.Presenters;

public interface IAttributeViewPresenter : IViewPresenter
{
	IDictionary<Type, PresentationAttributeAction>? AttributeTypesToActionsDictionary { get; }

	void RegisterAttributeTypes();

	BasePresentationAttribute GetPresentationAttribute(ViewModelRequest request);

	BasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);

	BasePresentationAttribute? GetOverridePresentationAttribute(ViewModelRequest request, Type viewType);
}