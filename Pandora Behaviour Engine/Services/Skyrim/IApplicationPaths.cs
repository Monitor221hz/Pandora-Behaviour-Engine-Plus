using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Services.Skyrim;

public interface IApplicationPaths
{
	DirectoryInfo AssemblyDirectory { get; }
}
