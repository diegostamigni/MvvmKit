using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmKit.Bindings;

public abstract class Bindable : INotifyPropertyChanged
{
	private readonly Dictionary<string, object?> properties = new();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected T? Get<T>([CallerMemberName] string? name = null)
	{
		if (this.properties.TryGetValue(name!, out var value))
		{
			return (T?)value;
		}

		return default(T);
	}

	protected void Set<T>(T value, [CallerMemberName] string? name = null)
	{
		if (Equals(value, Get<T>(name)))
		{
			return;
		}

		this.properties[name!] = value;
		OnPropertyChanged(name!);
	}

	protected bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, value))
		{
			return false;
		}

		field = value;
		OnPropertyChanged(propertyName);

		return true;
	}

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> this.PropertyChanged?.Invoke(this, new(propertyName));
}