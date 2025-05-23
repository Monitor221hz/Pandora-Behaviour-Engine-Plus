﻿using HKX2E;
using Pandora.API.Patch.Engine.Skyrim64;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public class PackFileGraph : PackFile, IPackFileGraph, IEquatable<PackFileGraph>
{

	public uint InitialEventCount { get; private set; } = 0;
	public uint InitialVariableCount { get; private set; } = 0;

	public hkbBehaviorGraphData Data { get; private set; }
	public hkbBehaviorGraphStringData StringData { get; private set; }
	public hkbVariableValueSet VariableValueSet { get; private set; }

	private HashSet<string> customEventBuffer = [];

	public bool AddEventBuffer(string name)
	{
		lock (customEventBuffer)
		{
			return customEventBuffer.Add(name);
		}
	}

	private readonly Dictionary<string, int> customEventIndices = new(StringComparer.OrdinalIgnoreCase);



	public PackFileGraph(FileInfo file, Project? project) : base(file, project)
	{
		Load();
		InitialVariableCount = (uint)Data.variableInfos.Count;
		InitialEventCount = (uint)Data.eventInfos.Count;
	}

	public PackFileGraph(FileInfo file) : this(file, null)
	{
	}

	[MemberNotNull(nameof(Data), nameof(StringData), nameof(VariableValueSet))]
	public override void Load()
	{
		base.Load();
		hkbBehaviorGraph graph = (hkbBehaviorGraph)Container.namedVariants.First()!.variant!;
		Data = graph.data!;
		StringData = Data.stringData!;
		VariableValueSet = Data.variableInitialValues!;
	}

	public override void ApplyPriorityChanges(PackFileDispatcher dispatcher)
	{
		dispatcher.ApplyChangesForNode(Data, this);
		dispatcher.ApplyChangesForNode(StringData, this);
		dispatcher.ApplyChangesForNode(VariableValueSet, this);
	}
	public override void PushPriorityObjects()
	{
		base.PushPriorityObjects();
		PushXmlAsObject(Data);
		PushXmlAsObject(StringData);
		PushXmlAsObject(VariableValueSet);
	}
	public override void PushXmlAsObjects()
	{
		base.PushXmlAsObjects();
	}
	public override void PopPriorityXmlAsObjects()
	{
		PopObjectAsXml(Data);
		PopObjectAsXml(StringData);
		PopObjectAsXml(VariableValueSet);
	}

	public void FlushEventBuffer(string name)
	{
		foreach (var eventName in customEventBuffer)
		{
			AddDefaultEvent(eventName);
		}
	}

	public int AddDefaultEvent(string name)
	{
		int index = -1;
		lock (customEventIndices)
		{
			if (customEventIndices.TryGetValue(name, out index))
			{
				return index;
			}
			lock (Data.eventInfos)
			{
				lock (StringData.eventNames)
				{
					index = StringData.eventNames.Count;
					Data.eventInfos.Add(new hkbEventInfo() { flags = 0 });
					StringData.eventNames.Add(name);
					customEventIndices.Add(name, index);
				}

			}
		}
		return index;
	}

	public int FindEvent(string name)
	{
		int index;
		lock (customEventIndices)
		{
			if (customEventIndices.TryGetValue(name, out index))
			{
				return index;
			}
			index = -1;
			lock (Data.eventInfos)
			{
				lock (StringData.eventNames)
				{
					for (int i = 0; i < StringData.eventNames.Count; i++)
					{
						if (StringData.eventNames[i].Equals(name, StringComparison.OrdinalIgnoreCase))
						{
							index = i;
							customEventIndices.Add(name, index);
							return index;
						}
					}
				}

			}
		}
		return index;
	}
	public bool Equals(PackFileGraph? other)
	{
		return base.Equals(other);
	}
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
