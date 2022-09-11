using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using MvvmKit.Abstractions.Bindings;

namespace MvvmKit.Bindings;

public sealed class Binding<TSource, TDestination> : IBinding<TSource, TDestination>
	where TSource : INotifyPropertyChanged
{
	private readonly TSource source;
	private readonly TDestination destination;
	private readonly Dictionary<string, Action> mappings = new();

	private Binding(TSource source, TDestination destination)
	{
		this.source = source;
		this.destination = destination;

		source.PropertyChanged += SourceOnPropertyChanged;
	}

	[SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
	public static IBinding<TSource, TDestination> Create(TSource source, TDestination view)
		=> new Binding<TSource, TDestination>(source, view);

	public void Apply()
	{
		foreach (var (_, action) in this.mappings)
		{
			action.Invoke();
		}
	}

	public ISourcePropertyBinding<TSource, TDestination, TSourceProperty> For<TSourceProperty>(
		Expression<Func<TSource, TSourceProperty>> sourceProperty)
	{
		var propertyBinding = new SourcePropertyBinding<TSource, TDestination, TSourceProperty>(
			this.source,
			this.destination,
			sourceProperty,
			GetKey(sourceProperty),
			this.mappings);

		return propertyBinding;
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