using Autofac;
using MvvmKit.Extensions.DependencyInjection.Abstractions;
using MvvmKit.Extensions.DependencyInjection.Autofac.Tests.Resources;
using NUnit.Framework;
using Shouldly;
using IMvvmKitContainer = MvvmKit.Extensions.DependencyInjection.Abstractions.IContainer;
using NamedParameter = MvvmKit.Extensions.DependencyInjection.Abstractions.NamedParameter;
using IAutofacContainer = Autofac.IContainer;

namespace MvvmKit.Extensions.DependencyInjection.Autofac.Tests;

[TestFixture]
public class AutofacContainerTests
{
	[Test]
	public void CanResolve()
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<Bar>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());
		adapterContainer.ShouldSatisfyAllConditions
		(
			() => adapterContainer.CanResolve<IFoo>().ShouldBeTrue(),
			() => adapterContainer.CanResolve<IBar>().ShouldBeTrue()
		);
	}

	[Test]
	public void CanResolve_Type()
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<Bar>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());
		adapterContainer.ShouldSatisfyAllConditions
		(
			() => adapterContainer.CanResolve(typeof(IFoo)).ShouldBeTrue(),
			() => adapterContainer.CanResolve(typeof(IBar)).ShouldBeTrue()
		);
	}

	[Test]
	public void Resolve()
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<Bar>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());

		var resolvedType = adapterContainer.Resolve<IBar>();
		resolvedType.ShouldNotBeNull();

		resolvedType.Hello().ShouldNotBeNullOrEmpty();
	}

	[Test]
	public void Resolve_Type()
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<Bar>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());

		var resolvedType = adapterContainer.Resolve(typeof(IBar));
		resolvedType.ShouldSatisfyAllConditions
		(
			() => resolvedType.ShouldNotBeNull(),
			() => resolvedType.ShouldBeAssignableTo<IBar>()
		);

		((IBar) resolvedType).Hello().ShouldNotBeNullOrEmpty();
	}

	[Test]
	public void Resolve_Type_With_NamedParameter()
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<BarWithParam>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());

		var expectedResult = Guid.NewGuid().ToString();
		var resolvedType = adapterContainer.Resolve(typeof(IBar), new NamedParameter("message", expectedResult));
		resolvedType.ShouldSatisfyAllConditions
		(
			() => resolvedType.ShouldNotBeNull(),
			() => resolvedType.ShouldBeAssignableTo<IBar>()
		);

		((IBar) resolvedType).Hello().ShouldBe(expectedResult);
	}

	[TestCase(typeof(UnmappedParameter), default)]
	[TestCase(typeof(UnmappedConstantParameter), 42)]
	public void Resolve_Type_With_UnmappedParameters_Throws(Type parameter, object? value)
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<BarWithParam>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());

		var parameterInstance = value is not null
			? (Parameter) Activator.CreateInstance(parameter, value)!
			: (Parameter) Activator.CreateInstance(parameter)!;

		Should.Throw<NotSupportedException>(() => adapterContainer.Resolve(typeof(IBar), parameterInstance));
	}

	[Test]
	public void Resolve_With_NamedParameter()
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<BarWithParam>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());

		var expectedResult = Guid.NewGuid().ToString();
		var resolvedType = adapterContainer.Resolve<IBar>(new NamedParameter("message", expectedResult));
		resolvedType.ShouldNotBeNull();

		resolvedType.Hello().ShouldBe(expectedResult);
	}

	[TestCase(typeof(UnmappedParameter), default)]
	[TestCase(typeof(UnmappedConstantParameter), 42)]
	public void Resolve_With_UnmappedParameters_Throws(Type parameter, object? value)
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<BarWithParam>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());

		var parameterInstance = value is not null
			? (Parameter) Activator.CreateInstance(parameter, value)!
			: (Parameter) Activator.CreateInstance(parameter)!;

		Should.Throw<NotSupportedException>(() => adapterContainer.Resolve<IBar>(parameterInstance));
	}

	[Test]
	public void TryResolve()
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<Bar>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());

		var canResolve = adapterContainer.TryResolve<IBar>(out var resolvedType);
		canResolve.ShouldBeTrue();
		resolvedType.ShouldNotBeNull();

		resolvedType.Hello().ShouldNotBeNullOrEmpty();
	}

	[Test]
	public void TryResolve_Type()
	{
		var autofacContainerBuilder = new ContainerBuilder();
		autofacContainerBuilder.RegisterType<Foo>().As<IFoo>();
		autofacContainerBuilder.RegisterType<Bar>().As<IBar>();

		IMvvmKitContainer adapterContainer = new AutofacContainer(autofacContainerBuilder.Build());

		var canResolve = adapterContainer.TryResolve(typeof(IBar), out var resolvedType);
		resolvedType.ShouldNotBeNull();
		resolvedType.ShouldSatisfyAllConditions
		(
			() => canResolve.ShouldBeTrue(),
			() => resolvedType.ShouldBeAssignableTo<IBar>()
		);

		((IBar) resolvedType).Hello().ShouldNotBeNullOrEmpty();
	}

	private record UnmappedParameter : Parameter;

	private record UnmappedConstantParameter(object Value) : ConstantParameter(Value);
}