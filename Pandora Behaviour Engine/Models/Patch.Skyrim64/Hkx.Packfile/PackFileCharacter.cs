// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using HKX2E;
using Pandora.API.Patch.Skyrim64;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public class PackFileCharacter : PackFile, IEquatable<PackFileCharacter>, IPackFileCharacter
{
	public PackFileCharacter(FileInfo file, IProject? project)
		: base(file, project)
	{
		Load();
		InitialAnimationCount = (uint)StringData.animationNames.Count;
	}

	public PackFileCharacter(FileInfo file)
		: this(file, null) { }

	public hkbCharacterData Data { get; set; }
	public hkbCharacterStringData StringData { get; set; }
	public uint InitialAnimationCount { get; private set; } = 0;
	public uint NewAnimationCount => (uint)StringData.animationNames.Count - InitialAnimationCount;
	public IList<string> AnimationNames => StringData.animationNames;
	public string BehaviorFileName => StringData.behaviorFilename;
	public string SkeletonFileName => StringData.rigName;

	private HashSet<string> uniqueBaseAnimations = new(StringComparer.OrdinalIgnoreCase);
	private HashSet<string> uniqueAnimations = new(StringComparer.OrdinalIgnoreCase);

	private object _uniqueAnimationLock = new();

	public object GetUniqueAnimationLock()
	{
		return _uniqueAnimationLock;
	}

	public object AttachUniqueAnimationLock(IPackFileCharacter character)
	{
		_uniqueAnimationLock = character.GetUniqueAnimationLock();
		return _uniqueAnimationLock;
	}

	private void CacheUniqueBaseAnimations()
	{
		uniqueBaseAnimations = StringData.animationNames.ToHashSet();
	}

	[MemberNotNull(nameof(StringData), nameof(Data))]
	public override void Load()
	{
		Data = (hkbCharacterData)Container.namedVariants.First()!.variant!;
		StringData = Data.stringData!;
	}

	public override void PushXmlAsObjects()
	{
		base.PushXmlAsObjects();
	}

	public override void PopPriorityXmlAsObjects()
	{
		PopObjectAsXml(Data);
		PopObjectAsXml(StringData);
	}

	public override void ApplyPriorityChanges(IPackFileDispatcher dispatcher)
	{
		base.ApplyPriorityChanges(dispatcher);
		dispatcher.ApplyChangesForNode(Data, this);
		dispatcher.ApplyChangesForNode(StringData, this);
	}

	public override void PushPriorityObjects()
	{
		base.PushPriorityObjects();
		PushXmlAsObject(Data);
		PushXmlAsObject(StringData);
	}

	public bool Equals(PackFileCharacter? other)
	{
		return base.Equals(other);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public bool AddUniqueAnimation(string name)
	{
		lock (uniqueBaseAnimations)
		{
			if (uniqueBaseAnimations.Count == 0)
			{
				CacheUniqueBaseAnimations();
			}
			if (uniqueBaseAnimations.Contains(name))
			{
				return false;
			}
		}
		if (uniqueAnimations.Add(name))
		{
			StringData.animationNames.Add(name);
			return true;
		}
		return false;
	}

	public bool Equals(IPackFileCharacter? other)
	{
		return base.Equals(other);
	}
}
