using System.Diagnostics.CodeAnalysis;
using Autofac;
using MvvmKit.Abstractions.Core;

namespace MvvmKit.Core;

public abstract class Setup : ISetup
{
	public virtual void ConfigureContainer(ContainerBuilder containerBuilder)
	{
	}

	[SuppressMessage("Performance", "CA1822:Mark members as static")]
	[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
	public IContainer BuildContainer(ContainerBuilder containerBuilder) => containerBuilder.Build();
}