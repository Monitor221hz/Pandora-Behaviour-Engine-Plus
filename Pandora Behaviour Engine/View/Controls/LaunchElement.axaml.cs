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