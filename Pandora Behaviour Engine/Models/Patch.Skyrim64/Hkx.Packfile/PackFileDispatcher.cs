// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using HKX2E;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public class PackFileDispatcher : IPackFileDispatcher
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private List<IPackFileChange> elementChanges { get; set; } = [];

	private List<IPackFileChange> textChanges { get; set; } = [];

	private List<IPackFileChangeOwner> changeOwners { get; set; } = [];

	private IPackFileValidator packFileValidator { get; set; } = new PackFileValidator();

	public void TrackPotentialNode(IPackFile packFile, string nodeName, XElement element)
	{
		packFile.XmlDeserializer.Collect(nodeName, element);
		packFileValidator.TrackElement(element);
	}

	public void AddChangeSet(IPackFileChangeOwner changeOwner)
	{
		lock (changeOwners)
		{
			changeOwners.Add(changeOwner);
		}
	}

	public void SortChangeSets()
	{
		changeOwners = changeOwners.OrderBy(s => s.Origin.Priority).ToList();
	}

	public void ApplyChangesForNode(IHavokObject obj, IPackFile packFile)
	{
		if (!packFile.Serializer.TryGetName(obj, out var nodeName))
		{
			return;
		}
		PackFileChangeSet.ApplyForNode(nodeName, packFile, changeOwners);
	}

	public void ApplyChanges(IPackFile packFile)
	{
		SortChangeSets();
		packFile.ApplyPriorityChanges(this);
		packFile.PushPriorityObjects();
		packFile.Load();
		bool isGraph = false;
		if (packFile is IPackFileGraph graph)
		{
			isGraph = true;
			packFileValidator.ValidateEventsAndVariables(graph);
		}
		PackFileChangeSet.ApplyInOrder(packFile, changeOwners);
		packFileValidator.ValidateTrackedElements();
		if (isGraph)
		{
			foreach (var changeSet in changeOwners)
			{
				changeSet.Validate(packFile, packFileValidator);
			}
		}
		//packFile.PushAllNodes();
	}
}
