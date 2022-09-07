using Autofac;

namespace MvvmKit.Abstractions.Core;

public interface IApplication
{
	ContainerBuilder GetContainerBuilder();

	void ConfigureContainer(ContainerBuilder containerBuilder);
}