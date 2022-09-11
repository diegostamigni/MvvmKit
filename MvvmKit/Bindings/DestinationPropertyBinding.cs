using System.ComponentModel;
using System.Linq.Expressions;
using MvvmKit.Abstractions.Bindings;
using MvvmKit.Abstractions.Converters;

namespace MvvmKit.Bindings;

sealed internal class DestinationPropertyBinding<TSource, TDestination, TSourceProperty, TDestinationProperty>
	: IDestinationPropertyBinding<TSource, TDestination, TSourceProperty, TDestinationProperty>
	where TSource : INotifyPropertyChanged
{
	private readonly TSource source;
	private readonly TDestination destination;
	private readonly Expression<Func<TSource, TSourceProperty>> sourceProperty;
	private readonly Expression<Func<TDestination, TDestinationProperty>> destinationProperty;
	private readonly string sourcePropertyKey;
	private readonly Dictionary<string, Action> mappings;

	public DestinationPropertyBinding(
		TSource source,
		TDestination destination,
		Expression<Func<TSource, TSourceProperty>> sourceProperty,
		Expression<Func<TDestination, TDestinationProperty>> destinationProperty,
		string sourcePropertyKey,
		Dictionary<string, Action> mappings)
	{
		this.source = source;
		this.destination = destination;
		this.sourceProperty = sourceProperty;
		this.destinationProperty = destinationProperty;
		this.sourcePropertyKey = sourcePropertyKey;
		this.mappings = mappings;

		SaveBinding();
	}

	private void SaveBinding()
	{
		this.mappings[this.sourcePropertyKey] = () =>
		{
			var sourceValue = this.sourceProperty
				.Compile()
				.Invoke(this.source);

			var body = Expression.Assign(
				this.destinationProperty.Body,
				Expression.Constant(sourceValue, typeof(TDestinationProperty)));

			var lambda = Expression.Lambda<Action<TDestination>>(body, this.destinationProperty.Parameters);

			var action = lambda.Compile();

			action.Invoke(this.destination);
		};
	}

	public void WithConversion<TConverter>() where TConverter : IValueConverter<TSourceProperty, TDestinationProperty>, new()
	{
		this.mappings[this.sourcePropertyKey] = () =>
		{
			var sourceValueUnconverted = this.sourceProperty
				.Compile()
				.Invoke(this.source);

			var valueConverter = new TConverter();

			var convertedSourceValue = valueConverter.Convert(sourceValueUnconverted);

			var body = Expression.Assign(
				this.destinationProperty.Body,
				Expression.Constant(convertedSourceValue, typeof(TDestinationProperty)));

			var lambda = Expression.Lambda<Action<TDestination>>(body, this.destinationProperty.Parameters);

			var action = lambda.Compile();

			action.Invoke(this.destination);
		};
	}
}