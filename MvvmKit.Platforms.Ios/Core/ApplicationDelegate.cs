using Autofac;
using MvvmKit.Abstractions.Core;
using MvvmKit.Abstractions.Navigation;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Core;

namespace MvvmKit.Platforms.Ios.Core;

public abstract class ApplicationDelegate<TApp, TSetup> : UIApplicationDelegate, IApplicationDelegate
	where TApp : IApplication, new()
	where TSetup : IIosSetup, new()
{
	protected IContainer Container { get; private set; } = null!;

	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		// TODO: Apply proper async/await
		ConfigureSetupAsync().GetAwaiter().GetResult();

		base.FinishedLaunching(application, launchOptions);

		this.Window ??= new();
		this.Window.MakeKeyAndVisible();

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
	where TApp : IApplication, new()
	where TSetup : IIosSetup, new()
	where TMainViewModel : IViewModel
{
	protected override async Task ConfigureSetupAsync()
	{
		await base.ConfigureSetupAsync();

		var navigationService = this.Container.Resolve<INavigationService>();

		await navigationService.NavigateAsync<TMainViewModel>();
	}
}