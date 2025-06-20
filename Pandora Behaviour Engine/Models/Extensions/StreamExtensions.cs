using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Pandora.Models.Extensions;

public static class StreamExtensions
{
	public static bool TryReadLine(this StreamReader reader, [NotNullWhen(true)] out string? line)
	{
		line = reader.ReadLine();
		return line != null;
	}
	public static bool TryReadNotEmptyLine(this StreamReader reader, [NotNullWhen(true)] out string? line)
	{
		line = reader.ReadLine();
		return !string.IsNullOrEmpty(line);
	}
	public static string ReadLineOrEmpty(this StreamReader reader) => reader.ReadLine() ?? string.Empty;
}
