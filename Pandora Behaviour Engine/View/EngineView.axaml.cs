using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Windowing;
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
		Loaded += EngineViewLoaded!;
	}
	private async void EngineViewLoaded(object sender, RoutedEventArgs e)
	{
		await _viewModel.LoadAsync();
	}
}