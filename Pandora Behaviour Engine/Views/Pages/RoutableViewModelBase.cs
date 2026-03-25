// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.ViewModels;
using ReactiveUI;

namespace Pandora.Views.Pages;

public abstract class RoutableViewModelBase(IScreen screen) : ViewModelBase, IRoutableViewModel
{
	public abstract string UrlPathSegment { get; }
	public IScreen HostScreen { get; } = screen;
}
