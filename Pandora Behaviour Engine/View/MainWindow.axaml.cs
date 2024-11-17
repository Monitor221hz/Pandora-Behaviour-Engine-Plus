using FluentAvalonia.UI.Windowing;
using Avalonia.Media;
using Avalonia.Controls.Chrome;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Avalonia.Interactivity;

namespace Pandora.Views;
public partial class MainWindow : AppWindow
{
	public MainWindow()
	{
		InitializeComponent();

		if(IsWindows)
		{
			Color color = Color.Parse("#FF9370DB");
			PlatformFeatures.SetWindowBorderColor(color);
		}

		TitleBar.ExtendsContentIntoTitleBar = true;
		TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
		TitleBar.Height = 42;

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