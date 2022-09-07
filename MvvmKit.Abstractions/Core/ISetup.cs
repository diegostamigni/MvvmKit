using Autofac;

namespace MvvmKit.Abstractions.Core;

public interface ISetup
{
	void ConfigureContainer(ContainerBuilder containerBuilder);

	IContainer BuildContainer(ContainerBuilder containerBuilder);
}