// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.Paths.Configuration.DTOs;
using System.Text.Json.Serialization;

namespace Pandora.Paths.Contexts;

[JsonSerializable(typeof(PathsConfiguration))]
[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class PathsJsonContext : JsonSerializerContext;