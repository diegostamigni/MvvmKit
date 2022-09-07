using MvvmKit.Abstractions.Navigation;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Extensions.DependencyInjection.Abstractions;
using MvvmKit.Platforms.Ios.Abstractions.Core;

namespace MvvmKit.Platforms.Ios.Core;

public abstract class ApplicationDelegate<TSetup> : UIApplicationDelegate, IApplicationDelegate
	where TSetup : IIosSetup, new()
{
	protected TSetup Setup { get; } = new();

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
		this.Container = this.Setup.CreateContainer();

		return Task.CompletedTask;
	}
}

public abstract class ApplicationDelegate<TSetup, TMainViewModel> : ApplicationDelegate<TSetup>
	where TSetup : IIosSetup<TMainViewModel>, new()
	where TMainViewModel : IViewModel
{
	protected override async Task ConfigureSetupAsync()
	{
		var navigationService = this.Container.Resolve<INavigationService>();
		await navigationService.NavigateAsync<TMainViewModel>();
	}
}