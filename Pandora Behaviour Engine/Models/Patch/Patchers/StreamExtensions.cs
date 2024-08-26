using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers
{
    public static class StreamExtensions
    {
        public static string ReadLineSafe(this StreamReader reader) => reader.ReadLine() ?? string.Empty;
    }
}
