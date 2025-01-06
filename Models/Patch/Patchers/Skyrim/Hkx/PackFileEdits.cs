using Pandora.Core.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using XmlCake.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public partial class PackFileEditor
	{
		private static readonly char[] trimChars = new char[] {'\t', '\r', '\n', ')', '(' };
		private static readonly Regex whiteSpaceRegex = new Regex(@"(?:\s|\(|\))+", RegexOptions.Compiled);
		private static readonly Regex escapeRegex = new Regex(@"(?:\*|\+|\?|\||\^|\.|\#)", RegexOptions.Compiled); 
		private static string NormalizeElementValue(XElement element)
		{
			var value = whiteSpaceRegex.Replace(element.Value.Trim(trimChars), " ");
			return value;
		}

		private static string NormalizeStringValue(string value)
		{
			return whiteSpaceRegex.Replace(value.Trim(trimChars), " ");
		}
		public static XElement ReplaceElement(IXMap xmap, string path, XElement element) => xmap.ReplaceElement(path, element);


		public static string InsertElement(IXMap xmap, string path, XElement element)
		{
			return xmap.InsertElement(path, element, true);
		}

		public static string AppendElement(IXMap xmap, string path, XElement element)
		{
			return xmap.AppendElement(path, element);
		}

		public static string PushElement(IXMap xmap, string path, XElement element)
		{
			return xmap.PushElement(path, element);
		}

		public static XElement RemoveElement(IXMap xmap, string path) => xmap.RemoveElement(path); 

		public static bool ReplaceText(IXMap xmap, string path, string preValue, string oldValue, string newValue)
		{
			
			XElement element = xmap.NavigateTo(path);
			if (String.IsNullOrWhiteSpace(oldValue)) return false;

			string source = NormalizeElementValue(element);
			//if (String.IsNullOrWhiteSpace(newValue))
			//{

			//	int index = source.IndexOf(oldValue, StringComparison.Ordinal);
			//	newValue = (index < 0) ? source : source.Remove(index, oldValue.Length);
			//	element.SetValue(newValue);
			//	return true;
			//}
			//preValue = NormalizeStringValue(preValue);
			//oldValue = NormalizeStringValue(oldValue);
			oldValue = whiteSpaceRegex.Replace(escapeRegex.Replace(oldValue, "\\$&"), "\\s*");

			//ReadOnlySpan<char> headSpan = source.AsSpan(0, preValue.Length);
			//ReadOnlySpan<char> tailSpan = source.AsSpan(preValue.Length+oldValue.Length+1);

			Regex targetRegex = new Regex(oldValue);
			int targetMatchIndex = targetRegex.Count(preValue);
			int matchIndex = -1; 
			for(var match = targetRegex.Match(source); match.Success; match = match.NextMatch())
			{
				matchIndex++;
				if (matchIndex == targetMatchIndex)
				{
					source = String.Concat(source.AsSpan(0, match.Index), newValue, source.AsSpan(match.Index + match.Length));
					break; 
				}
				
			}
			if (matchIndex == -1) { return false; }
			element.SetValue(source);

			return true; 

		}
		public static bool SetText(IXMap xmap, string path, string newValue)
		{
			XElement element = xmap.NavigateTo(path);
			element.SetValue(newValue);
			return true;
		}
		public static bool InsertText(IXMap xmap, string path, string markerValue, string newValue)
		{
			XElement element = xmap.NavigateTo(path);
			string source = NormalizeElementValue(element);

			markerValue = NormalizeStringValue(markerValue);
			var match = Regex.Match(source, markerValue);
			if (!match.Success) {  return false; }
			source = string.Concat(source.AsSpan(0, match.Index), newValue, source.AsSpan(match.Index));

			//var headSpan = source.AsSpan(0,markerValue.Length);
			//var tailSpan = source.AsSpan(markerValue.Length + 1);
			element.SetValue(source);

			return true; 
		}

		public static void AppendText(IXMap xmap, string path, string newValue)
		{
			XElement element = xmap.NavigateTo(path);
			string source = NormalizeElementValue(element);

			newValue = NormalizeStringValue(newValue);
			source = NormalizeStringValue(String.Concat(source, " ", newValue, " "));
			element.SetValue(source);
		}
		public static void RemoveText(IXMap xmap, string path, string value)
		{
			XElement element = xmap.NavigateTo(path);
			if (String.IsNullOrWhiteSpace(value)) return;
			string source = element.Value;
			source = NormalizeStringValue(source.Replace(value, string.Empty, true));
			element.SetValue(source);
		}
	}
}
