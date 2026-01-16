using Pandora.Paths.Contexts;
using Pandora.Platform.CreationEngine;
using System.IO;

namespace Pandora.Paths.Services;

public abstract class UserPathServiceBase
{
	protected readonly IUserPathContext UserPaths;
	protected readonly IGameDescriptor Descriptor;
	protected readonly IPathConfigService Config;

	protected UserPathServiceBase(
		IUserPathContext userPaths,
		IGameDescriptor descriptor,
		IPathConfigService config)
	{
		UserPaths = userPaths;
		Descriptor = descriptor;
		Config = config;
	}

	protected void SaveGameData(DirectoryInfo dir) =>
		Config.SaveGameDataPath(Descriptor.Id, dir);

	protected void SaveOutput(DirectoryInfo dir) =>
		Config.SaveOutputPath(Descriptor.Id, dir);
}
