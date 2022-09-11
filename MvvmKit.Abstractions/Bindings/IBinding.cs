using System.ComponentModel;
using System.Linq.Expressions;

namespace MvvmKit.Abstractions.Bindings;

public interface IBinding<TSource, TDestination>
	where TSource : INotifyPropertyChanged
{
	void Apply();

	ISourcePropertyBinding<TSource, TDestination> For<TProperty>(Expression<Func<TSource, TProperty>> sourceProperty);
}