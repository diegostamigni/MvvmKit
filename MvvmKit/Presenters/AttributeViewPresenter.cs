using MvvmKit.Abstractions.Presenters;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.ViewModels;

namespace MvvmKit.Presenters;

public abstract class AttributeViewPresenter : ViewPresenter, IAttributeViewPresenter
{
	private readonly IViewsContainer viewsContainer;

	private IDictionary<Type, PresentationAttributeAction>? attributeTypesActionsDictionary;

	public virtual IDictionary<Type, PresentationAttributeAction> AttributeTypesToActionsDictionary
	{
		get
		{
			if (this.attributeTypesActionsDictionary is null)
			{
				this.attributeTypesActionsDictionary = new Dictionary<Type, PresentationAttributeAction>();
				RegisterAttributeTypes();
			}

			return this.attributeTypesActionsDictionary;
		}
	}

	protected AttributeViewPresenter(IViewsContainer viewsContainer)
	{
		this.viewsContainer = viewsContainer;
	}

	public abstract void RegisterAttributeTypes();

	public abstract BasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType);

	public virtual object? CreateOverridePresentationAttributeViewInstance(Type viewType)
		=> Activator.CreateInstance(viewType);

	public virtual BasePresentationAttribute? GetOverridePresentationAttribute(ViewModelRequest request, Type viewType)
	{
		var hasInterface = viewType.GetInterfaces().Contains(typeof(IOverridePresentationAttribute));
		if (!hasInterface)
		{
			return null;
		}

		var viewInstance = CreateOverridePresentationAttributeViewInstance(viewType) as IOverridePresentationAttribute;
		try
		{
			var presentationAttribute = viewInstance?.PresentationAttribute(request);
			if (presentationAttribute is null)
			{
				return null;
			}

			presentationAttribute.ViewType ??= viewType;
			return presentationAttribute;
		}
		finally
		{
			(viewInstance as IDisposable)?.Dispose();
		}
	}

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

		var overrideAttribute = GetOverridePresentationAttribute(request, viewType);
		if (overrideAttribute is not null)
		{
			return overrideAttribute;
		}

		var attribute = viewType
			.GetCustomAttributes(typeof(BasePresentationAttribute), true)
			.FirstOrDefault();

		if (attribute is BasePresentationAttribute basePresentationAttribute)
		{
			basePresentationAttribute.ViewType ??= viewType;
			return basePresentationAttribute;
		}

		return CreatePresentationAttribute(request.ViewModelType, viewType);
	}

	protected virtual PresentationAttributeAction GetPresentationAttributeAction(
		ViewModelRequest request,
		out BasePresentationAttribute attribute)
	{
		var presentationAttribute = GetPresentationAttribute(request);

		var attributeType = presentationAttribute.GetType();
		attribute = presentationAttribute;

		if (this.AttributeTypesToActionsDictionary.TryGetValue(attributeType, out var attributeAction))
		{
			if (attributeAction.ShowAction is null)
			{
				throw new InvalidOperationException(
					$"attributeAction.ShowAction is null for attribute: {attributeType.Name}");
			}

			if (attributeAction.CloseAction is null)
			{
				throw new InvalidOperationException(
					$"attributeAction.CloseAction is null for attribute: {attributeType.Name}");
			}

			return attributeAction;
		}

		throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
	}

	public override Task<bool> CloseAsync(IViewModel viewModel)
		=> GetPresentationAttributeAction(new ViewModelInstanceRequest(viewModel), out var attribute)
			.CloseAction?
			.Invoke(viewModel, attribute) ?? Task.FromResult(false);

	public override Task<bool> ShowAsync(ViewModelRequest request)
	{
		var attributeAction = GetPresentationAttributeAction(request, out var attribute);

		if (attributeAction.ShowAction is not null && attribute.ViewType is not null)
		{
			return attributeAction.ShowAction.Invoke(attribute.ViewType, attribute, request);
		}

		return Task.FromResult(false);
	}
}