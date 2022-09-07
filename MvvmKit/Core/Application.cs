using Autofac;
using MvvmKit.Abstractions.Core;
using MvvmKit.Abstractions.Navigation;
using MvvmKit.Navigation;

namespace MvvmKit.Core;

public abstract class Application : IApplication
{
	public virtual ContainerBuilder GetContainerBuilder() => new();

	public virtual void ConfigureContainer(ContainerBuilder containerBuilder)
	{
		containerBuilder.RegisterType<NavigationService>().As<INavigationService>();
	}
}