// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Core.Settings.DTOs;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Pandora.Core.Settings.SubSettings;

internal sealed class AdvancedSettings : IAdvancedSettings, IDisposable
{
	private readonly BehaviorSubject<bool> _mergeSubject = new(true);

	private AppSettings _appSettings = null!;

	public IObservable<Unit> SaveRequired => _mergeSubject.Skip(1).Select(_ => Unit.Default);

	public void Initialize(AppSettings appSettings)
	{
		_appSettings = appSettings;
		PatchPreferences.MergeAddedEventPropertyArrays = appSettings.MergeAddedEventPropertyArrays;

		if (_mergeSubject.Value != appSettings.MergeAddedEventPropertyArrays)
		{
			_mergeSubject.OnNext(appSettings.MergeAddedEventPropertyArrays);
		}

		_mergeSubject
			.Skip(1)
			.Subscribe(value =>
			{
				_appSettings.MergeAddedEventPropertyArrays = value;
				PatchPreferences.MergeAddedEventPropertyArrays = value;
			});
	}

	public bool MergeAddedEventPropertyArrays
	{
		get => _mergeSubject.Value;
		set
		{
			if (_mergeSubject.Value == value)
				return;
			_appSettings.MergeAddedEventPropertyArrays = value;
			PatchPreferences.MergeAddedEventPropertyArrays = value;
			_mergeSubject.OnNext(value);
		}
	}

	public void Dispose()
	{
		_mergeSubject.Dispose();
	}
}