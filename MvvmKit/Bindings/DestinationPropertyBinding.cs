using System.ComponentModel;
using System.Linq.Expressions;
using MvvmKit.Abstractions.Bindings;

namespace MvvmKit.Bindings;

sealed internal class DestinationPropertyBinding<TSource, TSourceProperty, TDestination, TDestinationProperty>
	: IDestinationPropertyBinding<TSource, TDestination, TDestinationProperty>
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
		var weakThis = new WeakReference<
			DestinationPropertyBinding<TSource, TSourceProperty, TDestination, TDestinationProperty>>(this);

		this.mappings[this.sourcePropertyKey] = () =>
		{
			if (!weakThis.TryGetTarget(out var unwrappedWeakThis))
			{
				return;
			}

			var target = unwrappedWeakThis.sourceProperty.Compile().Invoke(unwrappedWeakThis.source);

			var body = Expression.Assign(
				unwrappedWeakThis.destinationProperty.Body,
				Expression.Constant(target, target?.GetType() ?? typeof(TSourceProperty)));

			var lambda = Expression.Lambda<Action<TDestination>>(body, unwrappedWeakThis.destinationProperty.Parameters);

			var action = lambda.Compile();

			action.Invoke(unwrappedWeakThis.destination);
		};
	}
}