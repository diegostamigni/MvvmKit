using MvvmKit.Abstractions.View;

namespace MvvmKit.Platforms.Ios.Abstractions.Views;

public interface ICurrentRequest
{
	ViewModelRequest? CurrentRequest { get; }
}