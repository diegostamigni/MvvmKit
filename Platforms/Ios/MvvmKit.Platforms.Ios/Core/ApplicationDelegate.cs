using Autofac;
using MvvmKit.Abstractions.Core;
using MvvmKit.Abstractions.Navigation;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Core;
using MvvmKit.Platforms.Ios.Abstractions.Core;

namespace MvvmKit.Platforms.Ios.Core;

public abstract class ApplicationDelegate<TApp, TSetup> : UIApplicationDelegate, IApplicationDelegate
	where TApp : Application, new()
	where TSetup : IosSetup, new()
{
	protected IContainer Container { get; private set; } = null!;

	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		this.Window ??= new(UIScreen.MainScreen.Bounds);
		this.Window.MakeKeyAndVisible();

		// TODO: Apply proper async/await
		ConfigureSetupAsync().GetAwaiter().GetResult();

		return true;
	}

	protected virtual Task ConfigureSetupAsync()
	{
		var application = new TApp();

		var containerBuilder = application.GetContainerBuilder();

		application.ConfigureContainer(containerBuilder);

		var setup = new TSetup();

		setup.ConfigureContainer(containerBuilder);

		this.Container = setup.BuildContainer(containerBuilder);

		return Task.CompletedTask;
	}
}

public abstract class ApplicationDelegate<TApp, TSetup, TMainViewModel> : ApplicationDelegate<TApp, TSetup>
	where TApp : Application, new()
	where TSetup : IosSetup, new()
	where TMainViewModel : IViewModel
{
	protected override async Task ConfigureSetupAsync()
	{
		await base.ConfigureSetupAsync();

		var navigationService = this.Container.Resolve<INavigationService>();

		await navigationService.NavigateAsync<TMainViewModel>();
	}
}