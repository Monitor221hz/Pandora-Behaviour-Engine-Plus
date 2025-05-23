using Pandora.Models.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class ProjectAnimSetData
{
	public int NumSets { get; private set; } = 1;

	public List<string> AnimSetFileNames { get; private set; } = [];

	public List<AnimSet> AnimSets { get; private set; } = [];

	public Dictionary<string, AnimSet> AnimSetsByName { get; private set; } = [];

	public static ProjectAnimSetData Read(StreamReader reader)
	{
		var setData = new ProjectAnimSetData();


		if (!int.TryParse(reader.ReadLine(), out int numSets)) { return setData; }
		setData.NumSets = numSets;

		for (int i = 0; i < numSets; i++)
		{
			var fileName = reader.ReadLineOrEmpty();
			setData.AnimSetFileNames.Add(fileName);
		}

		for (int i = 0; i < numSets; i++)
		{
			var animSet = AnimSet.Read(reader);
			setData.AnimSets.Add(animSet);
			setData.AnimSetsByName.Add(setData.AnimSetFileNames[i], animSet);
		}

		return setData;
	}

	public override string ToString()
	{
		StringBuilder sb = new();

		sb.AppendLine(NumSets.ToString());
		if (NumSets > 0)
		{
			sb.AppendJoin("\r\n", AnimSetFileNames).AppendLine();
			sb.AppendJoin("", AnimSets);
		}

		return sb.ToString();

	}
}