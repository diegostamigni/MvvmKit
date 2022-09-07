namespace MvvmKit.Extensions.DependencyInjection.Abstractions;

public record NamedParameter(string Name, object Value) : ConstantParameter(Value);