namespace MvvmKit.Extensions.DependencyInjection.Abstractions;

public interface IContainer
{
	bool CanResolve<T>() where T : class;

	bool CanResolve(Type type);

	T Resolve<T>() where T : class;

	T Resolve<T>(params Parameter[] parameters) where T : class;

	object Resolve(Type type);

	object Resolve(Type type, params Parameter[] parameters);

	bool TryResolve<T>(out T? resolved) where T : class;

	bool TryResolve(Type type, out object? resolved);
}