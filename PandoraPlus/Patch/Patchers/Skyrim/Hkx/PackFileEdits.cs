using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using XmlCake.String;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public partial class PackFileEditor
	{
		public static void ReplaceElement(PackFile packFile, string path, XElement element) => packFile.Map.ReplaceElement(path, element);

		public static void InsertElement(PackFile packFile, string path, XElement element)
		{
			packFile.Map.AppendElement(path.Substring(0, path.LastIndexOf('/')), element); 
		}


		public static void ReplaceText(PackFile packFile, string path, string oldValue, string newValue)
		{
			char[] trimChars = new char[3] { '\n', '\r', '\t' };
			XElement element = packFile.Map.NavigateTo(path);
			if (String.IsNullOrWhiteSpace(oldValue)) return;
			string source = element.Value;
			if (String.IsNullOrWhiteSpace(newValue))
			{

				int index = source.IndexOf(oldValue, StringComparison.Ordinal);
				newValue = (index < 0) ? source : source.Remove(index, oldValue.Length);
				element.SetValue(newValue);
				return;
			}
			oldValue = oldValue.Trim().Replace("\t", "").Replace('\n', ' ');
			newValue = newValue.Trim().Replace("\t", "").Replace('\n', ' ');

			element.SetValue(source.Replace(oldValue, newValue, true));

		}
	}
}
