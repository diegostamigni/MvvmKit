using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace MvvmKit.ViewModels.DependencyInjection;

public class ViewModelModule : Module
{
	private readonly Assembly appAssembly;

	public ViewModelModule(Type applicationType)
	{
		this.appAssembly = applicationType.Assembly;
	}

	protected override void Load(ContainerBuilder builder)
	{
		base.Load(builder);

		builder.RegisterAssemblyTypes(this.appAssembly)
			.Where(t => t.Name.EndsWith("ViewModel", StringComparison.InvariantCultureIgnoreCase))
			.AsSelf();
	}
}