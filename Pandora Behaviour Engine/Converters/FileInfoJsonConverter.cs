// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pandora.Converters;

public class FileInfoJsonConverter : JsonConverter<FileInfo?>
{
	public static readonly FileInfoJsonConverter Instance = new();

	public override FileInfo? Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options
	)
	{
		return new FileInfo(reader.GetString()!);
	}

	public override void Write(
		Utf8JsonWriter writer,
		FileInfo? value,
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
