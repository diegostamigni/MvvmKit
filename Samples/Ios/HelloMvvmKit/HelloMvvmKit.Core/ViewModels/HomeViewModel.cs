using MvvmKit.Abstractions.Commands;
using MvvmKit.Abstractions.Navigation;
using MvvmKit.Commands;
using MvvmKit.ViewModels;

namespace HelloMvvmKit.Core.ViewModels;

public class HomeViewModel : NavigationViewModel
{
	public ICommand NavigateNextCommand { get; }
	public ICommand LoadImageCommand { get; }

	public string? Title
	{
		get => Get<string>();
		set => Set(value);
	}

	public bool? IsLoading
	{
		get => Get<bool>();
		set => Set(value);
	}

	public byte[]? ImageContent
	{
		get => Get<byte[]>();
		set => Set(value);
	}

	public HomeViewModel(INavigationService navigationService)
		: base(navigationService)
	{
		this.NavigateNextCommand = new AsyncCommand(NavigateNextAsync, CanNextAsync);
		this.LoadImageCommand = new AsyncCommand(LoadImageAsync, CanLoadImageAsync);
	}

	public override void ViewAppearing()
	{
		base.ViewAppearing();

		this.Title = GetType().Name;
	}

	private static bool CanNextAsync() => true;

	private static bool CanLoadImageAsync() => true;

	private Task NavigateNextAsync(CancellationToken token)
		=> this.NavigationService.NavigateAsync<FirstChildViewModel>(token);

	private async Task LoadImageAsync(CancellationToken token)
	{
		const string imageUrl =
			"https://unsplash.com/photos/T5MfWLO1bFs/download?ixid=MnwxMjA3fDB8MXxhbGx8Mnx8fHx8fDJ8fDE2NjI5MTU2MDY&force=true&w=1920";

		this.IsLoading = true;
		try
		{
			using var httpClient = new HttpClient();

			var result = await httpClient.GetByteArrayAsync(imageUrl, token);

			this.ImageContent = result;
		}
		finally
		{
			this.IsLoading = false;
		}
	}
}