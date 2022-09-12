using System.Reflection;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Abstractions.Views;
using MvvmKit.ViewModels;
using MvvmKit.Views;

namespace MvvmKit.Platforms.Ios.Views;

public class IosViewsContainer : ViewsContainer, IIosViewsContainer
{
	private readonly Dictionary<Type, Dictionary<UIUserInterfaceIdiom, Type>> bindingMap = new();

	public ViewModelRequest? CurrentRequest { get; private set; }

	public virtual IIosView CreateView(ViewModelRequest request)
	{
		try
		{
			this.CurrentRequest = request;

			var viewType = GetViewType(request.ViewModelType);
			if (viewType is null)
			{
				throw new InvalidOperationException("View Type not found for " + request.ViewModelType);
			}

			var view = CreateViewOfType(viewType, request);
			view.Request = request;

			if (request is ViewModelInstanceRequest viewModelInstanceRequest)
			{
				view.ViewModel = viewModelInstanceRequest.ViewModelInstance;
			}

			return view;
		}
		finally
		{
			this.CurrentRequest = null;
		}
	}

	public virtual IIosView CreateViewOfType(Type viewType, ViewModelRequest? request)
	{
		var storyboardAttribute = viewType.GetCustomAttribute<FromStoryboardAttribute>();
		if (storyboardAttribute is not null)
		{
			var storyboardName = storyboardAttribute.StoryboardName ?? viewType.Name;
			try
			{
				var storyboard = UIStoryboard.FromName(storyboardName, null);
				var viewController = storyboard.InstantiateViewController(viewType.Name);
				return (IIosView)viewController;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(
					$"Loading view of type {viewType.Name} from storyboard {storyboardName} failed: {ex.Message}", ex);
			}
		}

		var view = Activator.CreateInstance(viewType) as IIosView;
		if (view is null)
		{
			throw new InvalidOperationException("View not loaded for " + viewType);
		}

		view.Request = request;

		if (request is ViewModelInstanceRequest viewModelInstanceRequest)
		{
			view.ViewModel = viewModelInstanceRequest.ViewModelInstance;
		}

		return view;
	}

	public virtual IIosView CreateView(IViewModel viewModel)
	{
		var request = new ViewModelInstanceRequest(viewModel);
		var view = CreateView(request);
		return view;
	}

	public override void Add(Type viewModelType, Type viewType)
	{
		var platformPresentationAttrs = viewType.GetCustomAttributes<SupportedByAttribute>()
			.Select(attr => attr.UserInterfaceIdiom)
			.ToList();

		if (platformPresentationAttrs.Count == 0)
		{
			base.Add(viewModelType, viewType);
			return;
		}

		foreach (var appleDeviceType in platformPresentationAttrs)
		{
			if (this.bindingMap.TryGetValue(viewModelType, out var values))
			{
				values[appleDeviceType] = viewType;
			}
			else
			{
				this.bindingMap[viewModelType] = new()
				{
					{ appleDeviceType, viewType }
				};
			}
		}
	}

	public override Type GetViewType(Type? viewModelType)
	{
		if (viewModelType is not null && this.bindingMap.TryGetValue(viewModelType, out var values))
		{
			if (values.TryGetValue(UIDevice.CurrentDevice.UserInterfaceIdiom, out var type))
			{
				return type;
			}

			if (values.TryGetValue(UIUserInterfaceIdiom.Unspecified, out type))
			{
				return type;
			}
		}

		return base.GetViewType(viewModelType);
	}
}