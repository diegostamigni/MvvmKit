using Autofac;
using IMvvmKitContainer = MvvmKit.Extensions.DependencyInjection.Abstractions.IContainer;
using NamedParameter = MvvmKit.Extensions.DependencyInjection.Abstractions.NamedParameter;
using Parameter = MvvmKit.Extensions.DependencyInjection.Abstractions.Parameter;
using IAutofacContainer = Autofac.IContainer;
using AutofacNamedParameter = Autofac.NamedParameter;
using AutofacParameter = Autofac.Core.Parameter;

namespace MvvmKit.Extensions.DependencyInjection.Autofac;

public class AutofacContainer : IMvvmKitContainer
{
	private readonly IAutofacContainer container;

	public AutofacContainer(IAutofacContainer container)
	{
		this.container = container;
	}

	public bool CanResolve<T>() where T : class => this.container.IsRegistered<T>();

	public bool CanResolve(Type type) => this.container.IsRegistered(type);

	public T Resolve<T>() where T : class => (T)Resolve(typeof(T));

	public T Resolve<T>(params Parameter[] parameters) where T : class
		=> this.container.Resolve<T>(GetParameters(parameters));

	public object Resolve(Type type) => this.container.Resolve(type);

	public object Resolve(Type type, params Parameter[] parameters)
		=> this.container.Resolve(type, GetParameters(parameters));

	public bool TryResolve<T>(out T? resolved) where T : class => this.container.TryResolve(out resolved);

	public bool TryResolve(Type type, out object? resolved) => this.container.TryResolve(type, out resolved);

	private static IEnumerable<AutofacParameter> GetParameters(params Parameter[] parameters)
	{
		var autofacParameters = new List<AutofacParameter>();
		foreach (var parameter in parameters)
		{
			switch (parameter)
			{
				case NamedParameter namedParameter:
					autofacParameters.Add(new AutofacNamedParameter(namedParameter.Name, namedParameter.Value));
					break;

				default:
					throw new NotSupportedException($"Parameter type {parameter.GetType().Name} is not supported");
			}
		}

		return autofacParameters;
	}
}