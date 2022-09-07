using System.Reflection;
using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Platforms.Ios.Abstractions.Views;
using MvvmKit.ViewModels;
using MvvmKit.Views;

namespace MvvmKit.Platforms.Ios.Views;

public class IosViewsContainer : ViewsContainer, IIosViewsContainer
{
	public ViewModelRequest? CurrentRequest { get; private set; }

	public virtual IIosView CreateView(ViewModelRequest request)
	{
		try
		{
			this.CurrentRequest = request;

			var viewType = GetViewType(request.ViewModelType);
			if (viewType == null)
			{
				throw new InvalidOperationException("View Type not found for " + request.ViewModelType);
			}

			var view = CreateViewOfType(viewType, request);
			view.Request = request;

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
		if (storyboardAttribute != null)
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
		if (view == null)
		{
			throw new InvalidOperationException("View not loaded for " + viewType);
		}

		return view;
	}

	public virtual IIosView CreateView(IViewModel viewModel)
	{
		var request = new ViewModelInstanceRequest(viewModel);
		var view = CreateView(request);
		return view;
	}
}