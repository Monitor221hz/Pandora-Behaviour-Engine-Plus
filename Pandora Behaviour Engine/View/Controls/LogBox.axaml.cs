using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pandora.Views;

public partial class LogBox : UserControl
{
    public LogBox()
    {
        InitializeComponent();
    }
	private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		LogTextBox.CaretIndex = int.MaxValue;
	}
}