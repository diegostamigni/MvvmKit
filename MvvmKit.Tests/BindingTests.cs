using MvvmKit.Abstractions.Commands;
using MvvmKit.Bindings;
using MvvmKit.Commands;
using NUnit.Framework;
using Shouldly;

namespace MvvmKit.Tests;

[TestFixture]
public class BindingTests
{
	[Test]
	public void Binding_StringProperty_Apply()
	{
		var source = new Source
		{
			Name = "Test"
		};

		var destination = new Destination();

		var bindings = Binding<Source, Destination>.Create(source, destination);
		bindings.For(vm => vm.Name).To(v => v.Name);
		bindings.Apply();

		destination.Name.ShouldBe("Test");
	}

	[Test]
	public void Binding_StringProperty_NotifyPropertyChanged()
	{
		var source = new Source
		{
			Name = "Test"
		};

		var destination = new Destination();

		var bindings = Binding<Source, Destination>.Create(source, destination);
		bindings.For(vm => vm.Name).To(v => v.Name);

		source.Name = "Update";
		destination.Name.ShouldBe("Update");
	}

	[Test]
	public void Binding_StringProperty_Apply_NotifyPropertyChanged()
	{
		var source = new Source
		{
			Name = "Test"
		};

		var destination = new Destination();

		var bindings = Binding<Source, Destination>.Create(source, destination);
		bindings.For(vm => vm.Name).To(v => v.Name);
		bindings.Apply();

		source.Name = "Update";
		destination.Name.ShouldBe("Update");
	}

	[Test]
	public void Binding_ICommand_Apply_NotifyPropertyChanged()
	{
		var source = new Source();
		source.Command = new Command(() =>
		{
			source.Name = "UpdateFromCommand";
		});

		var destination = new Destination();

		var bindings = Binding<Source, Destination>.Create(source, destination);
		bindings.For(vm => vm.Name).To(v => v.Name);
		bindings.For(vm => vm.Command).To(v => v.Command);
		bindings.Apply();

		destination.Command?.Execute();
		destination.Name.ShouldBe("UpdateFromCommand");
	}

	private class Source : Bindable
	{
		public string? Name
		{
			get => Get<string>();
			set => Set(value);
		}

		public ICommand? Command
		{
			get => Get<ICommand>();
			set => Set(value);
		}
	}

	private class Destination
	{
		public string? Name { get; set; }

		public ICommand? Command { get; set; }
	}
}
