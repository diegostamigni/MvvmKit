using MvvmKit.Extensions.DependencyInjection.Abstractions;

namespace MvvmKit.Abstractions.Core;

public interface ISetup
{
	IContainer CreateContainer();

	void SetupContainer(IContainer container);
}