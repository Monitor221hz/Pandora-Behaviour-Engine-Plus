// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Pandora.API.Patch;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.Skyrim.AnimData;
using Pandora.Skyrim.AnimSetData;

namespace Pandora.Models.Patch.Skyrim64.Format.Nemesis;

public sealed class NemesisCachePatcher
{
	private const string DeleteSentinel = "//* delete this line *//";

	private readonly Dictionary<string, int> _nextBase = new(StringComparer.OrdinalIgnoreCase);

	public void Apply(
		IModInfo mod,
		AnimDataManager animData,
		AnimSetDataManager animSets,
		IProjectManager projects
	)
	{
		string modcode = mod.Code.Trim().ToLowerInvariant();
		var insertedNames = ReadCharacterNames(mod.Folder);
		var handled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (DirectoryInfo cache in FindCacheDirectories(mod.Folder))
		{
			if (IsAnimSetDirectory(cache))
			{
				ApplyAnimSets(cache, modcode, animSets);
			}
			else if (IsAnimDataDirectory(cache))
			{
				ApplyAnimData(cache, modcode, animData, projects, insertedNames, handled);
			}
		}

		foreach (var (project, names) in insertedNames)
		{
			if (names.Count == 0 || handled.Contains(project))
			{
				continue;
			}
			AppendCharacterNames(projects, project, names, out Dictionary<string, int> pathIndex);
			AssignBindingIndexes(projects, project, pathIndex);
		}
	}

	private void ApplyAnimData(
		DirectoryInfo root,
		string modcode,
		AnimDataManager animData,
		IProjectManager projects,
		Dictionary<string, List<string>> insertedNames,
		HashSet<string> handled
	)
	{
		List<string>? projectList = null;
		foreach (DirectoryInfo folder in root.EnumerateDirectories())
		{
			if (folder.Name.Equals("$header$", StringComparison.OrdinalIgnoreCase))
			{
				FileInfo list = new(Path.Combine(folder.FullName, "$header$.txt"));
				if (list.Exists)
				{
					projectList = Splice(File.ReadAllLines(list.FullName))
						.Where(line => line.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
						.ToList();
				}
				continue;
			}

			string projectName = ProjectName(folder.Name);
			int index = animData.IndexOfProject(projectName);
			if (index < 0)
			{
				animData.AddProject(projectName, ReadNewProject(folder, animData));
				continue;
			}

			ProjectAnimData project = animData.AnimDataAt(index)!;
			handled.Add(projectName);
			ApplyExistingProject(folder, projectName, project, modcode, projects, insertedNames);
		}

		if (projectList != null)
		{
			animData.ReorderProjects(projectList);
		}
	}

	private void ApplyExistingProject(
		DirectoryInfo folder,
		string projectName,
		ProjectAnimData project,
		string modcode,
		IProjectManager projects,
		Dictionary<string, List<string>> insertedNames
	)
	{
		List<PatchFile> files = ReadPatchFiles(folder);
		var tokens = new HashSet<int>();
		foreach (PatchFile file in files)
		{
			CollectTokens(file, modcode, tokens);
		}

		insertedNames.TryGetValue(projectName, out List<string>? names);
		names ??= [];
		if (tokens.Count > 0 && tokens.Min() != 0)
		{
			throw new InvalidDataException($"{projectName} cache tokens do not start at 0.");
		}
		if (tokens.Count > 0 && tokens.Max() + 1 != tokens.Count)
		{
			throw new InvalidDataException($"{projectName} cache tokens are not contiguous.");
		}

		int baseIndex = BaseFor(projectName, project);
		if (tokens.Count > 0)
		{
			_nextBase[projectName] = baseIndex + tokens.Count;
		}

		if (names.Count > 0)
		{
			AppendCharacterNames(
				projects,
				projectName,
				names,
				out Dictionary<string, int> pathIndex
			);
			AssignBindingIndexes(projects, projectName, pathIndex);
		}

		FileInfo? header = files
			.Select(file => file.Info)
			.FirstOrDefault(info =>
				info.Name.Equals("$header$.txt", StringComparison.OrdinalIgnoreCase)
			);
		if (header != null)
		{
			ApplyProjectHeader(project, File.ReadAllLines(header.FullName));
		}

		var motions = new Dictionary<string, PatchFile>(StringComparer.OrdinalIgnoreCase);
		var clips = new List<PatchFile>();
		foreach (PatchFile file in files)
		{
			if (file.Info.Name.Equals("$header$.txt", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			if (file.Info.Name.Equals("$order$.txt", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			if (file.IsClip)
			{
				clips.Add(file);
			}
			else
			{
				motions[Path.GetFileNameWithoutExtension(file.Info.Name)] = file;
			}
		}

		clips.Sort(
			(left, right) => TokenOrder(left, modcode).CompareTo(TokenOrder(right, modcode))
		);

		foreach (PatchFile clip in clips)
		{
			ApplyClip(project, clip, motions, modcode, baseIndex);
		}

		foreach (PatchFile motion in motions.Values)
		{
			ApplyLooseMotion(project, motion, modcode, baseIndex);
		}
	}

	private static void ApplyClip(
		ProjectAnimData project,
		PatchFile clip,
		Dictionary<string, PatchFile> motions,
		string modcode,
		int baseIndex
	)
	{
		string fileStem = Path.GetFileNameWithoutExtension(clip.Info.Name);
		int split = fileStem.LastIndexOf('~');
		string clipName = split < 0 ? fileStem : fileStem[..split];
		string fileCode = split < 0 ? string.Empty : fileStem[(split + 1)..];
		List<string> body = clip.HasMarkers ? Splice(clip.Lines) : clip.Lines.ToList();
		body = TrimBlank(body);
		if (body.Count == 0)
		{
			if (fileCode.Length > 0)
			{
				project.TryRemoveClip(clipName, Resolve(fileCode, modcode, baseIndex));
				project.TryRemoveClip(clipName, fileCode);
			}
			return;
		}

		string resolved = Resolve(body[1], modcode, baseIndex);
		body[1] = resolved;
		ClipDataBlock block = ClipFromLines(body);
		string originalId = fileCode.Length == 0 ? resolved : Resolve(fileCode, modcode, baseIndex);
		if (
			!project.TryReplaceClip(block.Name, originalId, block)
			&& !project.TryReplaceClip(block.Name, fileCode, block)
		)
		{
			ClipMotionDataBlock? motion = null;
			if (
				motions.Remove(fileCode.Length == 0 ? body[1] : fileCode, out PatchFile? motionFile)
			)
			{
				motion = MotionFromFile(motionFile, resolved, modcode, baseIndex);
			}
			else if (motions.Remove(resolved, out motionFile))
			{
				motion = MotionFromFile(motionFile, resolved, modcode, baseIndex);
			}
			project.AddClipData(block, motion);
		}
		else if (
			motions.Remove(fileCode, out PatchFile? editedMotion)
			|| motions.Remove(resolved, out editedMotion)
		)
		{
			ClipMotionDataBlock motion = MotionFromFile(editedMotion, resolved, modcode, baseIndex);
			if (project.BoundMotionDataProject is MotionData data)
			{
				if (!data.ReplaceMotion(originalId, motion))
				{
					data.ReplaceMotion(fileCode, motion);
					if (!data.ReplaceMotion(resolved, motion))
					{
						data.AddClipMotionData(motion);
					}
				}
			}
		}
	}

	private static void ApplyLooseMotion(
		ProjectAnimData project,
		PatchFile motion,
		string modcode,
		int baseIndex
	)
	{
		string code = Path.GetFileNameWithoutExtension(motion.Info.Name);
		string resolved = Resolve(code, modcode, baseIndex);
		ClipMotionDataBlock block = MotionFromFile(motion, resolved, modcode, baseIndex);
		if (project.BoundMotionDataProject is not MotionData data)
		{
			data = new MotionData([], []);
			project.BoundMotionDataProject = data;
			project.Header.HasMotionData = 1;
		}
		if (
			!data.ReplaceMotion(Resolve(code, modcode, baseIndex), block)
			&& !data.ReplaceMotion(code, block)
		)
		{
			data.AddClipMotionData(block);
		}
	}

	private static ProjectAnimData ReadNewProject(DirectoryInfo folder, AnimDataManager manager)
	{
		FileInfo headerFile = new(Path.Combine(folder.FullName, "$header$.txt"));
		string[] headerLines = File.ReadAllLines(headerFile.FullName);
		int cursor = 0;
		if (
			headerLines.Length > 0
			&& int.TryParse(headerLines[0], out int declared)
			&& declared > 1
		)
		{
			cursor = 1;
		}
		int hasFileList = int.Parse(headerLines[cursor++], CultureInfo.InvariantCulture);
		int fileCount = int.Parse(headerLines[cursor++], CultureInfo.InvariantCulture);
		var assets = new List<string>(fileCount);
		for (int i = 0; i < fileCount; i++)
		{
			assets.Add(headerLines[cursor++]);
		}
		int hasCache = int.Parse(headerLines[cursor], CultureInfo.InvariantCulture);
		var header = new ProjectAnimDataHeader(hasFileList, fileCount, assets, hasCache);

		List<PatchFile> files = ReadPatchFiles(folder);
		var clips = new List<IClipDataBlock>();
		var motions = new List<IClipMotionDataBlock>();
		var byId = new Dictionary<int, IClipMotionDataBlock>();
		foreach (PatchFile file in Ordered(folder, files))
		{
			if (file.Info.Name.Equals("$header$.txt", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			if (file.IsClip)
			{
				List<string> body = TrimBlank(file.Lines.ToList());
				clips.Add(ClipFromLines(body));
			}
			else
			{
				List<string> body = TrimBlank(file.Lines.ToList());
				ClipMotionDataBlock motion = MotionFromLines(body);
				motions.Add(motion);
				if (int.TryParse(motion.ClipID, out int id))
				{
					byId[id] = motion;
				}
			}
		}

		var project = new ProjectAnimData(header, clips, manager);
		if (hasCache != 0)
		{
			project.BoundMotionDataProject = new MotionData(motions, byId);
		}
		return project;
	}

	private static IEnumerable<PatchFile> Ordered(DirectoryInfo folder, List<PatchFile> files)
	{
		FileInfo order = new(Path.Combine(folder.FullName, "$order$.txt"));
		if (!order.Exists)
		{
			return files.Where(file =>
				!file.Info.Name.Equals("$header$.txt", StringComparison.OrdinalIgnoreCase)
				&& !file.Info.Name.Equals("$order$.txt", StringComparison.OrdinalIgnoreCase)
			);
		}

		var byName = files.ToDictionary(file => file.Info.Name, StringComparer.OrdinalIgnoreCase);
		var ordered = new List<PatchFile>();
		foreach (string line in File.ReadAllLines(order.FullName))
		{
			if (line.Length == 0)
			{
				continue;
			}
			if (byName.Remove(line, out PatchFile? file))
			{
				ordered.Add(file);
			}
		}
		ordered.AddRange(
			byName.Values.Where(file =>
				!file.Info.Name.Equals("$header$.txt", StringComparison.OrdinalIgnoreCase)
			)
		);
		return ordered;
	}

	private void ApplyAnimSets(DirectoryInfo root, string modcode, AnimSetDataManager manager)
	{
		var folders = new Dictionary<string, DirectoryInfo>(StringComparer.OrdinalIgnoreCase);
		foreach (DirectoryInfo folder in root.EnumerateDirectories())
		{
			folders[folder.Name] = folder;
		}

		var used = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		if (folders.TryGetValue("$header$", out DirectoryInfo? headerFolder))
		{
			FileInfo list = new(Path.Combine(headerFolder.FullName, "$header$.txt"));
			if (list.Exists)
			{
				List<string> projectList = ProjectKeys(File.ReadAllLines(list.FullName));
				foreach (string projectPath in projectList)
				{
					string folderName = SetFolderName(projectPath);
					folders.TryGetValue(folderName, out DirectoryInfo? folder);
					string mapKey = Path.GetFileNameWithoutExtension(projectPath);
					if (
						TryGetSetProject(manager, mapKey, out ProjectAnimSetData? existing)
						&& existing != null
					)
					{
						if (folder != null)
						{
							ApplyExistingSetProject(folder, existing, modcode);
							used.Add(folder.Name);
						}
						continue;
					}
					if (folder == null)
					{
						throw new InvalidDataException(
							$"Animation set project {projectPath} has no patch folder."
						);
					}
					manager.AddProject(projectPath, ReadNewSetProject(folder, modcode));
					used.Add(folder.Name);
				}
				manager.ReorderProjects(projectList);
			}
		}

		foreach (DirectoryInfo folder in folders.Values)
		{
			if (
				folder.Name.Equals("$header$", StringComparison.OrdinalIgnoreCase)
				|| used.Contains(folder.Name)
			)
			{
				continue;
			}
			if (
				TryGetSetProject(
					manager,
					SetProjectKey(folder.Name),
					out ProjectAnimSetData? project
				)
				&& project != null
			)
			{
				ApplyExistingSetProject(folder, project, modcode);
			}
		}
	}

	private static void ApplyExistingSetProject(
		DirectoryInfo folder,
		ProjectAnimSetData project,
		string modcode
	)
	{
		var consumed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		FileInfo header = new(Path.Combine(folder.FullName, "$header$.txt"));
		if (header.Exists)
		{
			var names = new List<string>();
			var sets = new List<IAnimSet>();
			foreach (string name in SetNames(File.ReadAllLines(header.FullName)))
			{
				FileInfo? setFile = ResolveSetFile(folder, name, modcode);
				IAnimSet set;
				if (setFile != null)
				{
					consumed.Add(setFile.Name);
					string[] lines = File.ReadAllLines(setFile.FullName);
					if (!IsSetDocument(lines))
					{
						if (!project.AnimSetsByName.TryGetValue(name, out IAnimSet? kept))
						{
							throw new InvalidDataException(
								$"Animation set {name} in {folder.Name} is not a versioned set."
							);
						}
						set = kept;
					}
					else
					{
						set = FileHasMarkers(setFile) ? ReadSet(lines) : ReadPlainSet(lines);
					}
				}
				else if (project.AnimSetsByName.TryGetValue(name, out IAnimSet? existing))
				{
					set = existing;
				}
				else
				{
					throw new InvalidDataException(
						$"Animation set {name} is listed for {folder.Name} but has no patch file."
					);
				}
				names.Add(name);
				sets.Add(set);
			}
			project.ReplaceAll(names, sets);
		}

		foreach (FileInfo file in folder.EnumerateFiles("*.txt"))
		{
			if (
				file.Name.Equals("$header$.txt", StringComparison.OrdinalIgnoreCase)
				|| consumed.Contains(file.Name)
				|| !FileHasMarkers(file)
			)
			{
				continue;
			}
			string[] lines = File.ReadAllLines(file.FullName);
			if (!IsSetDocument(lines))
			{
				continue;
			}
			AddOrReplaceSet(project, file.Name, lines);
		}
	}

	private static ProjectAnimSetData ReadNewSetProject(DirectoryInfo folder, string modcode)
	{
		FileInfo header = new(Path.Combine(folder.FullName, "$header$.txt"));
		List<string> names = SetNames(File.ReadAllLines(header.FullName));
		var sets = new List<IAnimSet>(names.Count);
		var map = new Dictionary<string, IAnimSet>(names.Count, StringComparer.OrdinalIgnoreCase);
		foreach (string name in names)
		{
			FileInfo? setFile =
				ResolveSetFile(folder, name, modcode)
				?? throw new InvalidDataException(
					$"New animation set {name} is missing from {folder.Name}."
				);
			AnimSet set = ReadPlainSet(File.ReadAllLines(setFile.FullName));
			sets.Add(set);
			map[name] = set;
		}
		return new ProjectAnimSetData(names.Count, names, sets, map);
	}

	private static List<string> SetNames(string[] raw)
	{
		List<string> lines = Splice(raw);
		if (
			lines.Count > 0
			&& int.TryParse(lines[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out _)
		)
		{
			lines.RemoveAt(0);
		}
		return lines;
	}

	private static List<string> ProjectKeys(string[] raw)
	{
		return Splice(raw)
			.Where(line =>
				line.Contains('\\')
				|| line.Contains('/')
				|| line.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
			)
			.ToList();
	}

	private static string SetFolderName(string projectPath)
	{
		string stem = projectPath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
			? projectPath[..^4]
			: projectPath;
		return stem.Replace('\\', '~').Replace('/', '~');
	}

	private static void AddOrReplaceSet(
		ProjectAnimSetData project,
		string listedName,
		string[] lines
	)
	{
		AnimSet parsed =
			lines.Any(IsOpenLine) || lines.Any(IsCloseLine) ? ReadSet(lines) : ReadPlainSet(lines);
		if (
			project.AnimSetsByName.TryGetValue(listedName, out IAnimSet? existing)
			&& existing is AnimSet set
		)
		{
			set.ReplaceContents(
				parsed.Triggers,
				parsed.Conditions,
				parsed.AttackEntries,
				parsed.AnimInfos
			);
			return;
		}
		project.AddSet(Path.GetFileName(listedName), parsed);
	}

	/// <summary>
	/// A Nemesis set file starts with <c>V3</c>. Some mods also ship a companion
	/// file whose first line names another set and whose body repeats that set's
	/// added checksums. That file is not a set.
	/// </summary>
	private static bool IsSetDocument(IReadOnlyList<string> lines)
	{
		foreach (string line in lines)
		{
			if (
				line.Trim().Length == 0
				|| IsOpenLine(line)
				|| IsCloseLine(line)
				|| IsOriginalLine(line)
			)
			{
				continue;
			}
			string trimmed = line.Trim();
			return trimmed.Length > 1
				&& (trimmed[0] == 'V' || trimmed[0] == 'v')
				&& char.IsDigit(trimmed[1]);
		}
		return false;
	}

	private static AnimSet ReadPlainSet(string[] lines)
	{
		using var stream = new StreamReader(
			new MemoryStream(System.Text.Encoding.UTF8.GetBytes(string.Join("\n", lines)))
		);
		if (!AnimSet.TryRead(stream, out IAnimSet? set) || set is not AnimSet animSet)
		{
			throw new InvalidDataException("Could not read an animation set patch.");
		}
		return animSet;
	}

	private static FileInfo? ResolveSetFile(DirectoryInfo folder, string listedName, string modcode)
	{
		FileInfo exact = new(Path.Combine(folder.FullName, listedName));
		if (exact.Exists)
		{
			return exact;
		}
		string stem = Path.GetFileNameWithoutExtension(listedName);
		FileInfo prefixed = new(Path.Combine(folder.FullName, $"{modcode}${stem}.txt"));
		return prefixed.Exists ? prefixed : null;
	}

	private static bool TryGetSetProject(
		AnimSetDataManager manager,
		string key,
		out ProjectAnimSetData? project
	)
	{
		project = null;
		if (
			manager.AnimSetDataMap.TryGetValue(key, out IProjectAnimSetData? found)
			&& found is ProjectAnimSetData typed
		)
		{
			project = typed;
			return true;
		}
		foreach (var pair in manager.AnimSetDataMap)
		{
			if (
				string.Equals(pair.Key, key, StringComparison.OrdinalIgnoreCase)
				&& pair.Value is ProjectAnimSetData match
			)
			{
				project = match;
				return true;
			}
		}
		return false;
	}

	private int BaseFor(string projectName, ProjectAnimData project)
	{
		if (_nextBase.TryGetValue(projectName, out int stored))
		{
			return stored;
		}
		return project.MaxNumericClipId() + 1;
	}

	private static void AppendCharacterNames(
		IProjectManager projects,
		string projectName,
		IReadOnlyList<string> names,
		out Dictionary<string, int> pathIndex
	)
	{
		pathIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
		IPackFileCharacter? character = CharacterOf(projects, projectName);
		if (character == null)
		{
			return;
		}
		int index = character.AnimationNames.Count;
		foreach (string name in names)
		{
			pathIndex[name] = index;
			if (character.AddUniqueAnimation(name))
			{
				index++;
			}
		}
	}

	private static void AssignBindingIndexes(
		IProjectManager projects,
		string projectName,
		Dictionary<string, int> pathIndex
	)
	{
		foreach (IPackFile pack in projects.ActivePackFiles)
		{
			if (
				pack.ParentProject == null
				|| !string.Equals(
					pack.ParentProject.Identifier,
					projectName,
					StringComparison.OrdinalIgnoreCase
				)
			)
			{
				continue;
			}
			foreach (XElement element in pack.XmlDeserializer.Context.ElementNameMap.Values)
			{
				if (
					!string.Equals(
						(string?)element.Attribute("class"),
						"hkbClipGenerator",
						StringComparison.Ordinal
					)
				)
				{
					continue;
				}
				XElement? binding = element
					.Elements("hkparam")
					.FirstOrDefault(param =>
						(string?)param.Attribute("name") == "animationBindingIndex"
					);
				XElement? animation = element
					.Elements("hkparam")
					.FirstOrDefault(param => (string?)param.Attribute("name") == "animationName");
				if (binding == null || animation == null || binding.Value.Trim() != "-1")
				{
					continue;
				}
				if (pathIndex.TryGetValue(animation.Value.Trim(), out int index))
				{
					binding.Value = index.ToString(CultureInfo.InvariantCulture);
				}
			}
		}
	}

	private static IPackFileCharacter? CharacterOf(IProjectManager projects, string projectName)
	{
		if (!projects.TryGetProject(projectName, out IProject? project))
		{
			projects.TryGetProject(projectName.ToLowerInvariant(), out project);
		}
		return project?.CharacterPackFile;
	}

	private static Dictionary<string, List<string>> ReadCharacterNames(DirectoryInfo modFolder)
	{
		var names = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
		foreach (FileInfo file in EnumerateBehaviorFiles(modFolder))
		{
			List<string> inserted = InsertedAnimationNames(File.ReadAllLines(file.FullName));
			if (inserted.Count == 0 || file.Directory == null)
			{
				continue;
			}
			// Each creature project has its own character behavior. DefaultMale,
			// DefaultFemale, and FirstPerson are the human and player projects.
			string project = file.Directory.Name;
			if (!names.TryGetValue(project, out List<string>? list))
			{
				list = [];
				names[project] = list;
			}
			list.AddRange(inserted);
		}
		return names;
	}

	private static IEnumerable<FileInfo> EnumerateBehaviorFiles(DirectoryInfo root)
	{
		foreach (FileInfo file in root.EnumerateFiles("*.txt"))
		{
			yield return file;
		}
		foreach (DirectoryInfo directory in root.EnumerateDirectories())
		{
			if (IsAnimDataDirectory(directory) || IsAnimSetDirectory(directory))
			{
				continue;
			}
			foreach (FileInfo file in EnumerateBehaviorFiles(directory))
			{
				yield return file;
			}
		}
	}

	private static List<string> InsertedAnimationNames(IReadOnlyList<string> lines)
	{
		int start = -1;
		int end = lines.Count;
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i].Contains("name=\"animationNames\"", StringComparison.Ordinal))
			{
				start = i;
				continue;
			}
			if (start >= 0 && lines[i].Contains("</hkparam>", StringComparison.Ordinal))
			{
				end = i;
				break;
			}
		}
		if (start < 0)
		{
			return [];
		}
		var span = new List<string>(end - start);
		for (int i = start; i < end; i++)
		{
			span.Add(lines[i]);
		}
		var names = new List<string>();
		var cursor = new LineCursor(span);
		while (!cursor.End)
		{
			if (!cursor.IsOpen())
			{
				cursor.Take();
				continue;
			}
			(List<string> edited, _) = cursor.TakeBlock();
			foreach (string line in edited)
			{
				string? path = AnimationPath(line);
				if (path != null)
				{
					names.Add(path);
				}
			}
		}
		return names;
	}

	private static string? AnimationPath(string line)
	{
		const string open = "<hkcstring>";
		const string close = "</hkcstring>";
		int start = line.IndexOf(open, StringComparison.OrdinalIgnoreCase);
		if (start < 0)
		{
			return null;
		}
		start += open.Length;
		int end = line.IndexOf(close, start, StringComparison.OrdinalIgnoreCase);
		return end < 0 ? null : line[start..end];
	}

	private static void ApplyProjectHeader(ProjectAnimData project, string[] lines)
	{
		var cursor = new LineCursor(lines);
		if (cursor.End)
		{
			return;
		}
		int hasFileList = int.Parse(cursor.Take(), CultureInfo.InvariantCulture);
		List<string> assets = ReadCounted(cursor);
		int hasCache = cursor.End
			? project.Header.HasMotionData
			: int.Parse(cursor.Take(), CultureInfo.InvariantCulture);
		if (project.Header is ProjectAnimDataHeader header)
		{
			header.LeadInt = hasFileList;
			header.ProjectAssets = assets;
			header.AssetCount = assets.Count;
			header.HasMotionData = hasCache;
		}
	}

	private static List<string> ReadCounted(LineCursor cursor)
	{
		int count = int.Parse(cursor.Take(), CultureInfo.InvariantCulture);
		var items = new List<string>();
		int consumed = 0;
		while (consumed < count || (!cursor.End && cursor.IsOpen()))
		{
			if (!cursor.End && cursor.IsOpen())
			{
				(List<string> edited, List<string> original) = cursor.TakeBlock();
				items.AddRange(edited);
				consumed += original.Count;
				continue;
			}
			if (consumed >= count || cursor.End)
			{
				break;
			}
			items.Add(cursor.Take());
			consumed++;
		}
		return items;
	}

	private static void CollectTokens(PatchFile file, string modcode, HashSet<int> tokens)
	{
		if (file.Info.Name.Equals("$header$.txt", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		string stem = Path.GetFileNameWithoutExtension(file.Info.Name);
		int split = stem.LastIndexOf('~');
		if (split >= 0 && TryToken(stem[(split + 1)..], modcode, out int fromName))
		{
			tokens.Add(fromName);
		}
		if (TryToken(stem, modcode, out int fromMotion))
		{
			tokens.Add(fromMotion);
		}
		foreach (string line in file.Lines)
		{
			if (TryToken(line.Trim(), modcode, out int fromLine))
			{
				tokens.Add(fromLine);
			}
		}
	}

	private static int TokenOrder(PatchFile file, string modcode)
	{
		string stem = Path.GetFileNameWithoutExtension(file.Info.Name);
		int split = stem.LastIndexOf('~');
		string code = split >= 0 ? stem[(split + 1)..] : stem;
		return TryToken(code, modcode, out int n) ? n : int.MaxValue;
	}

	private static string Resolve(string code, string modcode, int baseIndex)
	{
		return TryToken(code.Trim(), modcode, out int n)
			? (baseIndex + n).ToString(CultureInfo.InvariantCulture)
			: code.Trim();
	}

	private static bool TryToken(string text, string modcode, out int n)
	{
		n = 0;
		string prefix = modcode + "$";
		if (!text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return int.TryParse(
			text.AsSpan(prefix.Length),
			NumberStyles.Integer,
			CultureInfo.InvariantCulture,
			out n
		);
	}

	private static ClipDataBlock ClipFromLines(IReadOnlyList<string> lines)
	{
		int events = int.Parse(lines[5], CultureInfo.InvariantCulture);
		var triggers = new List<string>(events);
		for (int i = 0; i < events; i++)
		{
			triggers.Add(lines[6 + i]);
		}
		return new ClipDataBlock(
			lines[0],
			lines[1],
			float.Parse(lines[2], CultureInfo.InvariantCulture),
			float.Parse(lines[3], CultureInfo.InvariantCulture),
			float.Parse(lines[4], CultureInfo.InvariantCulture),
			events,
			triggers
		);
	}

	private static ClipMotionDataBlock MotionFromFile(
		PatchFile file,
		string resolvedId,
		string modcode,
		int baseIndex
	)
	{
		List<string> body = file.HasMarkers ? Splice(file.Lines) : file.Lines.ToList();
		body = TrimBlank(body);
		if (body.Count > 0)
		{
			body[0] = Resolve(body[0], modcode, baseIndex);
		}
		if (resolvedId.Length > 0 && body.Count > 0)
		{
			body[0] = resolvedId;
		}
		return MotionFromLines(body);
	}

	private static ClipMotionDataBlock MotionFromLines(IReadOnlyList<string> lines)
	{
		int cursor = 1;
		string durationLine = lines[cursor++];
		if (
			!float.TryParse(
				durationLine,
				NumberStyles.Float,
				CultureInfo.InvariantCulture,
				out float duration
			)
		)
		{
			duration = 0;
		}
		int translations = int.Parse(lines[cursor++], CultureInfo.InvariantCulture);
		var translationLines = new List<string>(translations);
		for (int i = 0; i < translations; i++)
		{
			translationLines.Add(lines[cursor++]);
		}
		int rotations = int.Parse(lines[cursor++], CultureInfo.InvariantCulture);
		var rotationLines = new List<string>(rotations);
		for (int i = 0; i < rotations; i++)
		{
			rotationLines.Add(lines[cursor++]);
		}
		return new ClipMotionDataBlock(lines[0], duration, translationLines, rotationLines);
	}

	private static List<string> TrimBlank(List<string> lines)
	{
		while (lines.Count > 0 && lines[^1].Length == 0)
		{
			lines.RemoveAt(lines.Count - 1);
		}
		return lines;
	}

	private static List<PatchFile> ReadPatchFiles(DirectoryInfo folder)
	{
		var files = new List<PatchFile>();
		foreach (FileInfo info in folder.EnumerateFiles("*.txt"))
		{
			if (info.Name.Equals("$order$.txt", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			string[] lines = File.ReadAllLines(info.FullName);
			files.Add(new PatchFile(info, lines, lines.Any(IsOpenLine) || lines.Any(IsCloseLine)));
		}
		return files;
	}

	private static bool FileHasMarkers(FileInfo file)
	{
		foreach (string line in File.ReadLines(file.FullName))
		{
			if (IsOpenLine(line) || IsCloseLine(line))
			{
				return true;
			}
		}
		return false;
	}

	private static string ProjectName(string folderName)
	{
		int split = folderName.IndexOf('~');
		return split < 0 ? folderName : folderName[..split];
	}

	private static string SetProjectKey(string folderName)
	{
		string[] parts = folderName.Split('~');
		return parts.Length >= 2 ? parts[^1] : parts[0];
	}

	private static IEnumerable<DirectoryInfo> FindCacheDirectories(DirectoryInfo root)
	{
		foreach (DirectoryInfo directory in root.EnumerateDirectories())
		{
			if (IsAnimSetDirectory(directory) || IsAnimDataDirectory(directory))
			{
				yield return directory;
			}
		}
	}

	private static bool IsAnimSetDirectory(DirectoryInfo directory) =>
		directory.Name.StartsWith("animationsetdata", StringComparison.OrdinalIgnoreCase);

	private static bool IsAnimDataDirectory(DirectoryInfo directory) =>
		directory.Name.StartsWith("animationdata", StringComparison.OrdinalIgnoreCase)
		&& !IsAnimSetDirectory(directory);

	private static List<string> Splice(IReadOnlyList<string> lines)
	{
		var cursor = new LineCursor(lines);
		var result = new List<string>();
		while (!cursor.End)
		{
			if (cursor.IsOpen())
			{
				(List<string> edited, _) = cursor.TakeBlock();
				result.AddRange(edited);
				continue;
			}
			string line = cursor.Take();
			if (!IsDelete(line))
			{
				result.Add(line);
			}
		}
		return result;
	}

	private static AnimSet ReadSet(IReadOnlyList<string> lines)
	{
		var cursor = new LineCursor(lines);
		string version = cursor.Take();
		List<string> triggers = ReadCounted(cursor);
		List<ISetCondition> conditions = ReadGrouped<ISetCondition>(
			cursor,
			3,
			group => new SetCondition(
				group[0],
				int.Parse(group[1], CultureInfo.InvariantCulture),
				int.Parse(group[2], CultureInfo.InvariantCulture)
			)
		);
		List<ISetAttackEntry> attacks = ReadAttacks(cursor);
		List<ISetCachedAnimInfo> infos = ReadGrouped<ISetCachedAnimInfo>(
			cursor,
			3,
			group => new SetCachedAnimInfo(
				uint.Parse(group[0], CultureInfo.InvariantCulture),
				uint.Parse(group[1], CultureInfo.InvariantCulture),
				uint.Parse(group[2], CultureInfo.InvariantCulture)
			)
		);
		return new AnimSet(
			version,
			triggers.Count,
			conditions.Count,
			attacks.Count,
			infos.Count,
			triggers,
			conditions,
			attacks,
			infos
		);
	}

	private static List<T> ReadGrouped<T>(
		LineCursor cursor,
		int width,
		Func<IReadOnlyList<string>, T> parse
	)
	{
		int count = int.Parse(cursor.Take(), CultureInfo.InvariantCulture);
		var items = new List<T>();
		int consumed = 0;
		while (consumed < count || (!cursor.End && cursor.IsOpen()))
		{
			if (!cursor.End && cursor.IsOpen())
			{
				(List<string> edited, List<string> original) = cursor.TakeBlock();
				items.AddRange(Chunks(edited, width).Select(parse));
				consumed += original.Count / width;
				continue;
			}
			if (consumed >= count || cursor.End)
			{
				break;
			}
			var group = new string[width];
			for (int i = 0; i < width; i++)
			{
				group[i] = cursor.Take();
			}
			items.Add(parse(group));
			consumed++;
		}
		return items;
	}

	private static List<ISetAttackEntry> ReadAttacks(LineCursor cursor)
	{
		int count = int.Parse(cursor.Take(), CultureInfo.InvariantCulture);
		var items = new List<ISetAttackEntry>();
		int consumed = 0;
		while (consumed < count || (!cursor.End && cursor.IsOpen()))
		{
			if (!cursor.End && cursor.IsOpen())
			{
				(List<string> edited, List<string> original) = cursor.TakeBlock();
				items.AddRange(ParseAttackLines(edited));
				consumed += ParseAttackLines(original).Count;
				continue;
			}
			if (consumed >= count || cursor.End)
			{
				break;
			}
			items.Add(ReadOneAttack(cursor));
			consumed++;
		}
		return items;
	}

	private static List<ISetAttackEntry> ParseAttackLines(List<string> lines)
	{
		var cursor = new LineCursor(lines);
		var attacks = new List<ISetAttackEntry>();
		while (!cursor.End)
		{
			attacks.Add(ReadOneAttack(cursor));
		}
		return attacks;
	}

	private static ISetAttackEntry ReadOneAttack(LineCursor cursor)
	{
		string trigger = cursor.Take();
		int flag = int.Parse(cursor.Take(), CultureInfo.InvariantCulture);
		int clipCount = int.Parse(cursor.Take(), CultureInfo.InvariantCulture);
		var clips = new string[clipCount];
		for (int i = 0; i < clipCount; i++)
		{
			clips[i] = cursor.Take();
		}
		return new SetAttackEntry(trigger, flag, clipCount, clips);
	}

	private static IEnumerable<IReadOnlyList<string>> Chunks(List<string> lines, int width)
	{
		for (int i = 0; i + width <= lines.Count; i += width)
		{
			var group = new string[width];
			for (int j = 0; j < width; j++)
			{
				group[j] = lines[i + j];
			}
			yield return group;
		}
	}

	private static bool IsOpenLine(string line)
	{
		string trimmed = line.Trim();
		return trimmed.StartsWith("<!--", StringComparison.Ordinal)
			&& trimmed.Contains("MOD_CODE", StringComparison.Ordinal)
			&& trimmed.Contains("OPEN", StringComparison.Ordinal);
	}

	private static bool IsCloseLine(string line)
	{
		string trimmed = line.Trim();
		return trimmed.StartsWith("<!--", StringComparison.Ordinal)
			&& trimmed.Contains("CLOSE", StringComparison.Ordinal)
			&& !trimmed.Contains("MOD_CODE", StringComparison.Ordinal);
	}

	private static bool IsOriginalLine(string line)
	{
		string trimmed = line.Trim();
		return trimmed.StartsWith("<!--", StringComparison.Ordinal)
			&& trimmed.Contains("ORIGINAL", StringComparison.Ordinal);
	}

	private static bool IsDelete(string line) =>
		string.Equals(line.Trim(), DeleteSentinel, StringComparison.Ordinal);

	private sealed class PatchFile
	{
		public PatchFile(FileInfo info, string[] lines, bool hasMarkers)
		{
			Info = info;
			Lines = lines;
			HasMarkers = hasMarkers;
		}

		public FileInfo Info { get; }
		public string[] Lines { get; }
		public bool HasMarkers { get; }

		public bool IsClip
		{
			get
			{
				if (Info.Name.Contains('~', StringComparison.Ordinal))
				{
					return true;
				}
				if (Lines.Length < 6 || HasMarkers)
				{
					return Info.Name.Contains('~', StringComparison.Ordinal);
				}
				return int.TryParse(
						Lines[5],
						NumberStyles.Integer,
						CultureInfo.InvariantCulture,
						out _
					)
					&& float.TryParse(
						Lines[2],
						NumberStyles.Float,
						CultureInfo.InvariantCulture,
						out _
					)
					&& float.TryParse(
						Lines[3],
						NumberStyles.Float,
						CultureInfo.InvariantCulture,
						out _
					)
					&& float.TryParse(
						Lines[4],
						NumberStyles.Float,
						CultureInfo.InvariantCulture,
						out _
					)
					&& !Lines[5].Contains(' ');
			}
		}
	}

	private sealed class LineCursor
	{
		private readonly IReadOnlyList<string> _lines;
		private int _index;

		public LineCursor(IReadOnlyList<string> lines)
		{
			_lines = lines;
		}

		public bool End => _index >= _lines.Count;

		public bool IsOpen() => !End && IsOpenLine(_lines[_index]);

		public string Take() => _lines[_index++];

		public (List<string> Edited, List<string> Original) TakeBlock()
		{
			Take();
			var edited = new List<string>();
			var original = new List<string>();
			bool inOriginal = false;
			while (!End && !IsCloseLine(_lines[_index]))
			{
				string line = Take();
				if (IsOriginalLine(line))
				{
					inOriginal = true;
					continue;
				}
				if (IsDelete(line))
				{
					continue;
				}
				if (inOriginal)
				{
					original.Add(line);
				}
				else
				{
					edited.Add(line);
				}
			}
			if (!End && IsCloseLine(_lines[_index]))
			{
				Take();
			}
			return (edited, original);
		}
	}
}
