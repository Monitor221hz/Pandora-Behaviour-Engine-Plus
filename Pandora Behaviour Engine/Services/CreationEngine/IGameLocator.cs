using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Services.CreationEngine;

public interface IGameLocator
{
	DirectoryInfo? TryLocateGameData();
}
