// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pandora.DTOs;

public record ModSaveEntry(string Code, bool Active, uint Priority);

[JsonSerializable(typeof(List<ModSaveEntry>))]
[JsonSourceGenerationOptions(
	WriteIndented = true,
	PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
public partial class ModsJsonContext : JsonSerializerContext;
