using System;
using System.IO;

namespace Pandora.Logging.NLogger.Environment;

public interface ILogPathProvider
{
	DirectoryInfo Current { get; }
	IObservable<DirectoryInfo> Changed { get; }
}
