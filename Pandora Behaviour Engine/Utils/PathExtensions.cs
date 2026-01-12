using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Utils;

static class PathExtensions
{
	extension(string)
	{
		public static string operator /(string left, string right)
			=> Path.Combine(left, right);
	}
}
