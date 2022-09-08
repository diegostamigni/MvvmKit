using Autofac;
using MvvmKit.Core;
using MvvmKit.Platforms.Ios.Abstractions.Core;
using MvvmKit.Platforms.Ios.Presenters;
using MvvmKit.Platforms.Ios.Views;
using MvvmKit.Platforms.Ios.Views.DependencyInjection;

namespace MvvmKit.Platforms.Ios.Core;

public abstract class IosSetup : Setup, IIosSetup
{
	public override void ConfigureContainer(ContainerBuilder containerBuilder)
	{
		containerBuilder.RegisterAssemblyTypes(typeof(IosSetup).Assembly)
			.Where(x => RegistrationHelper.RegistrationSuffixes
				.Any(y => x.Name.EndsWith(y, StringComparison.InvariantCultureIgnoreCase)))
			.Except<IosViewsContainer>()
			.Except<IosViewPresenter>()
			.AsImplementedInterfaces();

		containerBuilder.RegisterType<IosViewPresenter>()
			.AsImplementedInterfaces()
			.SingleInstance();

		containerBuilder.RegisterModule(new ViewsModule(GetType()));
	}
}