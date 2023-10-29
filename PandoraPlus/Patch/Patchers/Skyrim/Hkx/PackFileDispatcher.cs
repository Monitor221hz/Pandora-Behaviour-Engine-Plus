using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Pandora.Core.Patchers.Skyrim;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class PackFileDispatcher
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		//public ObservableCollection<IPackFileChange> ChangeHistory { get; set; } = new List<IPackFileChange>();

		private List<IPackFileChange> elementChanges {  get; set; } = new List<IPackFileChange> ();

		private List<IPackFileChange> textChanges { get; set; } = new List<IPackFileChange>();	

		private static Regex EventFormat = new Regex(@"[$]{1}eventID{1}[\[]{1}(.+)[\]]{1}[$]{1}");
		private static Regex VarFormat = new Regex(@"[$]{1}variableID{1}[\[]{1}(.+)[\]]{1}[$]{1}");

		private PackFileValidator packFileValidator { get; set; } = new PackFileValidator ();

		public void AddChange(IPackFileChange change)
		{
			if (change.AssociatedType == System.Xml.XmlNodeType.Element)
			{
				elementChanges.Add(change);
			}
			else if (change.AssociatedType == System.Xml.XmlNodeType.Text)
			{
				textChanges.Add(change);
			}
		}

		public void SortChanges()
		{
			elementChanges = elementChanges.OrderBy(x => (int)x.Type).ToList();
			textChanges = textChanges.OrderBy(x => (int) x.Type).ToList();
		}
		public void ApplyChanges(PackFile packFile)
		{
			Logger.Info($"Dispatcher > {packFile.ParentProject?.Identifier}~{packFile.Name} > APPLY CHANGES");
			SortChanges();
			foreach(IPackFileChange change in elementChanges)
			{
				if (!change.Apply(packFile))
				{
					Logger.Warn($"Mod \"{change.ModName}\" > {change.Type} > {change.AssociatedType} > {change.Path} > FAILED");
				}
			}

			foreach(IPackFileChange change in textChanges)
			{
				if (!change.Apply(packFile))
				{
					Logger.Warn($"Mod \"{change.ModName}\" > {change.Type} > {change.AssociatedType} > {change.Path} > FAILED");
				}
			}

			packFileValidator.Validate(packFile, textChanges, elementChanges);

		}
	}





}
