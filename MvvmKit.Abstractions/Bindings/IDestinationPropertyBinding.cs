using MvvmKit.Abstractions.Converters;

namespace MvvmKit.Abstractions.Bindings;

public interface IDestinationPropertyBinding<TSource, TDestination, out TSourceProperty, in TDestinationProperty>
{
	void WithConversion<TConverter>()
		where TConverter : IValueConverter<TSourceProperty, TDestinationProperty>, new();
}