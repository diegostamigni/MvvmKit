using System.ComponentModel;
using System.Linq.Expressions;

namespace MvvmKit.Abstractions.Bindings;

public interface ISourcePropertyBinding<TSource, TDestination, TSourceProperty>
	where TSource : INotifyPropertyChanged
{
	IDestinationPropertyBinding<TSource, TDestination, TDestinationProperty> To<TDestinationProperty>(
		Expression<Func<TDestination, TDestinationProperty>> destinationProperty);
}