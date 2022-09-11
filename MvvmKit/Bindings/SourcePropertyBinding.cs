using System.ComponentModel;
using System.Linq.Expressions;
using MvvmKit.Abstractions.Bindings;

namespace MvvmKit.Bindings;

sealed internal class SourcePropertyBinding<TSource, TDestination, TSourceProperty>
	: ISourcePropertyBinding<TSource, TDestination, TSourceProperty>
	where TSource : INotifyPropertyChanged
{
	private readonly TSource source;
	private readonly TDestination destination;
	private readonly Expression<Func<TSource, TSourceProperty>> sourceProperty;
	private readonly string sourcePropertyKey;
	private readonly Dictionary<string, Action> mappings;

	public SourcePropertyBinding(
		TSource source,
		TDestination destination,
		Expression<Func<TSource, TSourceProperty>> sourceProperty,
		string sourcePropertyKey,
		Dictionary<string, Action> mappings)
	{
		this.source = source;
		this.destination = destination;
		this.sourceProperty = sourceProperty;
		this.sourcePropertyKey = sourcePropertyKey;
		this.mappings = mappings;
	}

	public IDestinationPropertyBinding<TSource, TDestination, TDestinationProperty> To<TDestinationProperty>(
		Expression<Func<TDestination, TDestinationProperty>> destinationProperty)
	{
		var propertyBinding = new DestinationPropertyBinding<TSource, TSourceProperty, TDestination, TDestinationProperty>(
			this.source,
			this.destination,
			this.sourceProperty,
			destinationProperty,
			this.sourcePropertyKey,
			this.mappings);

		return propertyBinding;
	}
}