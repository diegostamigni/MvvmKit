using System.Reflection;
using Autofac;
using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;

namespace MvvmKit.Platforms.Ios.Presenters;

public class IosViewPresenterResolver : IViewPresenterResolver
{
	private readonly ILifetimeScope lifetimeScope;
	private readonly IAttributeViewPresenterHelper attributeViewPresenterHelper;

	private IViewPresenter? currentPresenter;

	public IosViewPresenterResolver(
		ILifetimeScope lifetimeScope,
		IAttributeViewPresenterHelper attributeViewPresenterHelper)
	{
		this.lifetimeScope = lifetimeScope;
		this.attributeViewPresenterHelper = attributeViewPresenterHelper;
	}

	public IViewPresenter Resolve(ViewModelRequest request)
	{
		if (this.currentPresenter is not null)
		{
			return this.currentPresenter;
		}

		var attribute = this.attributeViewPresenterHelper.GetPresentationAttribute(request);

		var presenter = GetViewPresenter(attribute);

		this.currentPresenter = presenter ?? throw new InvalidOperationException(
			$"No valid presenter found for {request.ViewModelType} with attribute {attribute}");

		return presenter;
	}

	private IViewPresenter? GetViewPresenter<TPresentationAttribute>(TPresentationAttribute? presentationAttribute)
		where TPresentationAttribute : BasePresentationAttribute
	{
		var presenterAttr = presentationAttribute?.GetType().GetCustomAttribute<ViewPresenterAttribute>();

		if (presenterAttr is null)
		{
			return null;
		}

		return this.lifetimeScope.Resolve(presenterAttr.PresenterType) as IViewPresenter;
	}
}