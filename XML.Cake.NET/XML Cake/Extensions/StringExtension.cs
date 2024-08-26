
using System.Xml.Linq; 



namespace XmlCake.String; 

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
		return (insertValue.Length > 0) ? value.Substring(0, index) + joinedValues + value.Substring(index + joinedValues.Length) : joinedValues + value;
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

		return self.Substring(0, pos) + newValue
			   + self.Substring(pos + oldValue.Length);
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
		return self.Substring(0, pos) + newValue
	   + self.Substring(pos + oldValue.Length);
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
	//public static void RemoveLine(this XElement element, char separator, params int[] indexes)
	//{
	//    List<string> lines = element.Value.Split(separator).ToList();
	//    foreach(int index in indexes)
	//    {
	//        lines[index] = string.Empty; 
	//    }
	//    element.SetValue(String.Join(Environment.NewLine, lines.Where(s => String.IsNullOrEmpty(s))));
	//}



	//public static void AppendLine(this XElement element, char separator, params string[] lines)
	//{
	//    StringBuilder builder = new StringBuilder(element.Value);

	//    foreach (string line in lines)
	//    {
	//        builder.Append(Environment.NewLine);
	//        builder.Append(line);
	//    }

	//    element.SetValue(builder.ToString()); 

	//}


	public static void RemoveValue(this XElement element) => element.SetValue(string.Empty); 

}