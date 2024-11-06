
namespace Pandora.API.Patch.Engine.Plugins;
/// <summary>
/// UNSAFE - DO NOT USE
/// </summary>
public interface IPluginInfo
{
	public const string FILE_HEADER = "plugin";
	string Name { get; set; }
	string Author { get; set; }
	string Path { get; set; }
}