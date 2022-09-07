using HelloMvvmKit.Core;
using HelloMvvmKit.Core.ViewModels;
using MvvmKit.Platforms.Ios.Core;

namespace HelloMvvmKit.Ios;

[Register ("AppDelegate")]
public class AppDelegate : ApplicationDelegate<App, Setup, HomeViewModel>
{
	public override UIWindow? Window { get; set; }

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
		base.FinishedLaunching(application, launchOptions);

		return true;
	}
}
