// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Core.Settings.DTOs;

namespace Pandora.Core.Settings.SubSettings;

public interface IAdvancedSettings : INotifySettingsChanged
{
	bool MergeAddedEventPropertyArrays { get; set; }

	void Initialize(AppSettings appSettings);
}