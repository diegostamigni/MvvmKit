using Autofac;
using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Presenters;

public class IosViewPresenterResolver : IViewPresenterResolver
{
	private readonly ILifetimeScope lifetimeScope;
	private readonly IAttributeViewPresenterHelper attributeViewPresenterHelper;

	private BasePresentationAttribute? currentPresentationAttribute;

	public IosViewPresenterResolver(
		ILifetimeScope lifetimeScope,
		IAttributeViewPresenterHelper attributeViewPresenterHelper)
	{
		this.lifetimeScope = lifetimeScope;
		this.attributeViewPresenterHelper = attributeViewPresenterHelper;
	}

	public IViewPresenter Resolve(ViewModelRequest request)
	{
		if (this.currentPresentationAttribute is not null)
		{
			return GetViewPresenter(this.currentPresentationAttribute);
		}

		var attribute = this.attributeViewPresenterHelper.GetPresentationAttribute(request);
		if (attribute is not null and not ChildPresentationAttribute)
		{
			this.currentPresentationAttribute = attribute;
		}

		return GetViewPresenter(attribute);
	}

	private IViewPresenter GetViewPresenter<TPresentationAttribute>(TPresentationAttribute? presentationAttribute)
		where TPresentationAttribute : BasePresentationAttribute
	{
		return presentationAttribute switch
		{
			NavigationPresentationAttribute => this.lifetimeScope.Resolve<INavigationControllerViewPresenter>(),
			SplitViewPresentationAttribute => this.lifetimeScope.Resolve<ISplitViewControllerViewPresenter>(),
			ModalPresentationAttribute => this.lifetimeScope.Resolve<IModalViewControllerViewPresenter>(),
			TabPresentationAttribute => this.lifetimeScope.Resolve<ITabBarViewControllerViewPresenter>(),
			_ => throw new NotSupportedException(
				$"Presentation attribute {presentationAttribute?.GetType().Name} is not supported")
		};
	}
}