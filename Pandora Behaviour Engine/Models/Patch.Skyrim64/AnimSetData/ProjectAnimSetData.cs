// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Extensions;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class ProjectAnimSetDataFactory : IStreamReaderFactory<IProjectAnimSetData>
{
	public bool TryCreate(
		StreamReader reader,
		[NotNullWhen(true)] out IProjectAnimSetData? animSetData
	)
	{
		return ProjectAnimSetData.TryRead(reader, out animSetData);
	}
}

public class ProjectAnimSetData : IProjectAnimSetData
{
	public ProjectAnimSetData(
		int numSets,
		IList<string> animSetFileNames,
		IList<IAnimSet> animSets,
		Dictionary<string, IAnimSet> animSetsByName
	)
	{
		NumSets = numSets;
		AnimSetFileNames = animSetFileNames;
		AnimSets = animSets;
		AnimSetsByName = animSetsByName;
	}

	public int NumSets { get; private set; } = 1;

	public IList<string> AnimSetFileNames { get; private set; } = [];

	public IList<IAnimSet> AnimSets { get; private set; } = [];

	public Dictionary<string, IAnimSet> AnimSetsByName { get; private set; } = [];

	public static bool TryRead(
		StreamReader reader,
		[NotNullWhen(true)] out IProjectAnimSetData? setData
	)
	{
		setData = null;
		if (!int.TryParse(reader.ReadLine(), out int numSets))
		{
			return false;
		}

		string[] animSetFileNames = new string[numSets];
		IAnimSet[] animSets = new IAnimSet[numSets];
		Dictionary<string, IAnimSet> animSetNameMap = new(
			numSets,
			StringComparer.OrdinalIgnoreCase
		);
		for (int i = 0; i < numSets; i++)
		{
			if (!reader.TryReadLine(out var fileName))
			{
				return false;
			}
			animSetFileNames[i] = fileName;
		}
		for (int i = 0; i < numSets; i++)
		{
			if (!AnimSet.TryRead(reader, out var animSet))
			{
				return false;
			}
			animSets[i] = animSet;
			animSetNameMap.Add(animSetFileNames[i], animSet);
		}
		setData = new ProjectAnimSetData(numSets, animSetFileNames, animSets, animSetNameMap);
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
