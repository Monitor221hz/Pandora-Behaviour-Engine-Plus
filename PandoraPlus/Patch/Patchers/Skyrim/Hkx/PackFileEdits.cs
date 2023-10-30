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
		public static XElement ReplaceElement(PackFile packFile, string path, XElement element) => packFile.Map.ReplaceElement(path, element);


		public static string InsertElement(PackFile packFile, string path, XElement element)
		{
			return packFile.Map.InsertElement(path, element, true);
		}

		public static string AppendElement(PackFile packFile, string path, XElement element)
		{
			return packFile.Map.AppendElement(path, element);
		}

		public static XElement RemoveElement(PackFile packFile, string path) => packFile.Map.RemoveElement(path); 

		public static void ReplaceText(PackFile packFile, string path, string oldValue, string newValue)
		{
			
			XElement element = packFile.SafeNavigateTo(path);
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
		public static void InsertText(PackFile packFile, string path, string newValue)
		{
			char[] trimChars = new char[3] { '\n', '\r', '\t' };
			XElement element = packFile.SafeNavigateTo(path);
			string source = element.Value.TrimEnd(trimChars);

			newValue = newValue.Trim().Replace("\t", "").Replace('\n', ' ');
			element.SetValue(source.Append(' ', newValue+Environment.NewLine));
		}

		public static void RemoveText(PackFile packFile, string path, string value)
		{
			XElement element = packFile.SafeNavigateTo(path);
			if (String.IsNullOrWhiteSpace(value)) return;
			string source = element.Value;
			value = value.Trim().Replace("\t", "").Replace('\n', ' '); 

			element.SetValue(source.Replace(value, string.Empty, true));
		}
	}
}
