using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pandora.DTOs;

public record ModSaveEntry(string Code, bool Active, uint Priority);

[JsonSerializable(typeof(List<ModSaveEntry>))]
[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class PandoraJsonContext : JsonSerializerContext { }
