// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.IO;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Pandora.API.Patch;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Pandora.ViewModels;

public partial class ModInfoViewModel : ViewModelBase, IEquatable<ModInfoViewModel>, IActivatableViewModel
{
	public IModInfo ModInfo { get; }

	public ViewModelActivator Activator { get; } = new();

	public ModInfoViewModel(IModInfo modInfo)
	{
		ModInfo = modInfo;

		this.WhenActivated(disposables =>
		{
			this.WhenAnyValue(x => x.Active)
				.DistinctUntilChanged()
				.Subscribe(val => ModInfo.Active = val)
				.DisposeWith(disposables);

			this.WhenAnyValue(x => x.Priority)
				.DistinctUntilChanged()
				.Subscribe(val => ModInfo.Priority = val)
				.DisposeWith(disposables);
		});
	}

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
