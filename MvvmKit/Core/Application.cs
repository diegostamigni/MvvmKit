using Autofac;
using MvvmKit.Abstractions.Core;
using MvvmKit.ViewModels.DependencyInjection;

namespace MvvmKit.Core;

public abstract class Application : IApplication
{
	public virtual ContainerBuilder GetContainerBuilder() => new();

	public virtual void ConfigureContainer(ContainerBuilder containerBuilder)
	{
		containerBuilder.RegisterAssemblyTypes(typeof(Application).Assembly)
			.Where(x => RegistrationHelper.RegistrationSuffixes
				.Any(y => x.Name.EndsWith(y, StringComparison.InvariantCultureIgnoreCase)))
			.AsImplementedInterfaces();

		containerBuilder.RegisterModule(new ViewModelModule(GetType()));
	}
}