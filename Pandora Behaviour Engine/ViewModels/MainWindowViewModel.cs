namespace Pandora.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
	public EngineViewModel EngineVM { get; } = new EngineViewModel();
}
