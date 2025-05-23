using System.IO;

namespace Pandora.Models.Extensions;

public static class StreamExtensions
{
	public static string ReadLineSafe(this StreamReader reader) => reader.ReadLine() ?? string.Empty;
}
