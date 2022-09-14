using Autofac;
using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;

namespace MvvmKit.Platforms.Ios.Presenters;

public class IosViewPresenterResolver : IViewPresenterResolver
{
	private readonly ILifetimeScope lifetimeScope;
	private readonly IAttributeViewPresenterHelper attributeViewPresenterHelper;

	public IosViewPresenterResolver(
		ILifetimeScope lifetimeScope,
		IAttributeViewPresenterHelper attributeViewPresenterHelper)
	{
		this.lifetimeScope = lifetimeScope;
		this.attributeViewPresenterHelper = attributeViewPresenterHelper;
	}

	public IViewPresenter Resolve(ViewModelRequest request)
	{
		var attribute = this.attributeViewPresenterHelper.GetPresentationAttribute(request);

		return attribute switch
		{
			RootPresentationAttribute or ChildPresentationAttribute
				=> this.lifetimeScope.Resolve<INavigationControllerViewPresenter>(),

			_ => throw new NotSupportedException($"Presentation attribute {attribute.GetType().Name} is not supported")
		};
	}
}