using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Core.Extensions;
public static class StringExtension
{


	public static string Insert(this string value, int index, string separator, params string[] values)
	{
		List<string> sections = value.Split(separator).ToList();

		sections.InsertRange(index, values);

		return string.Join(separator, sections);
	}

	public static string Insert(this string value, int index, char separator, params string[] values)
	{
		List<string> sections = value.Split(separator).ToList();

		sections.InsertRange(index, values);

		return string.Join(separator, sections);
	}

	public static string Insert(this string value, string insertValue, char separator, params string[] values)
	{

		int index = value.IndexOf(insertValue) + insertValue.Length;
		string joinedValues = separator + string.Join(separator, values) + separator;
		//sections.InsertRange(index, values);
		return (insertValue.Length > 0) ? string.Concat(value.AsSpan(0, index), joinedValues, value.AsSpan(index + joinedValues.Length)) : joinedValues + value;
	}

	public static string Append(this string value, char separator, params string[] values)
	{
		List<string> sections = value.Split(separator).ToList();
		sections[sections.Count - 1] = sections.Last().TrimEnd().TrimEnd('\r', '\n', '\t');

		sections.AddRange(values);

		return string.Join(separator, sections);
	}

	public static string Append(this string value, params string[] values)
	{
		string separator = Environment.NewLine;
		List<string> sections = value.Split(separator).ToList();
		sections[sections.Count - 1] = sections.Last().TrimEnd().TrimEnd('\r', '\n', '\t');
		sections.AddRange(values);

		return string.Join(separator, sections);

	}
	public static string Insert(this string value, int index, params string[] values)
	{
		string separator = Environment.NewLine;
		List<string> sections = value.Split(separator).ToList();

		sections.InsertRange(index, values);

		return string.Join(separator, sections);
	}
	public static string Replace(this string self,
								  string oldValue, string newValue,
								  bool firstOccurrenceOnly = false)
	{
		if (!firstOccurrenceOnly)
			return self.Replace(oldValue, newValue);

		int pos = self.IndexOf(oldValue);
		if (pos < 0)
			return self;

		return string.Concat(self.AsSpan(0, pos), newValue
, self.AsSpan(pos + oldValue.Length));
	}

	public static string Replace(this string self, string oldValue, string newValue, int index)
	{
		int pos = -1;
		int newPos = 0;
		for (int i = 0; i < index; i++)
		{
			newPos = self.IndexOf(oldValue, newPos);
			if (newPos < 0) { break; }
			pos = newPos;
			newPos += oldValue.Length;
		}
		if (pos < 0) return self;
		return string.Concat(self.AsSpan(0, pos), newValue
, self.AsSpan(pos + oldValue.Length));
	}
	public static IEnumerable<int> IndexesOf(this string str, string searchstring)
	{
		int minIndex = str.IndexOf(searchstring);
		while (minIndex != -1)
		{
			yield return minIndex;
			minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
		}
	}
	public static void RemoveValue(this XElement element) => element.SetValue(string.Empty);

}
