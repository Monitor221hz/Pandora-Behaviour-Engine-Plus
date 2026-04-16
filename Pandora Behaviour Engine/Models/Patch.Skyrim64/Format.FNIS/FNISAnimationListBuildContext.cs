// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using HKX2E;
using Pandora.API.Patch;
using Pandora.API.Patch.Skyrim64;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;

public class FNISAnimationListBuildContext : IFNISAnimationListBuildContext
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly Dictionary<string, hkbStringEventPayload> _stringEventPayloadNameMap = [];

	public FNISAnimationListBuildContext(
		IProject targetProject,
		IProjectManager projectManager,
		IModInfo modInfo
	)
	{
		TargetProject = targetProject;
		ProjectManager = projectManager;
		ModInfo = modInfo;
	}

	public IProject TargetProject { get; private set; }
	public IProjectManager ProjectManager { get; private set; }

	public IModInfo ModInfo { get; private set; }

	public hkbStringEventPayload BuildCommonStringEventPayload(string name)
	{
		hkbStringEventPayload? payload;
		lock (_stringEventPayloadNameMap)
		{
			if (_stringEventPayloadNameMap.TryGetValue(name, out payload))
			{
				return payload;
			}
		}
		payload = new hkbStringEventPayload() { data = name };
		lock (_stringEventPayloadNameMap)
		{
			_stringEventPayloadNameMap.Add(name, payload);
		}
		return payload;
	}
}
