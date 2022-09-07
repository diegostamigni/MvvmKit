using MvvmKit.Abstractions.View;
using MvvmKit.Abstractions.ViewModels;

namespace MvvmKit.Platforms.Ios.Abstractions.Views;

public interface IIosViewCreator : ICurrentRequest
{
	IIosView CreateView(ViewModelRequest request);

	IIosView CreateView(IViewModel viewModel);

	IIosView CreateViewOfType(Type viewType, ViewModelRequest? request);
}