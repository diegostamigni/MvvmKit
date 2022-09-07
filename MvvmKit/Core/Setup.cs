using MvvmKit.Abstractions.Core;
using MvvmKit.Extensions.DependencyInjection.Abstractions;

namespace MvvmKit.Core;

public abstract class Setup : ISetup
{
	public abstract IContainer CreateContainer();

	public void SetupContainer(IContainer container)
	{
		throw new NotImplementedException();
	}
}