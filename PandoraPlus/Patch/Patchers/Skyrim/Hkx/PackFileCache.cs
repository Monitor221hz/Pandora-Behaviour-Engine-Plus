using Pandora.Core.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
public class PackFileCache
{
	private Dictionary<string, PackFile> pathMap = new Dictionary<string, PackFile>();


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

	public void DeletePackFileOutput()
	{
		lock (pathMap)
		{
			foreach (var packFile in pathMap.Values)
			{
				lock (packFile)
				{
					if (packFile.OutputHandle.Exists) { packFile.OutputHandle.Delete(); }
				}
			}
		}
	}
}
