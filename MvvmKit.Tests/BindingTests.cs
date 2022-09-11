using MvvmKit.Abstractions.Commands;
using MvvmKit.Abstractions.Converters;
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

	[Test]
	public void Binding_String_To_Int_Apply_Throws()
	{
		var source = new Source
		{
			Age = "10"
		};

		var destination = new Destination();

		var bindings = Binding<Source, Destination>.Create(source, destination);
		bindings.For(vm => vm.Age).To(v => v.Age);

		Should.Throw<ArgumentException>(bindings.Apply);
	}

	[Test]
	public void Binding_String_To_Int_With_Converter_Apply()
	{
		var source = new Source
		{
			Age = "10"
		};

		var destination = new Destination();

		var bindings = Binding<Source, Destination>.Create(source, destination);
		bindings.For(vm => vm.Age).To(v => v.Age).WithConversion<AgeValueConverter>();
		bindings.Apply();

		destination.Age.ShouldBe(10);
	}

	private class Source : Bindable
	{
		public string? Name
		{
			get => Get<string>();
			set => Set(value);
		}

		public string? Age
		{
			get => Get<string>();
			set => Set(value);
		}

		public string? Date
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

		public int? Age { get; set; }

		public DateTime? Date { get; set; }

		public ICommand? Command { get; set; }
	}

	private class AgeValueConverter : IValueConverter<string?, int?>
	{
		public int? Convert(string? value) => int.TryParse(value, out var age) ? age : null;
	}
}
