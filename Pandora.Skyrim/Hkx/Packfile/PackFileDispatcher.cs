// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using HKX2E;
using Pandora.API.Patch.Skyrim64;
using Pandora.Core.Settings;
using Pandora.Skyrim.Hkx.Changes;

namespace Pandora.Skyrim.Hkx.Packfile;

public class PackFileDispatcher : IPackFileDispatcher
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private List<IPackFileChange> _elementChanges = [];

	private List<IPackFileChange> _textChanges = [];

	private List<IPackFileChangeOwner> _changeOwners = [];

	private readonly PackFileValidator _packFileValidator = new();

	private readonly HashSet<(string Node, string Parameter)> _notifyEventsAddedFromNull = [];

	public bool MergeAddedEventPropertyArrays { get; set; }

	public void NoteNotifyEventsAddedFromNull(string nodeName, string parameterName)
	{
		_notifyEventsAddedFromNull.Add((nodeName, parameterName));
	}

	public bool WereNotifyEventsAddedFromNull(string nodeName, string parameterName)
	{
		return _notifyEventsAddedFromNull.Contains((nodeName, parameterName));
	}

	public void TrackPotentialNode(IPackFile packFile, string nodeName, XElement element)
	{
		packFile.XmlDeserializer.Collect(nodeName, element);
		_packFileValidator.TrackElement(element);
	}

	public void AddChangeSet(IPackFileChangeOwner changeOwner)
	{
		lock (_changeOwners)
		{
			_changeOwners.Add(changeOwner);
		}
	}

	public void SortChangeSets()
	{
		_changeOwners = _changeOwners.OrderBy(s => s.Origin.Priority).ToList();
	}

	public void ApplyChangesForNode(IHavokObject obj, IPackFile packFile)
	{
		if (!packFile.Serializer.TryGetName(obj, out var nodeName))
		{
			return;
		}

		PackFileChangeSet.ApplyForNode(nodeName, packFile, _changeOwners);
	}

	public void ApplyChanges(IPackFile packFile)
	{
		MergeAddedEventPropertyArrays = PatchPreferences.MergeAddedEventPropertyArrays;
		SortChangeSets();
		packFile.ApplyPriorityChanges(this);
		packFile.PushPriorityObjects();
		packFile.Load();
		bool isGraph = false;
		if (packFile is IPackFileGraph graph)
		{
			isGraph = true;
			_packFileValidator.ValidateEventsAndVariables(graph);
		}

		PackFileChangeSet.ApplyInOrder(packFile, _changeOwners);
		_packFileValidator.ValidateTrackedElements();
		if (isGraph)
		{
			foreach (var changeSet in _changeOwners)
			{
				changeSet.Validate(packFile, _packFileValidator);
			}
		}
		//packFile.PushAllNodes();
	}
}
