using Avalonia.ReactiveUI;
using Pandora.ViewModels;

namespace Pandora.Views;

public partial class LogBox : ReactiveUserControl<EngineViewModel>
{
    public LogBox()
    {
        InitializeComponent();
    }
}