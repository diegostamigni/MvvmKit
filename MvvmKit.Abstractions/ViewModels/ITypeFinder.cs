namespace MvvmKit.Abstractions.ViewModels;

public interface ITypeFinder
{
	Type? FindTypeOrNull(Type candidateType);
}