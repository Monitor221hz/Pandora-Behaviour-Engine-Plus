using Pandora.Paths.Configuration.DTOs;
using System.Text.Json.Serialization;

namespace Pandora.Paths.Contexts;

[JsonSerializable(typeof(PathsConfiguration))]
[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class PathsJsonContext : JsonSerializerContext;