using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Views;

public abstract class ViewsContainer : IViewsContainer
{
	private readonly Dictionary<Type, Type> bindingMap = new();
	private readonly List<IViewFinder> secondaryViewFinders;
	private IViewFinder? lastResortViewFinder;

	protected ViewsContainer()
	{
		this.secondaryViewFinders = new();
	}

	public void AddAll(IDictionary<Type, Type> viewModelViewLookup)
	{
		foreach (var pair in viewModelViewLookup)
		{
			Add(pair.Key, pair.Value);
		}
	}

	public void Add(Type viewModelType, Type viewType)
	{
		this.bindingMap[viewModelType] = viewType;
	}

	public void Add<TViewModel, TView>()
		where TViewModel : IViewModel
		where TView : IView
	{
		Add(typeof(TViewModel), typeof(TView));
	}

	public Type GetViewType(Type? viewModelType)
	{
		if (viewModelType is not null && this.bindingMap.TryGetValue(viewModelType, out var binding))
		{
			return binding;
		}

		foreach (var viewFinder in this.secondaryViewFinders)
		{
			binding = viewFinder.GetViewType(viewModelType);
			if (binding is not null)
			{
				return binding;
			}
		}

		if (this.lastResortViewFinder is not null)
		{
			binding = this.lastResortViewFinder.GetViewType(viewModelType);
			if (binding is not null)
			{
				return binding;
			}
		}

		throw new KeyNotFoundException("Could not find view for " + viewModelType);
	}

	public void AddSecondary(IViewFinder finder)
	{
		this.secondaryViewFinders.Add(finder);
	}

	public void SetLastResort(IViewFinder finder)
	{
		this.lastResortViewFinder = finder;
	}
}