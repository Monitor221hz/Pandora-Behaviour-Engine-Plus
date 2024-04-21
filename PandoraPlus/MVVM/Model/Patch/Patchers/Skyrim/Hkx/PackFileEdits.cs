using Pandora.Core.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public partial class PackFileEditor
	{
		private static readonly char[] trimChars = new char[] {'\t', '\r', '\n', ')', '(' };
		private static readonly Regex whiteSpaceRegex = new Regex(@"(?:\s|\(|\)){2,}", RegexOptions.Compiled);
		private static string NormalizeElementValue(XElement element)
		{
			var value = whiteSpaceRegex.Replace(element.Value.Trim(trimChars), " ");
			return value;
		}

		private static string NormalizeStringValue(string value)
		{
			return whiteSpaceRegex.Replace(value.Trim(trimChars), " ");
		}
		public static XElement ReplaceElement(PackFile packFile, string path, XElement element) => packFile.Map.ReplaceElement(path, element);


		public static string InsertElement(PackFile packFile, string path, XElement element)
		{
			return packFile.Map.InsertElement(path, element, true);
		}

		public static string AppendElement(PackFile packFile, string path, XElement element)
		{
			return packFile.Map.AppendElement(path, element);
		}

		public static string PushElement(PackFile packFile, string path, XElement element)
		{
			return packFile.Map.PushElement(path, element);
		}

		public static XElement RemoveElement(PackFile packFile, string path) => packFile.Map.RemoveElement(path); 

		public static bool ReplaceText(PackFile packFile, string path, string preValue, string oldValue, string newValue)
		{
			
			XElement element = packFile.SafeNavigateTo(path);
			if (String.IsNullOrWhiteSpace(oldValue)) return false;

			string source = NormalizeElementValue(element);
			if (String.IsNullOrWhiteSpace(newValue))
			{

				int index = source.IndexOf(oldValue, StringComparison.Ordinal);
				newValue = (index < 0) ? source : source.Remove(index, oldValue.Length);
				element.SetValue(newValue);
				return true;
			}
			preValue = NormalizeStringValue(preValue);
			oldValue = NormalizeStringValue(oldValue);

			ReadOnlySpan<char> headSpan = source.AsSpan(0, preValue.Length);
			ReadOnlySpan<char> tailSpan = source.AsSpan(preValue.Length+oldValue.Length+1);
			
			int sourceLength = source.Length;

			//normalize string to prevent range errors on later operations
			source = NormalizeStringValue(String.Concat(headSpan, " " + newValue + " ", tailSpan)); //pad spaces as bare minimum; don't want array values to be twinned
			element.SetValue(source);

			return true; 

		}
		public static bool SetText(PackFile packFile, string path, string newValue)
		{
			XElement element = packFile.SafeNavigateTo(path);
			element.SetValue(newValue);
			return true;
		}
		public static void InsertText(PackFile packFile, string path, string insertAfterValue, string newValue)
		{
			
			XElement element = packFile.SafeNavigateTo(path);
			string source = NormalizeElementValue(element);

			insertAfterValue = NormalizeStringValue(insertAfterValue);
			var headSpan = source.AsSpan(0,insertAfterValue.Length);
			var tailSpan = source.AsSpan(insertAfterValue.Length + 1);

			source = NormalizeStringValue(String.Concat(headSpan, newValue, tailSpan));
			element.SetValue(source);
		}

		public static void AppendText(PackFile packFile, string path, string newValue)
		{
			XElement element = packFile.SafeNavigateTo(path);
			string source = NormalizeElementValue(element);

			newValue = NormalizeStringValue(newValue);
			source = NormalizeStringValue(String.Concat(source, " ", newValue, " "));
			element.SetValue(source);
		}
		public static void RemoveText(PackFile packFile, string path, string value)
		{
			XElement element = packFile.SafeNavigateTo(path);
			if (String.IsNullOrWhiteSpace(value)) return;
			string source = element.Value;
			source = NormalizeStringValue(source.Replace(value, string.Empty, true));
			element.SetValue(source);
		}
	}
}
