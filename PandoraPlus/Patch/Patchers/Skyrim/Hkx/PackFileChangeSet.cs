using NLog;
using Pandora.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
using ChangeType = IPackFileChange.ChangeType;

public class PackFileChangeSet
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private Dictionary<ChangeType, List<IPackFileChange>> changes = new Dictionary<ChangeType, List<IPackFileChange>>();

	private static readonly IOrderedEnumerable<ChangeType> orderedChangeTypes = Enum.GetValues(typeof(ChangeType)).Cast<ChangeType>().OrderBy(t => t);

	public IModInfo Origin { get; set; }

	public PackFileChangeSet(IModInfo modInfo)
	{
		foreach (ChangeType changeType in orderedChangeTypes) { changes.Add(changeType, new List<IPackFileChange>()); }
		Origin = modInfo;
	}

	public void AddChange(IPackFileChange change) => changes[change.Type].Add(change);
	public static void ApplyInOrder(PackFile packFile, List<PackFileChangeSet> changeSetList)
	{
		foreach(ChangeType changeType in orderedChangeTypes)
		{
			foreach(var changeSet in changeSetList)
			{
				changeSet.ApplyForType(packFile, changeType);
			}
		}
	}

	public void ApplyForType(PackFile packFile, ChangeType changeType)
	{
		var changeList = changes[changeType];
		foreach (var change in changeList)
		{
			if (!change.Apply(packFile)) { Logger.Warn($"Dispatcher > \"{Origin.Name}\" > {packFile.ParentProject?.Identifier}~{packFile.Name} > {change.Type} > {change.AssociatedType} > {change.Path} > FAILED"); }
		}
	}
	public void Apply(PackFile packFile)
	{
		foreach (ChangeType changeType in orderedChangeTypes)
		{
			var changeList = changes[changeType]; 
			foreach(var change in changeList) 
			{ 
				if (!change.Apply(packFile)) { Logger.Warn($"Dispatcher > \"{Origin.Name}\" > {packFile.ParentProject?.Identifier}~{packFile.Name} > {change.Type} > {change.AssociatedType} > {change.Path} > FAILED"); }
			}
		}

	}

	public void Validate(PackFile packFile, PackFileValidator validator)
	{
		foreach (ChangeType changeType in orderedChangeTypes)
		{
			validator.Validate(packFile, changes[changeType]);
		}
	}
}
