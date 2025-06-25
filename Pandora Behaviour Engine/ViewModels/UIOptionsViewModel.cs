using Avalonia.Controls;
using Avalonia.Platform;
using Pandora.Utils;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Pandora.ViewModels;

public partial class UIOptionsViewModel : ViewModelBase
{
	[Reactive] private bool? _themeToggleState;
	[Reactive] private bool _isCompactRowHeight;
	[Reactive] private DataGridGridLinesVisibility _gridLinesVisibility;

	public UIOptionsViewModel()
	{
		InitializeProperties();
		InitializeSubscriptions();
	}

	private void InitializeProperties()
	{
		IsCompactRowHeight = Properties.GUISettings.Default.IsCompactRowHeight;
		GridLinesVisibility = (DataGridGridLinesVisibility)Properties.GUISettings.Default.GridLinesVisibility;
		ThemeToggleState = (PlatformThemeVariant)Properties.GUISettings.Default.AppTheme == PlatformThemeVariant.Light;

		Debug.WriteLine($"[Init] IsCompactRowHeight = {Properties.GUISettings.Default.IsCompactRowHeight}");
		Debug.WriteLine($"[Init] GridLinesVisibility = {Properties.GUISettings.Default.GridLinesVisibility}");

	}

	private void InitializeSubscriptions()
	{
		ViewModelSettingsHelper.BindSetting(
			this.WhenAnyValue(vm => vm.IsCompactRowHeight),
			val => Properties.GUISettings.Default.IsCompactRowHeight = val
		);

		ViewModelSettingsHelper.BindSetting(
			this.WhenAnyValue(vm => vm.GridLinesVisibility),
			val => Properties.GUISettings.Default.GridLinesVisibility = (int)val
		);

		ViewModelSettingsHelper.BindSetting(
			this.WhenAnyValue(vm => vm.ThemeToggleState),
			val =>
			{
				var theme = val == true
					? (int)PlatformThemeVariant.Light
					: (int)PlatformThemeVariant.Dark;

				Properties.GUISettings.Default.AppTheme = theme;
			}
		);
	}

	[ReactiveCommand]
	private void ToggleTheme(bool? isChecked)
	{
		ThemeToggleState = isChecked;
		if (isChecked is bool checkedValue)
		{
			var theme = checkedValue ? PlatformThemeVariant.Light : PlatformThemeVariant.Dark;
			AvaloniaServices.ApplyTheme(theme);
			Properties.GUISettings.Default.AppTheme = (int)theme;
			Properties.GUISettings.Default.Save();
		}
	}
}