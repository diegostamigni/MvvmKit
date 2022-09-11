using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Presenters;

public static class PresentationAttributeExtensions
{
	public static void Register<TPresentationAttribute>(
		this IDictionary<Type, PresentationAttributeAction> attributeTypesToActionsDictionary,
		Func<Type, TPresentationAttribute, ViewModelRequest, Task<bool>> showAction,
		Func<IViewModel, TPresentationAttribute, Task<bool>> closeAction)
		where TPresentationAttribute : class, IPresentation
	{
		attributeTypesToActionsDictionary.Add(
			typeof(TPresentationAttribute),
			new()
			{
				ShowAction = (view, attribute, request) => showAction(view, (attribute as TPresentationAttribute)!, request),
				CloseAction = (viewModel, attribute) => closeAction(viewModel, (attribute as TPresentationAttribute)!)
			});
	}
}