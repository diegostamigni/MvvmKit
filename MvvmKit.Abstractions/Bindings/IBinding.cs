using System.ComponentModel;
using System.Linq.Expressions;

namespace MvvmKit.Abstractions.Bindings;

public interface IBinding<TSource, TDestination>
	where TSource : INotifyPropertyChanged
{
	void Apply();

	ISourcePropertyBinding<TSource, TDestination, TSourceProperty> For<TSourceProperty>(
		Expression<Func<TSource, TSourceProperty>> sourceProperty);
}