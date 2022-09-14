using MvvmKit.Abstractions.Presenters;
using MvvmKit.Enums;

namespace MvvmKit.Platforms.Ios.Abstractions.Presenters;

public class SplitViewPresentationAttribute : BasePresentationAttribute
{
	public bool WrapInNavigationController { get; set; } = true;

	public MasterDetailPosition Position { get; set; }

	public SplitViewPresentationAttribute(Type viewModelType, MasterDetailPosition position = MasterDetailPosition.Detail)
		: base(viewModelType)
	{
		this.Position = position;

		// If this page is to be the master, the default behaviour should be that the page is not wrapped
		// in a navigation page. This is not the case for Root or Detail pages where default behaviour
		// would be to support navigation
		if (position == MasterDetailPosition.Master)
		{
			this.WrapInNavigationController = false;
		}
	}
}