using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Pandora.ViewModels;

namespace Pandora.Views;

public partial class EngineMenu : UserControl
{
    public EngineMenu()
    {
        InitializeComponent();

        var viewModel = new EngineViewModel
        {
            GetTaskDialogAbout = () => this.FindControl<TaskDialog>("TaskDialogAbout"),
            GetDialogUpdateAvaliable = () => this.FindControl<ContentDialog>("DialogUpdateAvaliable")
        };

        DataContext = viewModel;
    }
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (VisualRoot is AppWindow aw)
        {
            TitleBarHost.ColumnDefinitions[3].Width = new GridLength(aw.TitleBar.RightInset, GridUnitType.Pixel);
        }
    }
}