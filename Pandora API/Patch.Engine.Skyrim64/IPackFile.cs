using HKX2E;
namespace Pandora.API.Patch.Engine.Skyrim64;

public interface IPackFile
{
	hkRootLevelContainer Container { get; }
	string Name { get; }
	int NodeCount { get; }
	string UniqueName { get; }

	public FileInfo OutputHandle { get; }
	IProject GetProject(); 
	T GetPushedObjectAs<T>(string name) where T : class, IHavokObject;
	bool PathExists(string nodeName, string path);
	bool TargetExists(string nodeName);
	public FileInfo GetRebasedOutput(DirectoryInfo exportDirectory);
}

