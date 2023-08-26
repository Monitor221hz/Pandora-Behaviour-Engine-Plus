using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlCake.Linq;

namespace Pandora.Patch.Patchers.Skyrim
{
	public class HkxDispatcher : IDispatcher<XMap, XPathLookup, List<XNode>>
	{

		private bool DeleteElements(XMap target, XPathLookup pathLookup, List<XNode> nodes)
		{
			List<XElement> elements = nodes.Cast<XElement>().ToList();
			target.MapLayer("__data__"); 
			foreach (XElement element in elements)
			{
				string key = target.GetPath(element); 

				target.MapSlice(key);
			}
			return true; 
		}
		public bool Delete(XMap target, XPathLookup pathLookup, List<XNode> nodes)
		{

			target.MapLayer("__data__"); 

			foreach(XNode node in nodes)
			{
				target.GetPath(node); 
			}
			return true; 
		}

		public bool Insert(XMap target, XPathLookup pathLookup, List<XNode> nodes)
		{
			throw new NotImplementedException();
		}
	}
}
