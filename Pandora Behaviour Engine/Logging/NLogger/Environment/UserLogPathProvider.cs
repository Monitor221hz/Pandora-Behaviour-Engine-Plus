using Pandora.Paths.Abstractions;
using System;
using System.IO;

namespace Pandora.Logging.NLogger.Environment;

public sealed class UserLogPathProvider(IUserPaths userPaths) : ILogPathProvider
{
	public DirectoryInfo Current => userPaths.Output;

	public IObservable<DirectoryInfo> Changed => userPaths.OutputChanged;
}
