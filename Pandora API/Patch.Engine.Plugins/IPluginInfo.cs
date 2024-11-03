
namespace Pandora.API.Patch.Engine.Plugins;

public interface IPluginInfo
{
	public const string FILE_HEADER = "plugin"; 
	string Author { get; set; }
	string RelativePath { get; set; }
	string Name { get; set; }
	Version Version { get; set; }
}