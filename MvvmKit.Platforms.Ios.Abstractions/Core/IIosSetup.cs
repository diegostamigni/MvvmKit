using MvvmKit.Abstractions.Core;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Platforms.Ios.Abstractions.Core;

public interface IIosSetup : ISetup
{
}

public interface IIosSetup<TMainViewModel> : IIosSetup
	where TMainViewModel : IViewModel
{
}