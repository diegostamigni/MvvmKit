using Autofac;
using MvvmKit.Abstractions.Core;

namespace MvvmKit.Core;

public abstract class Setup : ISetup
{
	public virtual void ConfigureContainer(ContainerBuilder containerBuilder)
	{
	}

	public IContainer BuildContainer(ContainerBuilder containerBuilder) => containerBuilder.Build();
}