// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Utils;
using Pandora.Views.Pages;
using Pandora.Views.Pages.DTOs;
using ReactiveUI;

namespace Pandora.ViewModels;

public partial class AboutPageViewModel(IScreen screen) : RoutableViewModelBase(screen)
{
	public override string UrlPathSegment => Routes.About;

	public static string SubHeader => $"Version: {AppInfo.Version}";
	public static string Header => AppInfo.Name;
	public static string Content =>
		"Behaviour engine tool for patching Skyrim Nemesis/FNIS behaviour mods, with full creature support.";
}
