using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public interface IPackFileCache
{
	PackFile LoadPackFile(FileInfo file);
	PackFileCharacter LoadPackFileCharacter(FileInfo file);
	PackFileCharacter LoadPackFileCharacter(FileInfo file, Project project);
	PackFileGraph LoadPackFileGraph(FileInfo file);
	PackFileGraph LoadPackFileGraph(FileInfo file, Project project);
	PackFileSkeleton LoadPackFileSkeleton(FileInfo file);
	PackFileSkeleton LoadPackFileSkeleton(FileInfo file, Project project);
	bool TryLookupSharedProjects(PackFile packFile, [NotNullWhen(true)] out List<Project>? projects);
}