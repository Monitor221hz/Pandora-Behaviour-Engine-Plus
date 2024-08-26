using Avalonia.Controls;

namespace Pandora.Views;
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		var savedWindowHeight = Properties.GUISettings.Default.WindowHeight;
		var savedWindowWidth = Properties.GUISettings.Default.WindowWidth;
		Height = savedWindowHeight > 1 ? savedWindowHeight : Height;
		Width = savedWindowWidth > 1 ? savedWindowWidth : Width;
		Closed += MainWindow_Closed;
	}
	private void MainWindow_Closed(object? sender, System.EventArgs e)
	{
		Properties.GUISettings.Default.WindowHeight = Height;
		Properties.GUISettings.Default.WindowWidth = Width;
		Properties.GUISettings.Default.Save();
	}

}