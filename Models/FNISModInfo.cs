using Pandora.API.Patch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pandora.Core;

public partial class FNISModInfo : IModInfo
{

	private readonly static Regex whiteSpaceRegex = WhiteSpaceRegex();
	public string Name { get; set; }
  
	public override int GetHashCode()
	{
		return Code.GetHashCode();
	}
	public bool Equals(IModInfo? other)
	{
		return other == null ? false :
			Code == other.Code &&
			Version == other.Version;
	}

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
		Name = Path.GetFileNameWithoutExtension(file.Name);
		Folder = file.Directory!;
		Code = whiteSpaceRegex.Replace(Name, string.Empty);
	}

	[GeneratedRegex(@"\s+", RegexOptions.Compiled)]
	private static partial Regex WhiteSpaceRegex();
}
