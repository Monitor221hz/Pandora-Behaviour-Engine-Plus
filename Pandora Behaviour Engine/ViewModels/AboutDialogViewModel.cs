// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Utils;

namespace Pandora.ViewModels;

public class AboutDialogViewModel : ViewModelBase
{
	public static string SubHeader => $"Version: {AppInfo.Version}";
	public static string Header => AppInfo.Name;
	public static string Content =>
		"Behaviour engine tool for patching Skyrim Nemesis/FNIS behaviour mods, with full creature support.";

	public AboutDialogViewModel() { }
}
