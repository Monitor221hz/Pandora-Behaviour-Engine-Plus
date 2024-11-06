using System.Collections.Generic;
using System.Text;

namespace Pandora.API.Patch.Engine.Skyrim64;
public interface IProjectManager
{
	void GetAnimationInfo(StringBuilder builder);
	void GetExportInfo(StringBuilder builder);
	void GetFNISInfo(StringBuilder builder);
	IProject? LoadProjectEx(string projectFilePath);
	IProject? LoadProjectHeaderEx(string projectFilePath);
	void LoadProjects(List<string> projectPaths);
	bool ProjectExists(string name);
	bool ProjectLoaded(string name);
	bool TryActivatePackFileEx(IPackFile packFile);
	bool TryGetProjectEx(string name, out IProject? project);
	bool TryLookupPackFileEx(string name, out IPackFile? packFile);
	bool TryLookupPackFileEx(string projectName, string packFileName, out IPackFile? packFile);
	bool TryLookupProjectFolderEx(string name, out IProject? project);
}