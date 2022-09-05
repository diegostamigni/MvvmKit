using MvvmKit.Abstractions.ViewModels;
using MvvmKit.Enums.Navigation;

namespace MvvmKit.Abstractions.Navigation;

public record NavigateArgs
{
	public bool Cancel { get; init; }

	public NavigationMode Mode { get; init; }

	public IViewModel? ViewModel { get; init; }
}