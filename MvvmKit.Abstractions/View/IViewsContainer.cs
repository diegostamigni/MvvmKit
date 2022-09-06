using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Abstractions.View;

public interface IViewsContainer : IViewFinder
{
	void AddAll(IDictionary<Type, Type> viewModelViewLookup);

	void Add(Type viewModelType, Type viewType);

	void Add<TViewModel, TView>()
		where TViewModel : IViewModel
		where TView : IView;

	void AddSecondary(IViewFinder finder);

	void SetLastResort(IViewFinder finder);
}