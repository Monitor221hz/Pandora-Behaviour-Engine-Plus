using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
public class PackFileCache
{
	private Dictionary<string, PackFile> pathMap = new Dictionary<string, PackFile>(StringComparer.OrdinalIgnoreCase);
	private static readonly FileInfo PreviousOutputFile = new FileInfo(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName,"Pandora_Engine\\PreviousOutput.txt"));

	private Dictionary<PackFile, List<Project>> sharedPackFileProjectMap = new();
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
