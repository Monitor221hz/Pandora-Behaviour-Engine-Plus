using Pandora.Services.Interfaces;
using System.Diagnostics;
using System.IO;

namespace Pandora.Services.Paths;

public sealed class ApplicationPaths : IApplicationPaths
{
	public DirectoryInfo AssemblyDirectory { get; } =
		new(Path.GetDirectoryName(
			Process.GetCurrentProcess().MainModule!.FileName!)!
		);
}
