namespace MvvmKit.Extensions.DependencyInjection.Autofac.Tests.Resources;

internal interface IBar
{
	string Hello();
}

internal class Bar : IBar
{
	private readonly IFoo foo;

	public Bar(IFoo foo)
	{
		this.foo = foo;
	}

	public string Hello() => this.foo.Hello("Bar says hello");
}

internal class BarWithParam : IBar
{
	private readonly IFoo foo;
	private readonly string message;

	public BarWithParam(IFoo foo, string message)
	{
		this.foo = foo;
		this.message = message;
	}

	public string Hello() => this.foo.Hello(this.message);
}