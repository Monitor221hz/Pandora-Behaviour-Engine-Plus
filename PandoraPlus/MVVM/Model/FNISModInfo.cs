using Pandora.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.MVVM.Model;

public class FNISModInfo : IModInfo
{
	public string Name { get; private set; }

	public string Description { get; private set; } = string.Empty;

	public string Author { get; private set; } = "unknown";

	public Version Version { get; private set; } = new Version();

	public IModInfo.ModFormat Format { get; } = IModInfo.ModFormat.FNIS;

	public string URL { get; private set; } = "null";

	public string Code { get; private set; } = "n/a";

	public DirectoryInfo Folder { get; private set; }

	public bool Active { get; set; } = true;
	public uint Priority { get;  set; } = 0;

	public FNISModInfo(FileInfo file)
	{
		Name = file.Name;
		Folder = file.Directory!;
	}
}
