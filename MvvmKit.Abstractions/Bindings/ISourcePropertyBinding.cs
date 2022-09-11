using System.Linq.Expressions;

namespace MvvmKit.Abstractions.Bindings;

public interface ISourcePropertyBinding<TSource, TDestination, out TSourceProperty>
{
	IDestinationPropertyBinding<TSource, TDestination, TSourceProperty, TDestinationProperty> To<TDestinationProperty>(
		Expression<Func<TDestination, TDestinationProperty>> destinationProperty);
}