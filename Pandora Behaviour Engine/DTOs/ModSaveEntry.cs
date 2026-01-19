using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Pandora.DTOs;

public record ModSaveEntry(string Code, bool Active, uint Priority);

[JsonSerializable(typeof(List<ModSaveEntry>))]
[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class ModsJsonContext : JsonSerializerContext;
