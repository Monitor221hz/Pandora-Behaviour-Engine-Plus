// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Xml.Linq;
using HKX2E;

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFileDispatcher
{
	bool MergeAddedEventPropertyArrays { get; }
	void AddChangeSet(IPackFileChangeOwner changeOwner);
	void ApplyChanges(IPackFile packFile);
	void ApplyChangesForNode(IHavokObject obj, IPackFile packFile);
	void NoteNotifyEventsAddedFromNull(string nodeName, string parameterName);
	void SortChangeSets();
	void TrackPotentialNode(IPackFile packFile, string nodeName, XElement element);
	bool WereNotifyEventsAddedFromNull(string nodeName, string parameterName);
}
