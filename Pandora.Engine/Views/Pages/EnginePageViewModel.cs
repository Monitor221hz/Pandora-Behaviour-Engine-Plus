// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Pandora.API.Patch;
using Pandora.Configuration;
using Pandora.Configuration.ViewModels;
using Pandora.Core.Configuration;
using Pandora.Core.Logging.Extensions;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Mods;
using Pandora.Views.Pages;
using Pandora.Views.Pages.DTOs;
using ReactiveUI;

namespace Pandora.ViewModels;

public partial class EnginePageViewModel : RoutableViewModelBase
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public override string UrlPathSegment => Routes.Engine;

	public IEngineSharedState State { get; }

	public PatchBoxViewModel PatchBoxVM { get; }
	public LogBoxViewModel LogBoxVM { get; }

	private readonly ModService _mods;

	public ObservableCollection<IEngineConfigurationViewModel> EngineConfigurationViewModels { get; } =
	[];

	public EnginePageViewModel(
		IEngineSharedState state,
		IEngineConfigurationService engineConfigService,
		PatchBoxViewModel patchBox,
		LogBoxViewModel log,
		ModService mods,
		IScreen screen
	)
		: base(screen)
	{
		State = state;

		PatchBoxVM = patchBox;
		LogBoxVM = log;
		_mods = mods;

		EngineConfigurationViewModels = ConfigurationMenuBuilder.BuildTree(
			engineConfigService.GetAvailableConfigurations(),
			engineConfigService
		);
		AddReformatCommand();
	}

	private void AddReformatCommand()
	{
		ConfigurationCategoryViewModel? category = EngineConfigurationViewModels
			.OfType<ConfigurationCategoryViewModel>()
			.FirstOrDefault(item =>
				item.Name.Equals("Skyrim64", StringComparison.OrdinalIgnoreCase)
			);
		if (category == null)
		{
			category = new ConfigurationCategoryViewModel("Skyrim64");
			EngineConfigurationViewModels.Add(category);
		}

		category.Children.Add(
			new ConfigurationActionViewModel(
				"Reformat Nemesis Animdata",
				ReactiveCommand.CreateFromTask(ReformatNemesisAnimDataAsync)
			)
		);
	}

	private async Task ReformatNemesisAnimDataAsync()
	{
		var folders = _mods
			.Source.Select(mod => mod.ModInfo)
			.Where(mod => mod.Format == IModInfo.ModFormat.Nemesis)
			.Select(mod => (mod.Name, mod.Folder.FullName))
			.ToList();
		if (folders.Count == 0)
		{
			Logger.UiWarn("No Nemesis mods are loaded.");
			return;
		}

		int files = 0;
		int mods = 0;
		await Task.Run(() =>
		{
			foreach ((string name, string folder) in folders)
			{
				int changed = NemesisSetConditionFormatter.ReformatMod(new DirectoryInfo(folder));
				if (changed == 0)
				{
					continue;
				}

				files += changed;
				mods++;
				Logger.UiInfo($"Reformatted {changed} animation set file(s) in {name}.");
			}
		});

		if (files == 0)
		{
			Logger.UiInfo("No Nemesis animation set condition edits needed reformatting.");
		}
		else
		{
			Logger.UiInfo(
				$"Reformatted {files} animation set file(s) across {mods} Nemesis mod(s)."
			);
		}
	}
}
