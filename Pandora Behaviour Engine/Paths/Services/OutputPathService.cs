using Pandora.Paths.Contexts;
using Pandora.Platform.CreationEngine;
using System.IO;

namespace Pandora.Paths.Services;

public sealed class OutputPathService : UserPathServiceBase, IOutputPathService
{
	public OutputPathService(
		IUserPathContext userPaths,
		IGameDescriptor descriptor,
		IPathConfigService config)
		: base(userPaths, descriptor, config)
	{

	}
	public void Initialize()
	{
		var saved = Config.GetGamePaths(Descriptor.Id).OutputDirectory;
		if (saved is { Exists: true })
			SetOutputFolder(saved);
	}

	public void SetOutputFolder(DirectoryInfo dir)
	{
		if (!dir.Exists)
			dir.Create();

		UserPaths.SetOutput(dir);
		SaveOutput(dir);
	}

}
