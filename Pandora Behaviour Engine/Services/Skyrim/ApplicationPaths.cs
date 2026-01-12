using System.Diagnostics;
using System.IO;

namespace Pandora.Services.Skyrim;

public sealed class ApplicationPaths : IApplicationPaths
{
	public DirectoryInfo AssemblyDirectory { get; } =
		new(Path.GetDirectoryName(
			Process.GetCurrentProcess().MainModule!.FileName!)!
		);
}
