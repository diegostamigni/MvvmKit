using System.ComponentModel;

namespace MvvmKit.Abstractions.Bindings;

public interface IDestinationPropertyBinding<TSource, TDestination>
	where TSource : INotifyPropertyChanged
{
}