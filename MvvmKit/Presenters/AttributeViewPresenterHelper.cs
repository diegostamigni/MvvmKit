using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;

namespace MvvmKit.Presenters;

public class AttributeViewPresenterHelper : IAttributeViewPresenterHelper
{
	private readonly IViewsContainer viewsContainer;

	public AttributeViewPresenterHelper(IViewsContainer viewsContainer) => this.viewsContainer = viewsContainer;

	public virtual BasePresentationAttribute GetPresentationAttribute(ViewModelRequest request)
	{
		if (request.ViewModelType is null)
		{
			throw new InvalidOperationException("Cannot get view types for null ViewModelType");
		}

		var viewType = this.viewsContainer.GetViewType(request.ViewModelType);
		if (viewType is null)
		{
			throw new InvalidOperationException($"Could not get View Type for ViewModel Type {request.ViewModelType}");
		}

		var attribute = viewType
			.GetCustomAttributes(typeof(BasePresentationAttribute), true)
			.FirstOrDefault();

		if (attribute is BasePresentationAttribute basePresentationAttribute)
		{
			basePresentationAttribute.ViewType ??= viewType;
			return basePresentationAttribute;
		}

		throw new InvalidOperationException($"Could not find presentation attribute for {viewType.Name}");
	}
}