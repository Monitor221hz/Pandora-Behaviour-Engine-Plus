// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Windowing;
using Pandora.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace Pandora.Views;

public partial class EngineMenu : ReactiveUserControl<EngineViewModel>
{
    public EngineMenu()
    {
        InitializeComponent();

		this.WhenActivated(disposables =>
        {
            this.BindInteraction(
                ViewModel,
                vm => vm.ShowAboutDialog,
                async context =>
                {
					await AboutDialog.ShowAsync(true);
					context.SetOutput(Unit.Default);
				});
		});
    }
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (VisualRoot is AppWindow aw)
        {
            TitleBarHost.ColumnDefinitions[4].Width = new GridLength(aw.TitleBar.RightInset, GridUnitType.Pixel);
        }
    }
}
