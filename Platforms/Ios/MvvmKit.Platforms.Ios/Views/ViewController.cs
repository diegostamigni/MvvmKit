using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Views;

namespace MvvmKit.Platforms.Ios.Views;

public abstract class ViewController<TViewModel> : UIViewController, IIosView<TViewModel>
	where TViewModel : class, IViewModel
{
	IViewModel? IView.ViewModel
	{
		get => this.ViewModel;
		set => this.ViewModel = (TViewModel?) value;
	}

	public TViewModel? ViewModel { get; set; }

	public ViewModelRequest? Request { get; set; }

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		this.ViewModel?.ViewCreated();
	}

	public override void ViewWillAppear(bool animated)
	{
		base.ViewWillAppear(animated);
		this.ViewModel?.ViewAppearing();
	}

	public override void ViewDidAppear(bool animated)
	{
		base.ViewDidAppear(animated);
		this.ViewModel?.ViewAppeared();
	}

	public override void ViewWillDisappear(bool animated)
	{
		base.ViewWillDisappear(animated);
		this.ViewModel?.ViewDisappearing();
	}

	public override void ViewDidDisappear(bool animated)
	{
		base.ViewDidDisappear(animated);
		this.ViewModel?.ViewDisappeared();
	}

	public override void DidMoveToParentViewController(UIViewController? parent)
	{
		base.DidMoveToParentViewController(parent);

		if (parent == null)
		{
			this.ViewModel?.ViewDestroy();
		}
	}
}