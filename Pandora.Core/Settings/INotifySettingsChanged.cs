// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Reactive;

namespace Pandora.Core.Settings;

public interface INotifySettingsChanged
{
	IObservable<Unit> SaveRequired { get; }
}