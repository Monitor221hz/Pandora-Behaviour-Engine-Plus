// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pandora.Converters;

public class DirectoryInfoJsonConverter : JsonConverter<DirectoryInfo?>
{
	public static readonly DirectoryInfoJsonConverter Instance = new();

	public override DirectoryInfo? Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options
	)
	{
		return new DirectoryInfo(reader.GetString()!);
	}

	public override void Write(
		Utf8JsonWriter writer,
		DirectoryInfo? value,
		JsonSerializerOptions options
	)
	{
		if (value == null)
		{
			writer.WriteNullValue();
			return;
		}
		writer.WriteStringValue(value.FullName);
	}
}
