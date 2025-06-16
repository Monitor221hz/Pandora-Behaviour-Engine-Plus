using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Pandora.ViewModels;

namespace Pandora.Views;

public partial class LogBox : ReactiveUserControl<EngineViewModel>
{
    public LogBox()
    {
        InitializeComponent();
    }

	private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => 
		LogTextBox.CaretIndex = int.MaxValue;
}