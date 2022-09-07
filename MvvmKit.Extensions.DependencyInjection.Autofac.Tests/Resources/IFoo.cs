namespace MvvmKit.Extensions.DependencyInjection.Autofac.Tests.Resources;

interface IFoo
{
	string Hello(string message);
}

class Foo : IFoo
{
	public string Hello(string message) => message;
}