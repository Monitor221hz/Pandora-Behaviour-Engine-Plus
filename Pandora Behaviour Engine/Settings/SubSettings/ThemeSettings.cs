// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Platform.Avalonia;
using Pandora.Settings.DTOs;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Pandora.Settings.SubSettings;

internal sealed class ThemeSettings : IThemeSettings, IDisposable
{
	private readonly BehaviorSubject<AppTheme> _themeSubject = new(AppTheme.System);

	private AppSettings _appSettings = null!;

	public IObservable<AppTheme> ThemeChanged => _themeSubject.AsObservable();

	public IObservable<Unit> SaveRequired => _themeSubject.Skip(1).Select(_ => Unit.Default);

	public void Initialize(AppSettings appSettings)
	{
		_appSettings = appSettings;

		if (_appSettings.Theme != _themeSubject.Value)
		{
			_themeSubject.OnNext(_appSettings.Theme);
		}

		_themeSubject
		   .Skip(1)
		   .Subscribe(theme =>
		   {
			   _appSettings.Theme = theme;
		   });
	}

	public AppTheme Theme
	{
		get => _themeSubject.Value;
		set
		{
			if (_themeSubject.Value == value) return;
			_appSettings.Theme = value;
			_themeSubject.OnNext(value);
		}
	}

	public void Dispose()
	{
		_themeSubject.Dispose();
	}
}
