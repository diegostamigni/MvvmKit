using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Abstractions.Views;
using MvvmKit.Platforms.Ios.Views;
using MvvmKit.Presenters;

namespace MvvmKit.Platforms.Ios.Presenters;

public class IosViewPresenter : AttributeViewPresenter, IIosViewPresenter
{
	private readonly IIosViewCreator viewCreator;

	protected IUIApplicationDelegate ApplicationDelegate { get; }

	protected UIWindow Window { get; }

	public UINavigationController? MasterNavigationController { get; protected set; }

	public IosViewPresenter(
		IViewsContainer viewsContainer,
		IViewModelTypeFinder viewModelTypeFinder,
		IIosViewCreator viewCreator,
		IUIApplicationDelegate applicationDelegate,
		UIWindow window) : base(viewsContainer, viewModelTypeFinder)
	{
		this.viewCreator = viewCreator;
		this.ApplicationDelegate = applicationDelegate;
		this.Window = window;
	}

	public override BasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
	{
		if (this.MasterNavigationController == null)
		{
			return new RootPresentationAttribute
			{
				WrapInNavigationController = true,
				ViewType = viewType,
				ViewModelType = viewModelType
			};
		}

		return new ChildPresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
	}

	public override object? CreateOverridePresentationAttributeViewInstance(Type viewType)
		=> this.viewCreator.CreateViewOfType(viewType, null);

	public override void RegisterAttributeTypes()
	{
		this.AttributeTypesToActionsDictionary.Register<RootPresentationAttribute>(
			(viewType, attribute, request) =>
			{
				var viewController = (UIViewController) this.viewCreator.CreateView(request);
				return ShowRootViewControllerAsync(viewController, attribute, request);
			}, CloseRootViewControllerAsync);

		this.AttributeTypesToActionsDictionary.Register<ChildPresentationAttribute>(
			(viewType, attribute, request) =>
			{
				var viewController = (UIViewController) this.viewCreator.CreateView(request);
				return ShowChildViewControllerAsync(viewController, attribute, request);
			}, CloseChildViewControllerAsync);
	}

	public void ClosedPopoverViewController()
	{
		throw new NotImplementedException();
	}

	public Task<bool> ClosedModalViewControllerAsync(
		UIViewController viewController,
		ModalPresentationAttribute attribute)
	{
		throw new NotImplementedException();
	}

	protected virtual Task<bool> CloseRootViewControllerAsync(IViewModel viewModel, RootPresentationAttribute attribute)
		=> Task.FromResult(false);

	protected virtual Task<bool> CloseChildViewControllerAsync(IViewModel viewModel, ChildPresentationAttribute attribute)
	{
		// if the current root is a NavigationController, close it in the stack
		if (this.MasterNavigationController != null &&
		    TryCloseViewControllerInsideStack(this.MasterNavigationController, viewModel, attribute))
		{
			return Task.FromResult(true);
		}

		return Task.FromResult(false);
	}

	protected virtual bool TryCloseViewControllerInsideStack(
		UINavigationController navController,
		IViewModel toClose,
		ChildPresentationAttribute attribute)
	{
		// check for top view controller
		var topView = navController.TopViewController;
		if (topView is IIosView iosView && iosView.ViewModel == toClose)
		{
			navController.PopViewController(attribute.Animated);
			return true;
		}

		// loop through stack
		var controllers = navController.ViewControllers?.ToList();
		var controllerToClose = controllers?.Find(vc => vc is IIosView iView && iView.ViewModel == toClose);
		if (controllerToClose != null)
		{
			controllers!.Remove(controllerToClose);
			navController.ViewControllers = controllers.ToArray();

			return true;
		}

		return false;
	}

	protected void SetupWindowRootNavigation(UIViewController viewController, RootPresentationAttribute attribute)
	{
		if (attribute.WrapInNavigationController)
		{
			this.MasterNavigationController = CreateNavigationController(viewController);

			SetWindowRootViewController(this.MasterNavigationController, attribute);
		}
		else
		{
			SetWindowRootViewController(viewController, attribute);

			CloseMasterNavigationController();
		}
	}

	protected virtual void CloseMasterNavigationController()
	{
		if (this.MasterNavigationController == null)
		{
			return;
		}

		if (this.MasterNavigationController.ViewControllers != null)
		{
			foreach (var item in this.MasterNavigationController.ViewControllers)
			{
				item.DidMoveToParentViewController(null);
			}
		}

		this.MasterNavigationController = null;
	}

	protected virtual NavigationController CreateNavigationController(UIViewController viewController)
		=> new(viewController);

	protected void RemoveWindowSubviews()
	{
		foreach (var v in this.Window.Subviews)
		{
			v.RemoveFromSuperview();
		}
	}

	protected virtual void SetWindowRootViewController(
		UIViewController controller,
		RootPresentationAttribute? attribute = null)
	{
		RemoveWindowSubviews();

		if (attribute == null || attribute.AnimationOptions == UIViewAnimationOptions.TransitionNone)
		{
			this.Window.RootViewController = controller;
			return;
		}

		UIView.Transition(this.Window, attribute.AnimationDuration, attribute.AnimationOptions,
			() => this.Window.RootViewController = controller, null
		);
	}

	protected virtual Task<bool> ShowRootViewControllerAsync(
		UIViewController viewController,
		RootPresentationAttribute attribute,
		ViewModelRequest request)
	{
		return viewController switch
		{
			_ => ShowRootViewControllerAsync(viewController, attribute)
		};
	}

	private Task<bool> ShowRootViewControllerAsync(UIViewController viewController, RootPresentationAttribute attribute)
	{
		SetupWindowRootNavigation(viewController, attribute);
		return Task.FromResult(true);
	}

	protected virtual Task<bool> ShowChildViewControllerAsync(
		UIViewController viewController,
		ChildPresentationAttribute attribute,
		ViewModelRequest request)
	{
		if (this.MasterNavigationController != null)
		{
			PushViewControllerIntoStack(this.MasterNavigationController, viewController, attribute);
			return Task.FromResult(true);
		}

		throw new InvalidOperationException(
			$"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
	}

	protected virtual void PushViewControllerIntoStack(
		UINavigationController navigationController,
		UIViewController viewController,
		ChildPresentationAttribute attribute)
	{
		navigationController.PushViewController(viewController, attribute.Animated);
	}
}