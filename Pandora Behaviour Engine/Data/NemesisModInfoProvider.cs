using Pandora.API.Patch;
using Pandora.Models.Patch.Mod;
using System.IO;
using System.Threading.Tasks;

namespace Pandora.Data;

public class NemesisModInfoProvider : FileBasedModInfoProvider
{
	protected override string InfoFileName => "info.ini";

	public override string SingleRelativePath => Path.Join("Nemesis_Engine", "mod");

	protected override Task<IModInfo?> TryParseAsync(FileInfo infoFile)
	{
		IModInfo? mod = NemesisModInfo.ParseMetadata(infoFile);
		return Task.FromResult(mod);
	}
}
