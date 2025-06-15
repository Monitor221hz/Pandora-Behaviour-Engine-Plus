using Avalonia.Controls;
using Avalonia.Interactivity;
using Pandora.ViewModels;

namespace Pandora.Views;

public partial class EngineView : UserControl
{
	private EngineViewModel _viewModel;
	public EngineView()
    {
        InitializeComponent();
		_viewModel = new EngineViewModel();
		DataContext = _viewModel;
	}
}