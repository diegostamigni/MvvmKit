namespace MvvmKit.Platforms.Ios.Views;

[AttributeUsage(AttributeTargets.Class)]
public class FromStoryboardAttribute : Attribute
{
	public string? StoryboardName { get; set; }

	public FromStoryboardAttribute(string storyboardName = null)
	{
		this.StoryboardName = storyboardName;
	}
}