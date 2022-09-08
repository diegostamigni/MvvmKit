using HelloMvvmKit.Core.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Views;

namespace HelloMvvmKit.Ios.ViewControllers;

[ChildPresentation(typeof(FirstChildViewModel))]
public class FirstChildViewController : ViewController<FirstChildViewModel>
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
	}
}