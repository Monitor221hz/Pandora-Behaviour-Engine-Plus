using HKX2E;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public class PackFileDispatcher
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private List<IPackFileChange> elementChanges { get; set; } = [];

	private List<IPackFileChange> textChanges { get; set; } = [];

	private List<PackFileChangeSet> changeSets { get; set; } = [];

	private PackFileValidator packFileValidator { get; set; } = new PackFileValidator();

	public void TrackPotentialNode(PackFile packFile, string nodeName, XElement element)
	{
		packFile.PartialDeserializer.Collect(nodeName, element);
		packFileValidator.TrackElement(element);
	}
	public void AddChangeSet(PackFileChangeSet changeSet)
	{
		lock (changeSets)
		{
			changeSets.Add(changeSet);
		}
	}

	public void SortChangeSets()
	{
		changeSets = changeSets.OrderBy(s => s.Origin.Priority).ToList();
	}
	public void ApplyChangesForNode(IHavokObject obj, PackFile packFile)
	{
		if (!packFile.Serializer.TryGetName(obj, out var nodeName))
		{
			return;
		}
		PackFileChangeSet.ApplyForNode(nodeName, packFile, changeSets);
	}
	public void ApplyChanges(PackFile packFile)
	{
		SortChangeSets();
		packFile.ApplyPriorityChanges(this);
		packFile.PushPriorityObjects();
		packFile.Load();
		bool isGraph = false;
		if (packFile is PackFileGraph graph)
		{
			isGraph = true;
			packFileValidator.ValidateEventsAndVariables(graph);
		}
		PackFileChangeSet.ApplyInOrder(packFile, changeSets);
		packFileValidator.ValidateTrackedElements();
		if (isGraph)
		{
			foreach (var changeSet in changeSets)
			{
				changeSet.Validate(packFile, packFileValidator);
			}
		}
		//packFile.PushAllNodes();
	}
}