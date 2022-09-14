using System.Diagnostics.CodeAnalysis;
using MvvmKit.Presenters;

namespace MvvmKit.Platforms.Ios.Presenters;

public abstract class ViewControllerViewPresenter : ViewPresenter
{
	[SuppressMessage("Performance", "CA1822:Mark members as static")]
	protected UIWindow Window => UIApplication.SharedApplication.ConnectedScenes
		.Cast<UIScene>()
		.Where(x =>
			x is UIWindowScene &&
			x.ActivationState is UISceneActivationState.ForegroundActive
				or UISceneActivationState.ForegroundInactive)
		.Select(x => x as UIWindowScene)
		.SelectMany(x => x!.Windows)
		.FirstOrDefault(x => x.IsKeyWindow) ?? UIApplication.SharedApplication.Delegate.GetWindow();
}