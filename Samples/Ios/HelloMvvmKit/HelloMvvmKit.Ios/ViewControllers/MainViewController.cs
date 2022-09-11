using HelloMvvmKit.Core.ViewModels;
using MvvmKit.Abstractions.Converters;
using MvvmKit.Bindings;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Views;

namespace HelloMvvmKit.Ios.ViewControllers;

[RootPresentation(typeof(HomeViewModel), WrapInNavigationController = true)]
public class MainViewController : ViewController<HomeViewModel>
{
	private readonly UIImageView imageView = new();
	private readonly UIActivityIndicatorView activityIndicator = new(UIActivityIndicatorViewStyle.Large);

	public override void LoadView()
	{
		base.LoadView();

		this.View!.BackgroundColor = UIColor.SystemBackground;

		this.imageView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
		this.imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
		this.imageView.Frame = this.View!.Frame;
		this.View!.AddSubview(this.imageView);

		this.activityIndicator.AutoresizingMask = UIViewAutoresizing.None;
		this.activityIndicator.Center = this.View!.Center;
		this.activityIndicator.StartAnimating();
		this.View!.AddSubview(this.activityIndicator);
	}

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

		this.NavigationItem.SetRightBarButtonItem(new("Next", UIBarButtonItemStyle.Plain, (_, _) =>
		{
			this.ViewModel!.NavigateNextCommand.Execute();
		}), true);

		this.NavigationItem.SetLeftBarButtonItem(new("Load image", UIBarButtonItemStyle.Plain, (_, _) =>
		{
			this.ViewModel!.LoadImageCommand.Execute();
		}), true);

		SetupBindings();
	}

	private void SetupBindings()
	{
		var bindings = Binding<HomeViewModel, MainViewController>.Create(this.ViewModel!, this);
		bindings.For(vm => vm.Title).To(vc => vc.Title);
		bindings.For(vm => vm.ImageContent).To(vc => vc.imageView.Image).WithConversion<ByteArrayToImageConverter>();
		bindings.For(vm => vm.IsLoading).To(vc => vc.activityIndicator.Hidden).WithConversion<InvertedBoolConverter>();
		bindings.Apply();
	}

	private class ByteArrayToImageConverter : IValueConverter<byte[]?, UIImage?>
	{
		public UIImage? Convert(byte[]? value) => value is null ? null : UIImage.LoadFromData(NSData.FromArray(value));
	}

	private class InvertedBoolConverter : IValueConverter<bool?, bool>
	{
		public bool Convert(bool? value) => !value ?? false;
	}
}