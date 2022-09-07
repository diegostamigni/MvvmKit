using Autofac;
using MvvmKit.Abstractions.Core;
using MvvmKit.Abstractions.Navigation;
using MvvmKit.Navigation;

namespace MvvmKit.Core;

public abstract class Setup : ISetup
{
	public virtual ContainerBuilder SetupContainer()
	{
		var containerBuilder = new ContainerBuilder();
		containerBuilder.RegisterType<NavigationService>().As<INavigationService>();

		return containerBuilder;
	}

	public IContainer BuildContainer() => SetupContainer().Build();
}