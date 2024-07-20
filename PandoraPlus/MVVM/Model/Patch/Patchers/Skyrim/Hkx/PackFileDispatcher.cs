using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Pandora.Core.Patchers.Skyrim;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;

public class PackFileDispatcher
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	//public ObservableCollection<IPackFileChange> ChangeHistory { get; set; } = new List<IPackFileChange>();

	private List<IPackFileChange> elementChanges { get; set; } = new List<IPackFileChange>();

	private List<IPackFileChange> textChanges { get; set; } = new List<IPackFileChange>();

	private List<PackFileChangeSet> changeSets { get; set; } = new List<PackFileChangeSet>();

	private static Regex EventFormat = new Regex(@"[$]{1}eventID{1}[\[]{1}(.+)[\]]{1}[$]{1}");
	private static Regex VarFormat = new Regex(@"[$]{1}variableID{1}[\[]{1}(.+)[\]]{1}[$]{1}");

	private PackFileValidator packFileValidator { get; set; } = new PackFileValidator();


	public void AddChangeSet(PackFileChangeSet changeSet)
	{
		lock(changeSets)
		{
			changeSets.Add(changeSet);
		}
	}

	public void SortChangeSets()
	{
		changeSets = changeSets.OrderBy(s => s.Origin.Priority).ToList();
	}
	public void ApplyChanges(PackFile packFile)
	{
		SortChangeSets();
		
		PackFileChangeSet.ApplyInOrder(packFile, changeSets);

		if (packFile is not PackFileGraph) { return; }

		packFileValidator.ValidateEventsAndVariables((PackFileGraph)packFile);

		foreach (var changeSet in changeSets) { changeSet.Validate(packFile, packFileValidator); }
	}
}
