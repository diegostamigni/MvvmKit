namespace MvvmKit.Abstractions.Converters;

public interface IValueConverter<in TSource, out TDestination>
{
	TDestination Convert(TSource value);
}