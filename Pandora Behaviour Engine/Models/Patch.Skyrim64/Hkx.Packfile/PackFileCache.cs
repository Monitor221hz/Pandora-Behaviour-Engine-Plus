using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public class PackFileCache : IPackFileCache
{
	private Dictionary<string, PackFile> pathMap = new(StringComparer.OrdinalIgnoreCase);
	private static readonly FileInfo PreviousOutputFile = new(Path.Combine(Environment.CurrentDirectory, "Pandora_Engine\\PreviousOutput.txt"));

	private Dictionary<PackFile, List<Project>> sharedPackFileProjectMap = [];
	public PackFileCache() { }
	public PackFile LoadPackFile(FileInfo file)
	{

		PackFile? packFile = null;

		lock (pathMap)
		{
			if (!pathMap.TryGetValue(file.FullName, out packFile))
			{
				packFile = new PackFile(file);
				pathMap.Add(file.FullName, packFile);
			}
		}

		return packFile;
	}

	public PackFileGraph LoadPackFileGraph(FileInfo file)
	{
		PackFile? packFile = null;
		lock (pathMap)
		{
			if (!pathMap.TryGetValue(file.FullName, out packFile))
			{
				packFile = new PackFileGraph(file);
				pathMap.Add(file.FullName, packFile);
			}
		}

		return (PackFileGraph)packFile;
	}

	public PackFileGraph LoadPackFileGraph(FileInfo file, Project project)
	{
		PackFile? packFile = null;
		lock (pathMap)
		{
			if (!pathMap.TryGetValue(file.FullName, out packFile))
			{
				packFile = new PackFileGraph(file, project);
				pathMap.Add(file.FullName, packFile);

			}
		}

		return (PackFileGraph)packFile;
	}

	public PackFileCharacter LoadPackFileCharacter(FileInfo file)
	{
		PackFile? packFile = null;
		lock (pathMap)
		{
			if (!pathMap.TryGetValue(file.FullName, out packFile))
			{
				packFile = new PackFileCharacter(file);
				pathMap.Add(file.FullName, packFile);
			}
		}

		return (PackFileCharacter)packFile;
	}

	public PackFileCharacter LoadPackFileCharacter(FileInfo file, Project project)
	{
		PackFile? packFile = null;
		lock (pathMap)
		{
			if (!pathMap.TryGetValue(file.FullName, out packFile))
			{
				packFile = new PackFileCharacter(file, project);
				pathMap.Add(file.FullName, packFile);
			}
		}

		return (PackFileCharacter)packFile;
	}

	public PackFileSkeleton LoadPackFileSkeleton(FileInfo file)
	{
		PackFile? packFile = null;
		lock (pathMap)
		{
			if (!pathMap.TryGetValue(file.FullName, out packFile))
			{
				packFile = new PackFileSkeleton(file);
				pathMap.Add(file.FullName, packFile);
			}
		}

		return (PackFileSkeleton)packFile;
	}

	public PackFileSkeleton LoadPackFileSkeleton(FileInfo file, Project project)
	{
		PackFile? packFile = null;
		lock (pathMap)
		{
			if (!pathMap.TryGetValue(file.FullName, out packFile))
			{
				packFile = new PackFileSkeleton(file, project);
				pathMap.Add(file.FullName, packFile);
			}
		}

		return (PackFileSkeleton)packFile;
	}

	public bool TryLookupSharedProjects(PackFile packFile, [NotNullWhen(true)] out List<Project>? projects)
	{
		return sharedPackFileProjectMap.TryGetValue(packFile, out projects);
	}
}
