using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Abstractions.Views;
using MvvmKit.Platforms.Ios.Views;
using MvvmKit.Presenters;

namespace MvvmKit.Platforms.Ios.Presenters;

public abstract class ViewControllerViewPresenter<TPresentationAttribute> : ViewPresenter
	where TPresentationAttribute : BasePresentationAttribute
{
	private UINavigationController? navigationController;

	private readonly IIosViewCreator viewCreator;
	private readonly IAttributeViewPresenterHelper attributeViewPresenterHelper;

	protected ViewControllerViewPresenter(
		IIosViewCreator viewCreator,
		IAttributeViewPresenterHelper attributeViewPresenterHelper)
	{
		this.viewCreator = viewCreator;
		this.attributeViewPresenterHelper = attributeViewPresenterHelper;
	}

	protected abstract Task<bool> HandleShowAsync(UIViewController view, TPresentationAttribute presentationAttribute);

	protected abstract Task<bool> HandleCloseAsync(IViewModel viewModel, TPresentationAttribute presentationAttribute);

	public override async Task<bool> ShowAsync(ViewModelRequest request)
	{
		var presentationAttribute = this.attributeViewPresenterHelper.GetPresentationAttribute(request);

		var view = (UIViewController) this.viewCreator.CreateView(request);

		if (presentationAttribute is TPresentationAttribute typedPresentationAttribute)
		{
			if (GetWindow().RootViewController is null)
			{
				await SetRootViewControllerAsync(view, typedPresentationAttribute);
			}

			return await HandleShowAsync(view, typedPresentationAttribute);
		}

		return presentationAttribute switch
		{
			ChildPresentationAttribute childAttribute => await ShowChildViewControllerAsync(view, childAttribute),
			_ => throw new NotSupportedException(
				$"Presentation attribute {presentationAttribute.GetType().Name} is not supported in this context")
		};
	}

	public override Task<bool> CloseAsync(IViewModel viewModel)
	{
		var presentationAttribute = this.attributeViewPresenterHelper.GetPresentationAttribute(new(viewModel.GetType()));

		if (presentationAttribute is TPresentationAttribute typedPresentationAttribute)
		{
			return HandleCloseAsync(viewModel, typedPresentationAttribute);
		}

		var result = presentationAttribute switch
		{
			ChildPresentationAttribute childAttribute => CloseChildViewController(viewModel, childAttribute),
			_ => throw new NotSupportedException(
				$"Presentation presentationAttribute {presentationAttribute.GetType().Name} is not supported in this context")
		};

		return Task.FromResult(result);
	}

	protected virtual UIWindow GetWindow() => UIApplication.SharedApplication.ConnectedScenes
		.Cast<UIScene>()
		.Where(x =>
			x is UIWindowScene &&
			x.ActivationState is UISceneActivationState.ForegroundActive
				or UISceneActivationState.ForegroundInactive)
		.Select(x => x as UIWindowScene)
		.SelectMany(x => x!.Windows)
		.FirstOrDefault(x => x.IsKeyWindow) ?? UIApplication.SharedApplication.Delegate.GetWindow();

	protected virtual UINavigationController CreateNavigationController(UIViewController viewController)
		=> new NavigationController(viewController);

	protected virtual UINavigationController? GetNavigationController(UIViewController? viewController = default)
		=> viewController?.NavigationController ?? this.navigationController;

	protected virtual void SetWindowRootViewController(UIViewController controller, TPresentationAttribute? attribute = null)
	{
		var window = GetWindow();

		window.RootViewController = controller;
	}

	protected virtual Task<bool> SetRootViewControllerAsync(UIViewController viewController, TPresentationAttribute attribute)
	{
		if (attribute is IHasWrapInNavigationController { WrapInNavigationController: true })
		{
			this.navigationController = CreateNavigationController(viewController);

			SetWindowRootViewController(this.navigationController, attribute);

			return Task.FromResult(true);
		}

		// if not wrapped, set the view controller as the root
		SetWindowRootViewController(viewController, attribute);

		if (this.navigationController is null)
		{
			return Task.FromResult(true);
		}

		// if previous navigation controller exists, clean it up
		if (attribute is not IHasWrapInNavigationController { WrapInNavigationController: true })
		{
			foreach (var item in this.navigationController.ViewControllers ?? Array.Empty<UIViewController>())
			{
				item.DidMoveToParentViewController(null);
			}

			this.navigationController = null;
		}

		return Task.FromResult(true);
	}

	protected virtual Task<bool> ShowChildViewControllerAsync(
		UIViewController viewController,
		ChildPresentationAttribute attribute)
	{
		var navController = GetNavigationController(viewController);

		if (navController is not null)
		{
			navController.PushViewController(viewController, attribute.Animated);
			return Task.FromResult(true);
		}

		throw new InvalidOperationException(
			$"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
	}

	protected virtual bool CloseChildViewController(IViewModel viewModel, ChildPresentationAttribute attribute)
	{
		var navController = GetNavigationController();

		if (navController is null)
		{
			return false;
		}

		// check for top view controller
		var topView = navController.TopViewController;

		if (topView is IIosView iosView && iosView.ViewModel == viewModel)
		{
			navController.PopViewController(attribute.Animated);

			return true;
		}

		// loop through stack
		var controllers = navController.ViewControllers?.ToList() ?? new();

		var controllerToClose = controllers.SingleOrDefault(vc => vc is IIosView iView && iView.ViewModel == viewModel);

		if (controllerToClose is not null)
		{
			controllers.Remove(controllerToClose);

			navController.ViewControllers = controllers.ToArray();

			return true;
		}

		return false;
	}
}