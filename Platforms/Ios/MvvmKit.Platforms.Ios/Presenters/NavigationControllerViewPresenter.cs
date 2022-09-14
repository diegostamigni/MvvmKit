using System.Diagnostics.CodeAnalysis;
using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Abstractions.Views;
using MvvmKit.Platforms.Ios.Views;
using MvvmKit.Presenters;

namespace MvvmKit.Platforms.Ios.Presenters;

public class NavigationControllerViewPresenter : ViewPresenter, INavigationControllerViewPresenter
{
	private readonly IIosViewCreator viewCreator;
	private readonly IAttributeViewPresenterHelper attributeViewPresenterHelper;

	private UINavigationController? masterNavigationController;

	[SuppressMessage("Performance", "CA1822:Mark members as static")]
	private UIWindow Window => UIApplication.SharedApplication.ConnectedScenes
		.Cast<UIScene>()
		.Where(x =>
			x is UIWindowScene &&
			x.ActivationState is UISceneActivationState.ForegroundActive
				or UISceneActivationState.ForegroundInactive)
		.Select(x => x as UIWindowScene)
		.SelectMany(x => x!.Windows)
		.FirstOrDefault(x => x.IsKeyWindow) ?? UIApplication.SharedApplication.Delegate.GetWindow();

	public NavigationControllerViewPresenter(
		IIosViewCreator viewCreator,
		IAttributeViewPresenterHelper attributeViewPresenterHelper)
	{
		this.viewCreator = viewCreator;
		this.attributeViewPresenterHelper = attributeViewPresenterHelper;
	}

	protected virtual NavigationController CreateNavigationController(UIViewController viewController) => new(viewController);

	protected void SetupWindowRootNavigation(UIViewController viewController, RootPresentationAttribute attribute)
	{
		if (attribute.WrapInNavigationController)
		{
			this.masterNavigationController = CreateNavigationController(viewController);

			SetWindowRootViewController(this.masterNavigationController, attribute);
		}
		else
		{
			SetWindowRootViewController(viewController, attribute);

			CloseMasterNavigationController();
		}
	}

	protected virtual void SetWindowRootViewController(
		UIViewController controller,
		RootPresentationAttribute? attribute = null)
	{
		if (attribute is null || attribute.AnimationOptions == UIViewAnimationOptions.TransitionNone)
		{
			this.Window.RootViewController = controller;
			return;
		}

		UIView.Transition(this.Window, attribute.AnimationDuration, attribute.AnimationOptions,
			() => this.Window.RootViewController = controller, null
		);
	}

	protected virtual void CloseMasterNavigationController()
	{
		if (this.masterNavigationController is null)
		{
			return;
		}

		if (this.masterNavigationController.ViewControllers is not null)
		{
			foreach (var item in this.masterNavigationController.ViewControllers)
			{
				item.DidMoveToParentViewController(null);
			}
		}

		this.masterNavigationController = null;
	}

	public override Task<bool> ShowAsync(ViewModelRequest request)
	{
		var presentationAttribute = this.attributeViewPresenterHelper.GetPresentationAttribute(request);

		var view = (UIViewController) this.viewCreator.CreateView(request);

		return presentationAttribute switch
		{
			RootPresentationAttribute rootAttribute => ShowRootViewControllerAsync(view, rootAttribute),
			ChildPresentationAttribute childAttribute => ShowChildViewControllerAsync(view, childAttribute),
			_ => throw new NotSupportedException($"Presentation attribute {presentationAttribute.GetType().Name} is not supported in this context")
		};
	}

	protected virtual Task<bool> ShowRootViewControllerAsync(
		UIViewController viewController,
		RootPresentationAttribute attribute)
	{
		SetupWindowRootNavigation(viewController, attribute);
		return Task.FromResult(true);
	}

	protected virtual Task<bool> ShowChildViewControllerAsync(
		UIViewController viewController,
		ChildPresentationAttribute attribute)
	{
		if (this.masterNavigationController is not null)
		{
			this.masterNavigationController.PushViewController(viewController, attribute.Animated);
			return Task.FromResult(true);
		}

		throw new InvalidOperationException(
			$"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
	}

	public override Task<bool> CloseAsync(IViewModel viewModel)
	{
		var attribute = this.attributeViewPresenterHelper.GetPresentationAttribute(new(viewModel.GetType()));

		var result = attribute switch
		{
			RootPresentationAttribute => false,
			ChildPresentationAttribute childAttribute => CloseChildViewController(viewModel, childAttribute),
			_ => throw new NotSupportedException($"Presentation attribute {attribute.GetType().Name} is not supported in this context")
		};

		return Task.FromResult(result);
	}

	private bool CloseChildViewController(IViewModel viewModel, ChildPresentationAttribute attribute)
	{
		if (this.masterNavigationController is null)
		{
			return false;
		}

		// check for top view controller
		var topView = this.masterNavigationController.TopViewController;

		if (topView is IIosView iosView && iosView.ViewModel == viewModel)
		{
			this.masterNavigationController.PopViewController(attribute.Animated);

			return true;
		}

		// loop through stack
		var controllers = this.masterNavigationController.ViewControllers?.ToList() ?? new();

		var controllerToClose = controllers.SingleOrDefault(vc => vc is IIosView iView && iView.ViewModel == viewModel);

		if (controllerToClose is not null)
		{
			controllers.Remove(controllerToClose);

			this.masterNavigationController.ViewControllers = controllers.ToArray();

			return true;
		}

		return false;
	}
}