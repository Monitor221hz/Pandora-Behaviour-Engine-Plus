using HKX2E;
using Pandora.API.Patch.Engine.Skyrim64;
using Pandora.Core.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
public class PackFileGraph : PackFile, IPackFileGraph, IEquatable<PackFileGraph>
{

	public uint InitialEventCount { get; private set; } = 0;
	public uint InitialVariableCount {  get; private set; } = 0;

	public hkbBehaviorGraphData Data { get; private set; }
	public hkbBehaviorGraphStringData StringData { get; private set; }
	public hkbVariableValueSet VariableValueSet { get; private set; }	



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

	public int AddDefaultEvent(string name)
	{
		int index = -1; 
		lock (Data.eventInfos)
		{
			lock (StringData.eventNames)
			{
				index = StringData.eventNames.Count;

				StringData.eventNames.Add(name);
			}
			Data.eventInfos.Add(new hkbEventInfo() { flags = 0 });
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
