namespace MvvmKit.Core;

public static class RegistrationHelper
{
	public static readonly HashSet<string> RegistrationSuffixes = new()
	{
		"Service",
		"Repository",
		"Loader",
		"Dispatcher",
		"Manager",
		"Resolver",
		"Helper",
	};
}