// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Pandora.ViewModels;

public partial class ModInfoViewModel
	: ViewModelBase,
		IEquatable<ModInfoViewModel>,
		IActivatableViewModel, // maybe not needed
		IDisposable
{
	public IModInfo ModInfo { get; }

	public ViewModelActivator Activator { get; } = new();

	private readonly CompositeDisposable _disposables = new();

	public ModInfoViewModel(IModInfo modInfo)
	{
		ModInfo = modInfo;

		this.WhenAnyValue(x => x.Priority)
			.Subscribe(val => ModInfo.Priority = val)
			.DisposeWith(_disposables);

		this.WhenAnyValue(x => x.Active)
			.Subscribe(val => ModInfo.Active = val)
			.DisposeWith(_disposables);
	}

	public void Dispose() => _disposables.Dispose();

	public string Name => ModInfo.Name;
	public string Author => ModInfo.Author;
	public string URL => ModInfo.URL;
	public string Code => ModInfo.Code;
	public Version Version => ModInfo.Version;
	public DirectoryInfo Folder => ModInfo.Folder;
	public IModInfo.ModFormat Format => ModInfo.Format;

	[Reactive]
	private bool _active;

	[Reactive]
	private uint _priority;

	public bool Equals(ModInfoViewModel? other)
	{
		return other != null && this.ModInfo.Equals(other.ModInfo);
	}

	public override int GetHashCode()
	{
		return ModInfo.GetHashCode();
	}
}