// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

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