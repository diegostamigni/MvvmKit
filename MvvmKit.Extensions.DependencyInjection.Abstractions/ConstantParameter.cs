using System.Diagnostics.CodeAnalysis;

namespace MvvmKit.Extensions.DependencyInjection.Abstractions;

[ExcludeFromCodeCoverage]
public abstract record ConstantParameter(object Value) : Parameter;