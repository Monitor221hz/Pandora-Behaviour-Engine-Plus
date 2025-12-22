// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Extensions;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class SetCondition : ISetCondition
{
	public SetCondition(string variableName, int value1, int value2)
	{
		VariableName = variableName;
		Value1 = value1;
		Value2 = value2;
	}

	public string VariableName { get; private set; } = string.Empty;

	public int Value1 { get; private set; } = 0;

	public int Value2 { get; private set; } = 0;

	public static bool TryRead(
		StreamReader reader,
		[NotNullWhen(true)] out ISetCondition? condition
	)
	{
		condition = null;
		if (
			!reader.TryReadLine(out var variableName)
			|| !int.TryParse(reader.ReadLine(), out int value1)
			|| !int.TryParse(reader.ReadLine(), out int value2)
		)
		{
			return false;
		}
		condition = new SetCondition(variableName, value1, value2);
		return true;
	}

	public override string ToString()
	{
		StringBuilder sb = new();

		sb.AppendLine(VariableName);
		sb.AppendLine(Value1.ToString());
		sb.AppendLine(Value2.ToString());

		return sb.ToString();
	}
}
