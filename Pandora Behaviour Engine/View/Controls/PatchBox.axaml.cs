using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pandora.Views;

public partial class PatchBox : UserControl
{
    public PatchBox()
    {
        InitializeComponent();
		ModInfoDataGrid.AttachedToVisualTree += ModInfoDataGrid_AttachedToVisualTree;
		ModInfoDataGrid.DetachedFromLogicalTree += ModInfoDataGrid_DetachedFromLogicalTree;
    }

	private void ModInfoDataGrid_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
	{
		var savedColumn1 = Properties.GUISettings.Default.PatchBoxNameColumnWidth;
		var savedColumn2 = Properties.GUISettings.Default.PatchBoxAuthorColumnWidth;
		var savedColumn3 = Properties.GUISettings.Default.PatchBoxVersionColumnWidth;
		ModInfoDataGrid.Columns[1].Width = savedColumn1 > 1 ? new DataGridLength(savedColumn1) : ModInfoDataGrid.Columns[1].Width;
		ModInfoDataGrid.Columns[2].Width = savedColumn2 > 1 ? new DataGridLength(savedColumn2) : ModInfoDataGrid.Columns[2].Width;
		ModInfoDataGrid.Columns[3].Width = savedColumn3 > 1 ? new DataGridLength(savedColumn3) : ModInfoDataGrid.Columns[3].Width;
	}

	private void ModInfoDataGrid_DetachedFromLogicalTree(object? sender, Avalonia.LogicalTree.LogicalTreeAttachmentEventArgs e)
	{
		Properties.GUISettings.Default.PatchBoxNameColumnWidth = ModInfoDataGrid.Columns[1].Width.Value;
		Properties.GUISettings.Default.PatchBoxAuthorColumnWidth = ModInfoDataGrid.Columns[2].Width.Value;
		Properties.GUISettings.Default.PatchBoxVersionColumnWidth = ModInfoDataGrid.Columns[3].Width.Value;
	}
}