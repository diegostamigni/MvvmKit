namespace MvvmKit.Platforms.Ios.Views;

public class NavigationController : UINavigationController
{
	public NavigationController()
	{
	}

	public NavigationController(UIViewController rootViewController) : base(rootViewController)
	{
	}

	public NavigationController(NSCoder coder) : base(coder)
	{
	}

	public NavigationController(string nibName, NSBundle bundle) : base(nibName, bundle)
	{
	}

	public NavigationController(Type navigationBarType, Type toolbarType) : base(navigationBarType, toolbarType)
	{
	}

	protected NavigationController(NSObjectFlag t) : base(t)
	{
	}

	protected internal NavigationController(IntPtr handle) : base(handle)
	{
	}

	public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
		=> this.TopViewController.GetSupportedInterfaceOrientations();

	public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
		=> this.TopViewController.PreferredInterfaceOrientationForPresentation();

	public override bool ShouldAutorotate() => this.TopViewController.ShouldAutorotate();
}