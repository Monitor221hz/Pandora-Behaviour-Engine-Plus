// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using HKX2E;
using Pandora.API.Patch;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;

public class FNISAnimationListBuildContext
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private Dictionary<string, hkbStringEventPayload> stringEventPayloadNameMap = [];

	public FNISAnimationListBuildContext(
		Project targetProject,
		ProjectManager projectManager,
		IModInfo modInfo
	)
	{
		TargetProject = targetProject;
		ProjectManager = projectManager;
		ModInfo = modInfo;
	}

	public Project TargetProject { get; private set; }
	public ProjectManager ProjectManager { get; private set; }

	public IModInfo ModInfo { get; private set; }

	public hkbStringEventPayload BuildCommonStringEventPayload(string name)
	{
		hkbStringEventPayload? payload;
		lock (stringEventPayloadNameMap)
		{
			if (stringEventPayloadNameMap.TryGetValue(name, out payload))
			{
				return payload;
			}
		}
		payload = new hkbStringEventPayload() { data = name };
		lock (stringEventPayloadNameMap)
		{
			stringEventPayloadNameMap.Add(name, payload);
		}
		return payload;
	}
}
