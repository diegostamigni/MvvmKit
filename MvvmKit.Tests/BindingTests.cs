using MvvmKit.Bindings;
using NUnit.Framework;
using Shouldly;

namespace MvvmKit.Tests;

[TestFixture]
public class BindingTests
{
	[Test]
	public void Binding_Apply()
	{
		var source = new Source("Test");
		var destination = new Destination();

		var bindings = Binding<Source, Destination>.Create(source, destination);
		bindings.Bind(vm => vm.Name, v => v.Name);
		bindings.Apply();

		destination.Name.ShouldBe("Test");
	}

	[Test]
	public void Binding_NotifyPropertyChanged()
	{
		var source = new Source("Test");
		var destination = new Destination();

		var bindings = Binding<Source, Destination>.Create(source, destination);
		bindings.Bind(vm => vm.Name, v => v.Name);

		source.Name = "Update";
		destination.Name.ShouldBe("Update");
	}

	[Test]
	public void Binding_Apply_NotifyPropertyChanged()
	{
		var source = new Source("Test");
		var destination = new Destination();

		var bindings = Binding<Source, Destination>.Create(source, destination);
		bindings.Bind(vm => vm.Name, v => v.Name);
		bindings.Apply();

		source.Name = "Update";
		destination.Name.ShouldBe("Update");
	}

	private class Source : Bindable
	{
		public string? Name
		{
			get => Get<string>();
			set => Set(value);
		}

		public Source(string? name = default) => this.Name = name;
	}

	private record Destination(string? Name = default);
}
