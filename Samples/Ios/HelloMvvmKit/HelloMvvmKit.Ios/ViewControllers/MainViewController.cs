using System.Diagnostics.CodeAnalysis;
using HelloMvvmKit.Core.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Views;

namespace HelloMvvmKit.Ios.ViewControllers;

[SuppressMessage("ReSharper", "AsyncVoidLambda")]
[RootPresentation(typeof(HomeViewModel), WrapInNavigationController = true)]
public class MainViewController : ViewController<HomeViewModel>
{
	public override void LoadView()
	{
		base.LoadView();
		this.View!.BackgroundColor = UIColor.SystemBackground;
	}

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

		this.Title = $"{GetType().Name} ({this.ViewModel!.GetType().Name})";

		this.NavigationItem.SetRightBarButtonItem(new(UIBarButtonSystemItem.Organize, (_, _) =>
		{
			this.ViewModel!.NavigateNextCommand.Execute();
		}), true);
	}
}