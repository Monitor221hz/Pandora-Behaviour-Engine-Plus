using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Pandora.ViewModels;

namespace Pandora.Views;

public partial class LaunchElement : ReactiveUserControl<EngineViewModel>
{
    public LaunchElement()
    {
        InitializeComponent();
    }
}