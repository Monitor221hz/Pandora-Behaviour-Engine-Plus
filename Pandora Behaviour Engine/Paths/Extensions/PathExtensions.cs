using System.IO;

namespace Pandora.Paths.Extensions;

public static class PathExtensions
{
	extension(string)
	{
		public static string operator /(string left, string right)
			=> Path.Combine(left, right);
	}
}
