using ReactiveUI;
using Splat;

namespace OlibKey.ViewModels.Pages
{
	public class StartPageViewModel : ReactiveObject, IRoutableViewModel
	{
		// routing
		public string UrlPathSegment => "/startPage";

		public IScreen HostScreen { get; }

		public StartPageViewModel(IScreen screen = null) => HostScreen = screen ?? Locator.Current.GetService<IScreen>();
	}
}
