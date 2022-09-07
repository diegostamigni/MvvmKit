using Autofac;
using MvvmKit.Core;
using MvvmKit.Platforms.Ios.Abstractions.Core;
using MvvmKit.Platforms.Ios.Presenters;
using MvvmKit.Platforms.Ios.Threading;
using MvvmKit.Platforms.Ios.Views;

namespace MvvmKit.Platforms.Ios.Core;

public abstract class IosSetup : Setup, IIosSetup
{
	public override void ConfigureContainer(ContainerBuilder containerBuilder)
	{
		containerBuilder.RegisterType<IosViewPresenter>().AsImplementedInterfaces();
		containerBuilder.RegisterType<IosUIThreadDispatcher>().AsImplementedInterfaces();
		containerBuilder.RegisterType<IosViewDispatcher>().AsImplementedInterfaces();
		containerBuilder.RegisterType<IosViewsContainer>().AsImplementedInterfaces();
	}
}