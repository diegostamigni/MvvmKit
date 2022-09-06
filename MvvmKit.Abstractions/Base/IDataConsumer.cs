namespace MvvmKit.Abstractions.Base;

public interface IDataConsumer
{
	object? DataContext { get; set; }
}