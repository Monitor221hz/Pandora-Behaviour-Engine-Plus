using NLog;
using Pandora.API.Patch;
using Pandora.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
using ChangeType = IPackFileChange.ChangeType;

public class PackFileChangeSet : IPackFileChangeOwner
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private Dictionary<string,Dictionary<ChangeType, List<IPackFileChange>>> nodeScopedChangeMap = new(StringComparer.OrdinalIgnoreCase);

	private static readonly IOrderedEnumerable<ChangeType> orderedChangeTypes = Enum.GetValues(typeof(ChangeType)).Cast<ChangeType>().OrderBy(t => t);

	public IModInfo Origin { get; set; }

	public PackFileChangeSet(IModInfo modInfo)
	{
		//foreach (ChangeType changeType in orderedChangeTypes) { changes.Add(changeType, new List<IPackFileChange>()); }
		Origin = modInfo;
	}

	public PackFileChangeSet(PackFileChangeSet packFileChangeSet)
	{
		//foreach (ChangeType changeType in orderedChangeTypes) { changes.Add(changeType, new List<IPackFileChange>()); }
		Origin = packFileChangeSet.Origin;
	}
	//public void AddElementAsChange(XElement element) => AddChange(new PushElementChange(PackFile.ROOT_CONTAINER_NAME, element));

	public void AddElementAsChange(XElement element)
	{
		return;
	}
	//public void AddChange(IPackFileChange change) => changes[change.Type].Add(change);
	public void AddChange(IPackFileChange change)
	{
		if (nodeScopedChangeMap.TryGetValue(change.Target, out var changeTypedMap))
		{
			changeTypedMap[change.Type].Add(change);
			return;
		}
		changeTypedMap = new();
		foreach (ChangeType changeType in orderedChangeTypes) { changeTypedMap.Add(changeType, new List<IPackFileChange>()); }
		changeTypedMap[change.Type].Add(change);
		nodeScopedChangeMap.Add(change.Target,changeTypedMap);
	}
	public static void ApplyInOrder(PackFile packFile, List<PackFileChangeSet> changeSetList)
	{
		foreach (ChangeType changeType in orderedChangeTypes)
		{
			foreach (var changeSet in changeSetList)
			{
				changeSet.ApplyForType(packFile, changeType);
			}
		}
	}
	public static void ApplyForNode(string nodeName, PackFile packFile, List<PackFileChangeSet> changeSetList)
	{
		foreach (var changeSet in changeSetList)
		{
			changeSet.ApplyForNode(nodeName, packFile);
		}
	}
	public void ApplyInOrder(PackFile packFile)
	{
		foreach (var changeType in orderedChangeTypes)
		{
			ApplyForType(packFile, changeType);
		}
	}
	public void ApplyForNode(string nodeName, PackFile packFile)
	{
		if (!nodeScopedChangeMap.TryGetValue(nodeName, out var changeTypedMap))
		{
			return;
		}
		foreach (var changeType in orderedChangeTypes)
		{
			var changeList = changeTypedMap[changeType];
			for (int i = changeList.Count - 1; i >= 0; i--)
			{
				IPackFileChange? change = changeList[i];
				if (!change.Apply(packFile)) { Logger.Warn($"Dispatcher > \"{Origin.Name}\" > {packFile.ParentProject?.Identifier}~{packFile.Name} > {change.Type} > {change.AssociatedType} > {change.Path} > FAILED"); }
				changeList.RemoveAt(i);
			}
		}
	}
	public void ApplyForType(PackFile packFile, ChangeType changeType)
	{
		foreach(var changeTypedMap in nodeScopedChangeMap.Values)
		{
			var changeList = changeTypedMap[changeType];
			foreach (var change in changeList)
			{
				if (!change.Apply(packFile)) { Logger.Warn($"Dispatcher > \"{Origin.Name}\" > {packFile.ParentProject?.Identifier}~{packFile.Name} > {change.Type} > {change.AssociatedType} > {change.Path} > FAILED"); }
			}
		}

	}
	public void Apply(PackFile packFile)
	{
		foreach (ChangeType changeType in orderedChangeTypes)
		{
			foreach (var changeTypedMap in nodeScopedChangeMap.Values)
			{
				var changeList = changeTypedMap[changeType];
				foreach (var change in changeList)
				{
					if (!change.Apply(packFile)) { Logger.Warn($"Dispatcher > \"{Origin.Name}\" > {packFile.ParentProject?.Identifier}~{packFile.Name} > {change.Type} > {change.AssociatedType} > {change.Path} > FAILED"); }
				}
			}

		}

	}

	public void Validate(PackFile packFile, PackFileValidator validator)
	{
		foreach (ChangeType changeType in orderedChangeTypes)
		{
			foreach (var changeTypedMap in nodeScopedChangeMap.Values)
			{
				validator.Validate(packFile, changeTypedMap[changeType]);
			}
				
		}
	}
}
