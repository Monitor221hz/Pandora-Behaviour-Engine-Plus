using Avalonia;
using Avalonia.Platform;
using Avalonia.Styling;
using Pandora.Utils;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Pandora.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
	[Reactive] private bool _themeToggleState = Properties.GUISettings.Default.AppTheme == (int)PlatformThemeVariant.Dark;

	public SettingsViewModel()
	{
		ViewModelSettingsHelper.BindSetting(
			this.WhenAnyValue(vm => vm.ThemeToggleState),
			isDark =>
			{
				Application.Current!.RequestedThemeVariant = isDark
					? ThemeVariant.Dark
					: ThemeVariant.Light;

				Properties.GUISettings.Default.AppTheme = isDark ? 1 : 0;
				Properties.GUISettings.Default.Save();
			}
		);
	}
}