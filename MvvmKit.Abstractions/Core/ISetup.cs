using Autofac;

namespace MvvmKit.Abstractions.Core;

public interface ISetup
{
	ContainerBuilder SetupContainer();

	IContainer BuildContainer();
}