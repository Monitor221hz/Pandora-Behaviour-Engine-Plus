// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.DTOs;
using Pandora.Services.Interfaces;
using Pandora.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pandora.Utils;

public static class ConfigurationMenuBuilder
{
	private static readonly char[] PathSeparators = ['/', '\\'];

	public static ObservableCollection<IEngineConfigurationViewModel> BuildTree(
		IEnumerable<EngineConfigDescriptor> descriptors,
		IEngineConfigurationService configService)
	{
		var rootItems = new ObservableCollection<IEngineConfigurationViewModel>();

		foreach (var desc in descriptors)
		{
			InsertIntoTree(rootItems, desc, configService);
		}

		return rootItems;
	}

	private static void InsertIntoTree(
		ObservableCollection<IEngineConfigurationViewModel> collection,
		EngineConfigDescriptor desc,
		IEngineConfigurationService service)
	{
		if (string.IsNullOrWhiteSpace(desc.MenuPath))
		{
			collection.Add(new ConfigurationOptionViewModel(desc.Name, desc.Factory, service));
			return;
		}

		var segments = desc.MenuPath.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
		ObservableCollection<IEngineConfigurationViewModel> currentLevel = collection;

		foreach (var segment in segments)
		{
			var existingContainer = currentLevel
				.OfType<ConfigurationCategoryViewModel>()
				.FirstOrDefault(c => c.Name.Equals(segment, StringComparison.OrdinalIgnoreCase));

			if (existingContainer == null)
			{
				existingContainer = new ConfigurationCategoryViewModel(segment);
				currentLevel.Add(existingContainer);
			}

			currentLevel = existingContainer.Children;
		}

		currentLevel.Add(new ConfigurationOptionViewModel(desc.Name, desc.Factory, service));
	}
}