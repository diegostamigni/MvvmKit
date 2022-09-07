namespace HelloMvvmKit.Ios;

[Register ("SceneDelegate")]
public class SceneDelegate : UIResponder, IUIWindowSceneDelegate {

	[Export ("window")]
	public UIWindow? Window { get; set; }
}
