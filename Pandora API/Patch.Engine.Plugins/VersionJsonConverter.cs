using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pandora.API.Patch.Engine.Plugins;

public class VersionJsonConverter : JsonConverter<Version>
{
	public override Version? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var str = reader.GetString();
		return str == null ? null : Version.Parse(str);
	}

	public override void Write(Utf8JsonWriter writer, Version value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString());
}
