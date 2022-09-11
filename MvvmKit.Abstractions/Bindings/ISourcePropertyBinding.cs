using System.ComponentModel;
using System.Linq.Expressions;

namespace MvvmKit.Abstractions.Bindings;

public interface ISourcePropertyBinding<TSource, TDestination>
	where TSource : INotifyPropertyChanged
{
	IDestinationPropertyBinding<TSource, TDestination> To<TProperty>(
		Expression<Func<TDestination, TProperty>> destinationProperty);
}