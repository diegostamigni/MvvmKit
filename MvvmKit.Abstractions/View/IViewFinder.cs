namespace MvvmKit.Abstractions.View;

public interface IViewFinder
{
	Type? GetViewType(Type? viewModelType);
}