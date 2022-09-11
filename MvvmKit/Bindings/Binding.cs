using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using MvvmKit.Abstractions.Bindings;

namespace MvvmKit.Bindings;

[SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
public class Binding<TSource, TDestination> :
	IBinding<TSource, TDestination>,
	ISourcePropertyBinding<TSource, TDestination>,
	IDestinationPropertyBinding<TSource, TDestination> where TSource : INotifyPropertyChanged
{
	private readonly TSource source;
	private readonly TDestination destination;
	private readonly Dictionary<string, Action> mappings = new();

	private object? tmpSourceProperty;

	protected Binding(TSource source, TDestination destination)
	{
		this.source = source;
		this.destination = destination;

		source.PropertyChanged += SourceOnPropertyChanged;
	}

	public static IBinding<TSource, TDestination> Create(TSource source, TDestination view)
		=> new Binding<TSource, TDestination>(source, view);

	public void Apply()
	{
		foreach (var (_, action) in this.mappings)
		{
			action.Invoke();
		}
	}

	public ISourcePropertyBinding<TSource, TDestination> For<TProperty>(
		Expression<Func<TSource, TProperty>> sourceProperty)
	{
		this.tmpSourceProperty = sourceProperty;

		return this;
	}

	public IDestinationPropertyBinding<TSource, TDestination> To<TProperty>(
		Expression<Func<TDestination, TProperty>> destinationProperty)
	{
		if (this.tmpSourceProperty is not Expression<Func<TSource, TProperty>> sourceProperty)
		{
			throw new InvalidOperationException("Source property is not set");
		}

		SaveBinding(sourceProperty, destinationProperty);

		this.tmpSourceProperty = null;

		return this;
	}

	private void SaveBinding<TProperty>(
		Expression<Func<TSource, TProperty>> sourceProperty,
		Expression<Func<TDestination, TProperty>> destinationProperty)
	{
		var key = GetKey(sourceProperty);

		var weakThis = new WeakReference<Binding<TSource, TDestination>>(this);

		this.mappings[key] = () =>
		{
			if (!weakThis.TryGetTarget(out var unwrappedWeakThis))
			{
				return;
			}

			var target = sourceProperty.Compile().Invoke(unwrappedWeakThis.source);

			var body = Expression.Assign(
				destinationProperty.Body,
				Expression.Constant(target, target?.GetType() ?? typeof(TProperty)));

			var lambda = Expression.Lambda<Action<TDestination>>(body, destinationProperty.Parameters);

			var action = lambda.Compile();

			action.Invoke(unwrappedWeakThis.destination);
		};
	}

	private static string GetKey<TProperty>(Expression<Func<TSource,TProperty>> viewModelProperty)
	{
		var memberExpression = (MemberExpression)viewModelProperty.Body;
		var member = memberExpression.Member;
		var declaringType = member.DeclaringType ?? member.ReflectedType ?? throw new InvalidOperationException();

		return $"{declaringType.FullName}+{member.Name}";
	}

	private Action? GetAction(string propertyName)
	{
		var key = $"{this.source.GetType().FullName}+{propertyName}";
		return this.mappings.TryGetValue(key, out var action) ? action : default;
	}

	private void SourceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is null)
		{
			return;
		}

		var action = GetAction(e.PropertyName);
		action?.Invoke();
	}
}