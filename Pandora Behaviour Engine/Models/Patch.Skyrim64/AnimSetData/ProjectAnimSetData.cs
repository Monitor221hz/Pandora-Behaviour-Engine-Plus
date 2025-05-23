using Pandora.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class ProjectAnimSetData
{
	public ProjectAnimSetData(int numSets, IList<string> animSetFileNames, IList<AnimSet> animSets, Dictionary<string, AnimSet> animSetsByName)
	{
		NumSets = numSets;
		AnimSetFileNames = animSetFileNames;
		AnimSets = animSets;
		AnimSetsByName = animSetsByName;
	}

	public int NumSets { get; private set; } = 1;

	public IList<string> AnimSetFileNames { get; private set; } = [];

	public IList<AnimSet> AnimSets { get; private set; } = [];

	public Dictionary<string, AnimSet> AnimSetsByName { get; private set; } = [];
	public static bool TryRead(StreamReader reader, [NotNullWhen(true)] out ProjectAnimSetData? setData)
	{
		setData = null;
		if (!int.TryParse(reader.ReadLine(), out int numSets)) { return false; }

		string[] animSetFileNames = new string[numSets];
		AnimSet[] animSets = new AnimSet[numSets];
		Dictionary<string, AnimSet> animSetNameMap = new(numSets, StringComparer.OrdinalIgnoreCase);
		for (int i = 0; i < numSets; i++)
		{
			if (!reader.TryReadLine(out var fileName)) { return false; }
			animSetFileNames[i] = fileName;
		}
		for (int i = 0; i < numSets; i++)
		{
			if (!AnimSet.TryRead(reader, out var animSet)) { return false; }
			animSets[i] = animSet;
			animSetNameMap.Add(animSetFileNames[i], animSet);
		}
		setData = new(numSets, animSetFileNames, animSets, animSetNameMap);
		return true;
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