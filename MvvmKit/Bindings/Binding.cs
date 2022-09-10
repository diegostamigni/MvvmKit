using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace MvvmKit.Bindings;

[SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
public class Binding<TSource, TDestination> where TSource : INotifyPropertyChanged
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

	public static Binding<TSource, TDestination> Create(TSource source, TDestination view) => new(source, view);

	public void Bind<TProperty>(
		Expression<Func<TSource, TProperty>> viewModelProperty,
		Expression<Func<TDestination, TProperty>> viewProperty)
	{
		var weakThis = new WeakReference<Binding<TSource, TDestination>>(this);
		var key = GetKey(viewModelProperty);
		this.mappings.Add(key, () =>
		{
			if (!weakThis.TryGetTarget(out var unwrappedWeakThis))
			{
				return;
			}

			var target = viewModelProperty.Compile().Invoke(unwrappedWeakThis.source);
			var body = Expression.Assign(viewProperty.Body, Expression.Constant(target, target?.GetType() ?? typeof(TProperty)));
			var lambda = Expression.Lambda<Action<TDestination>>(body, viewProperty.Parameters);
			var action = lambda.Compile();
			action.Invoke(unwrappedWeakThis.destination);
		});
	}

	public void Apply()
	{
		foreach (var (_, action) in this.mappings)
		{
			action.Invoke();
		}
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