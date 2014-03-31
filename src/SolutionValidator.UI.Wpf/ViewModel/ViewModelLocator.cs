using SolutionValidator.Core.Infrastructure.DependencyInjection;

namespace SolutionValidator.UI.Wpf.ViewModel
{
	/// <summary>
	///     This class contains static references to all the view models in the
	///     application and provides an entry point for the bindings.
	/// </summary>
	public class ViewModelLocator
	{
		public MainViewModel Main
		{
			get { return Dependency.Resolve<MainViewModel>(); }
		}


		public static void Cleanup()
		{
			// TODO Clear the ViewModels
		}
	}
}