using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using Pandora.Core.Extensions;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public partial class PackFileEditor
	{
		static private readonly char[] trimChars = new char[3] { '\n', '\r', '\t' };
		private static readonly Regex whiteSpaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
		public static string GetTrimElementValue(XElement element)
		{
			return whiteSpaceRegex.Replace(element.Value.Trim(), " ");
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

			string source = whiteSpaceRegex.Replace(element.Value.Trim(), " ");
			if (String.IsNullOrWhiteSpace(newValue))
			{

				int index = source.IndexOf(oldValue, StringComparison.Ordinal);
				newValue = (index < 0) ? source : source.Remove(index, oldValue.Length);
				element.SetValue(newValue);
				return true;
			}
			preValue = whiteSpaceRegex.Replace(preValue.Trim(), " ");
			oldValue = whiteSpaceRegex.Replace(oldValue.Trim(), " ");
			newValue = ' ' + whiteSpaceRegex.Replace(newValue.Trim(), " ");

			ReadOnlySpan<char> headSpan = source.AsSpan(0, preValue.Length);
			ReadOnlySpan<char> tailSpan = source.AsSpan(preValue.Length+oldValue.Length+1);
			

			int sourceLength = source.Length;

			element.SetValue(String.Concat(headSpan, newValue, tailSpan));
			if ((sourceLength - oldValue.Length + newValue.Length) != element.Value.Length) 
			{ 
				return false; 
			}
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
			string source = GetTrimElementValue(element);
			newValue = ' ' + whiteSpaceRegex.Replace(newValue.Trim(), " ") + ' ';

			insertAfterValue = whiteSpaceRegex.Replace(insertAfterValue.Trim(), " ");
			var headSpan = source.AsSpan(0,insertAfterValue.Length);
			var tailSpan = source.AsSpan(insertAfterValue.Length + 1);
			element.SetValue(String.Concat(headSpan, newValue, tailSpan));
		}

		public static void AppendText(PackFile packFile, string path, string newValue)
		{
			XElement element = packFile.SafeNavigateTo(path);
			string source = GetTrimElementValue(element);

			newValue = whiteSpaceRegex.Replace(newValue.Trim(), " ");
			element.SetValue(source.Append(' ', newValue+Environment.NewLine));
		}
		public static void RemoveText(PackFile packFile, string path, string value)
		{
			XElement element = packFile.SafeNavigateTo(path);
			if (String.IsNullOrWhiteSpace(value)) return;
			string source = element.Value;
			value = whiteSpaceRegex.Replace(value.Trim(), " "); 

			element.SetValue(source.Replace(value, string.Empty, true));
		}
	}
}
