using System.ComponentModel;

namespace MvvmKit.Abstractions.Bindings;

public interface IDestinationPropertyBinding<TSource, TDestination, TDestinationProperty>
	where TSource : INotifyPropertyChanged
{
}