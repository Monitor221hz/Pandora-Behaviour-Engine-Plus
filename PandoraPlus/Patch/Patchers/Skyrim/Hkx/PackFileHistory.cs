using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class PackFileHistory
	{
		//public ObservableCollection<IPackFileChange> ChangeHistory { get; set; } = new List<IPackFileChange>();

		private List<IPackFileChange> elementChanges {  get; set; } = new List<IPackFileChange> ();

		private List<IPackFileChange> textChanges { get; set; } = new List<IPackFileChange>();	


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


		public void ApplyChanges(PackFile packFile)
		{
			foreach(IPackFileChange change in elementChanges)
			{
				if (!change.Apply(packFile))
				{
					Debug.WriteLine($"{change.Type} change for {change.AssociatedType} at {change.Path} failed.");
				}
			}

		}
	}





}
