using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Pandora.Models.Extensions
{
	public static class StreamExtensions
	{
		public static bool TryReadLine(this StreamReader reader,[NotNullWhen(true)] out string? line)
		{
			line = reader.ReadLine();
			return line != null;
		}
		public static string ReadLineOrEmpty(this StreamReader reader) => reader.ReadLine() ?? string.Empty;
	}
}
