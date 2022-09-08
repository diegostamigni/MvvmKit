using System.Reflection;
using Autofac;
using MvvmKit.Abstractions.Presenters;
using MvvmKit.Platforms.Ios.Abstractions.Views;
using Module = Autofac.Module;

namespace MvvmKit.Platforms.Ios.Views.DependencyInjection;

public class ViewsModule : Module
{
	private readonly Assembly setupAssembly;

	public ViewsModule(Type setupType)
	{
		this.setupAssembly = setupType.Assembly;
	}

	protected override void Load(ContainerBuilder builder)
	{
		base.Load(builder);

		builder.RegisterType<IosViewsContainer>()
			.AsImplementedInterfaces()
			.SingleInstance();

		var viewControllersByViewModel = this.setupAssembly.GetTypes()
			.Where(x => x.IsClass &&
				!x.IsAbstract &&
				typeof(IIosView).IsAssignableFrom(x) &&
				x.IsDefined(typeof(BasePresentationAttribute)))
			.Select(x => new
			{
				ViewControllerType = x,
				x.GetCustomAttribute<BasePresentationAttribute>()!.ViewModelType
			})
			.ToDictionary(key => key.ViewModelType, value => value.ViewControllerType);

		builder.RegisterBuildCallback(container =>
		{
			var viewsContainer = container.Resolve<IIosViewsContainer>();
			viewsContainer.AddAll(viewControllersByViewModel);
		});
	}
}