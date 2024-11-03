
namespace Pandora.API.Patch.Engine.Plugins;

public interface IPluginInfo
{
	public const string FILE_HEADER = "plugin";
	string Name { get; set; }
	string Author { get; set; }
	string Path { get; set; }
}