// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using HKX2E;
using Nito.HashAlgorithms;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using Pandora.Paths.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;

#region OAR Configuration

// NOTE: OAR can read both LF and CRLF correctly, but if the case is different, it will not be read correctly. e.g., GraphVariable <- Invalid

internal record OARNamespaceConfig
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = "";
	[JsonPropertyName("description")]
	public string Description { get; set; } = "";
	[JsonPropertyName("author")]
	public string Author { get; set; } = "";
}

internal record OARConfig
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = "";
	[JsonPropertyName("description")]
	public string Description { get; set; } = "";
	[JsonPropertyName("priority")]
	public int Priority { get; set; } = int.MaxValue;

	[JsonPropertyName("conditions")]
	public List<OARCondition> Conditions { get; set; } = [];
}

internal record OARCondition
{
	[JsonPropertyName("condition")]
	public string Condition { get; set; } = "CompareValues";

	[JsonPropertyName("requiredVersion")]
	public string RequiredVersion { get; set; } = "1.0.0.0";

	[JsonPropertyName("Value A")]
	public OARValueA ValueA { get; set; } = new();

	public string Comparison { get; set; } = "==";

	[JsonPropertyName("Value B")]
	public OARValueB ValueB { get; set; } = new();
}

internal record OARValueA
{
	[JsonPropertyName("graphVariable")]
	public string GraphVariable { get; set; } = "";
	[JsonPropertyName("graphVariableType")]
	public string GraphVariableType { get; set; } = "Int";
}

internal record OARValueB
{
	[JsonPropertyName("value")]
	public int Value { get; set; }
}

#endregion

#region Dyn FNIS_AA Configuration

internal record DynFNISAaConfig
{
	[JsonPropertyName("crc")]
	public uint Crc { get; set; }

	[JsonPropertyName("fnis_version")]
	public string FnisVersion { get; set; } = "V07.06.00.0";
	[JsonPropertyName("fnis_creature_version")]
	public string FnisCreatureVersion { get; set; } = "V07.06.00.0";

	[JsonPropertyName("mods")]
	public List<DynFNISAaMod> Mods { get; set; } = new();

	public DynFNISAaConfig(List<DynFNISAaMod> mods, string fnisVersion = "V07.06.00.0")
	{
		Mods = mods;
		FnisVersion = fnisVersion;
		Crc = CalculateCrc();
	}

	private uint CalculateCrc()
	{
		var crc32 = new CRC32();
		var sb = new StringBuilder();

		sb.Append(FnisVersion);

		foreach (var mod in Mods)
		{
			sb.Append(mod.Prefix);
			sb.Append(mod.Name);
			sb.Append(mod.ModId);

			foreach (var group in mod.Groups)
			{
				sb.Append(group.Name);
				sb.Append(group.Base);
			}
		}

		var bytes = Encoding.UTF8.GetBytes(sb.ToString());
		var hash = crc32.ComputeHash(bytes);

		return BitConverter.ToUInt32(hash, 0);
	}
}

internal record DynFNISAaMod
{
	[JsonPropertyName("prefix")]
	public string Prefix { get; set; } = "";
	[JsonPropertyName("name")]
	public string Name { get; set; } = "";
	[JsonPropertyName("mod_id")]
	public int ModId { get; set; }
	[JsonPropertyName("groups")]
	public List<DynFNISAaGroup> Groups { get; set; } = new();
}

internal record DynFNISAaGroup
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = "";
	/// <summary>
	/// As FNIS is already set to 0, we will start from 1.
	/// </summary>
	[JsonPropertyName("base")]
	public int Base { get; set; }
}

#endregion

#region Group Definition

internal record AAGroupDefinition
{
	public int Id { get; set; }
	public int Count { get; set; }
	public List<string> Animations { get; set; } = [];
}

#endregion

public class AltAnimToOarBuilder
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
	private readonly ConcurrentBag<AlternateAnimation> _alternateAnimations;
	private readonly IEnginePathsFacade _pathContext;

	private readonly Dictionary<(string prefix, string group), int> _baseMap = new();

	private readonly FrozenDictionary<string, AAGroupDefinition> _groupDefs;

	private readonly string _outputRoot;

	public AltAnimToOarBuilder(ConcurrentBag<AlternateAnimation> alternateAnimations, IEnginePathsFacade pathContext)
	{
		_alternateAnimations = alternateAnimations;
		_pathContext = pathContext;

		BuildBaseMap();
		_groupDefs = LoadGroupDefinitions();
		_outputRoot = ResolveOutputRoot();
	}

	/// <summary>
	/// This assumes that the function is called only once per patch, and that all `FNIS_AA` mods are passed `project` beforehand.
	/// </summary>
	public void Build()
	{
		foreach (var anim in _alternateAnimations)
		{
			var prefix = anim.Prefix; // e.g., `xpe`

			string modName = new DirectoryInfo(anim.AnimRoot).Name; // e.g. `meshes/actors/character/animations/XPMSE` -> `XPMSE`

			foreach (var set in anim.Sets)
			{
				var group = set.Group;
				_logger.Debug($"FNISAA to OAR Builder > current: (group={group}, prefix={prefix})");

				if (!_groupDefs.TryGetValue(group, out var groupDef))
				{
					_logger.Warn($"FNISAA to OAR Builder > Missing {group} in GroupDef (prefix={prefix}) > SKIPPED");
					continue;
				}

				if (!_baseMap.TryGetValue((prefix, group), out var baseValue))
				{
					_logger.Warn($"FNISAA to OAR Builder > Missing key(prefix={prefix}, group={group}) in BaseMap > SKIPPED");
					continue;
				}

				var outNamespaceDir = Path.Combine(_outputRoot, modName);  // e.g., `.../OpenAnimationReplacer/XPMSE`
				Directory.CreateDirectory(outNamespaceDir);
				WriteNamespaceConfig(outNamespaceDir, modName);

				Parallel.ForEach(
					Enumerable.Range(0, set.Slots),
					slot =>
					{
						var dirName = $"{prefix}{group}_{slot + 1}"; // e.g., `xpe_mt_1`
						var outDir = Path.Combine(outNamespaceDir, dirName);

						Directory.CreateDirectory(outDir);

						CopyAnimations(anim.AnimRoot, prefix, slot, groupDef, outDir);
						WriteConfig(group, slot, baseValue, dirName, outDir);
					});
			}
		}

		WriteDynFnisAAConfig();
	}

	private void BuildBaseMap()
	{
		// To be honest, as long as the valid range for the `FNISaa<group_name>` value does not overlap
		// with the slot ranges of other prefix + group combinations, any value is acceptable.
		var prefixOrder = new List<string>();
		var groupOrder = new Dictionary<string, Dictionary<string, int>>();

		foreach (var aa in _alternateAnimations)
		{
			if (!prefixOrder.Contains(aa.Prefix))
				prefixOrder.Add(aa.Prefix);

			foreach (var set in aa.Sets)
			{
				if (!groupOrder.TryGetValue(set.Group, out var dict))
				{
					dict = new Dictionary<string, int>();
					groupOrder[set.Group] = dict;
				}

				dict.TryAdd(aa.Prefix, set.Slots);
			}
		}

		foreach (var (group, prefixMap) in groupOrder)
		{
			int currentBase = 1;

			foreach (var prefix in prefixOrder)
			{
				if (!prefixMap.ContainsKey(prefix))
					continue;

				_baseMap[(prefix, group)] = currentBase;
				currentBase += prefixMap[prefix];
			}
		}

		if (_logger.IsDebugEnabled)
		{
			// Build a Rust-like `{:#?}` pretty debug dump
			var dump = new StringBuilder();
			dump.AppendLine("BaseMap {");
			foreach (var kv in _baseMap)
			{
				var (prefix, group) = kv.Key;
				var baseValue = kv.Value;

				dump.AppendLine($"    (\"{group}\", \"{prefix}\"): {baseValue},");
			}
			dump.Append("}");

			_logger.Debug(dump.ToString());
		}
	}

	private void CopyAnimations(string animRoot, string prefix, int slot, AAGroupDefinition groupDef, string outDir)
	{
		foreach (var animPath in groupDef.Animations)
		{
			// Address the following Path diff.
			// - table: male/mt_runforward.hkx
			// - mod: animations/FNISSexyMove/fsm0_mt_runforward.hkx <- need search this.
			var fileName = Path.GetFileName(animPath);
			var subDir = Path.GetDirectoryName(animPath) ?? "";

			var isMale = string.Equals(
				Path.GetFileName(subDir),
				"male",
				StringComparison.OrdinalIgnoreCase
			);

			// Remove `male/`
			var inputSubDir = isMale ? Path.GetDirectoryName(subDir) ?? "" : subDir;

			var inputPath = Path.Combine(
				_pathContext.GameDataFolder.FullName,
				"meshes/actors/character",
				animRoot, // e.g., `animations/FNISSexyMove`
				inputSubDir,
				$"{prefix}{slot}_{fileName}"
			);

			var outputPath = Path.Combine(outDir, animPath);

			CopyIfExists(inputPath, outputPath);

			if (isMale)
			{
				var femaleSubDir = Path.Combine(
					Path.GetDirectoryName(subDir) ?? "",
					"female"
				);

				var femaleOutputPath = Path.Combine(
					outDir,
					femaleSubDir,
					fileName
				);

				CopyIfExists(inputPath, femaleOutputPath);
			}
		}
	}

	private void CopyIfExists(string inputPath, string outputPath)
	{
		var dir = Path.GetDirectoryName(outputPath);
		if (!string.IsNullOrEmpty(dir))
		{
			Directory.CreateDirectory(dir);
		}

		// The animations registered in the FNIS_AA group table may not always be present.
		// (That is why we have not classified this as a warning or error.)
		if (!File.Exists(inputPath))
		{
			if (_logger.IsDebugEnabled)
			{
				var relative = Path.GetRelativePath(_pathContext.GameDataFolder.FullName, inputPath).Replace('\\', '/');
				_logger.Info($"FNISAA to OAR Builder > Missing animation file > {relative}> SKIPPED");
			}
			return;
		}

		File.Copy(inputPath, outputPath, true);
	}

	private void WriteNamespaceConfig(string outDir, string modId)
	{
		var config = new OARNamespaceConfig { Name = modId };

		var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
		{
			WriteIndented = true
		});

		if (json != null) File.WriteAllText(Path.Combine(outDir, "config.json"), json);
	}

	private void WriteConfig(string group, int slot, int baseValue, string dirName, string outDir)
	{
		var config = new OARConfig
		{
			Name = dirName,
			Description = $"base({baseValue}) + slot({slot})",
			Conditions =
			[
				new OARCondition
			{
				ValueA = new OARValueA
				{
					GraphVariable = $"FNISaa{group}" // e.g., `FNISaa_mt`
				},
				ValueB = new OARValueB
				{
					Value = baseValue + slot
				}
			}
			]
		};

		var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
		{
			WriteIndented = true
		});

		if (json != null) File.WriteAllText(Path.Combine(outDir, "config.json"), json);
	}

	/// <summary>
	/// Write fnis_aa.dll config.json.
	/// </summary>
	private void WriteDynFnisAAConfig()
	{
		var mods = _alternateAnimations
			.GroupBy(a => a.Prefix)
			.Select((group, modIndex) => new DynFNISAaMod
			{
				Prefix = group.Key,
				Name = group.Key,
				ModId = modIndex,

				Groups = group
					.SelectMany(a => a.Sets)
					.GroupBy(s => s.Group)
					.Select(g => new DynFNISAaGroup
					{
						Name = g.Key,
						Base = _baseMap.TryGetValue((group.Key, g.Key), out var b)
							? b
							: 1
					})
					.ToList()
			})
			.ToList();

		var config = new DynFNISAaConfig(mods);
		var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
		{
			WriteIndented = true
		});

		var path = Path.Combine(_pathContext.OutputFolder.FullName, "SKSE", "Plugins", "fnis_aa", "config.json");

		Directory.CreateDirectory(Path.GetDirectoryName(path)!);
		File.WriteAllText(path, json, new UTF8Encoding(false)); // no-BOM utf8
	}

	/// TODO: Change to compile time hash table?
	private FrozenDictionary<string, AAGroupDefinition> LoadGroupDefinitions()
	{
		string path = Path.Combine(_pathContext.TemplateFolder.FullName, "Alternate_AnimationGroups.json");

		if (!File.Exists(path))
		{
			return FrozenDictionary<string, AAGroupDefinition>.Empty;
		}

		string json = File.ReadAllText(path);

		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};

		var result = JsonSerializer.Deserialize<Dictionary<string, AAGroupDefinition>>(json, options);

		return result?.ToFrozenDictionary() ?? FrozenDictionary<string, AAGroupDefinition>.Empty;
	}

	private string ResolveOutputRoot()
	{
		return Path.Combine(_pathContext.OutputMeshesFolder.FullName, "actors/character/animations/OpenAnimationReplacer");
	}

	/// <summary>
	/// Push AA-related graph variables(e.g., `FNISaa_mt`) to `0_master`(3rd_person) packfile, so that fnis_aa.dll can read them.
	/// </summary> <summary>
	///
	/// </summary>
	public void PushAAVars(IProjectManager projectManager)
	{
		if (!projectManager.TryLookupPackFile("0_master", out var packFile)
			|| packFile is not PackFileGraph graph)
		{
			_logger.Warn("FNISAA to OAR Builder > PushAAVars > 0_master not found > FAILED");
			return;
		}

		var stringData = graph.GetPushedObjectAs<hkbBehaviorGraphStringData>("#0106");
		var valueSet = graph.GetPushedObjectAs<hkbVariableValueSet>("#0107");
		var graphData = graph.GetPushedObjectAs<hkbBehaviorGraphData>("#0108");

		if (stringData == null || valueSet == null || graphData == null)
		{
			_logger.Warn("FNISAA to OAR Builder > PushAAVars > graph data(0_master) missing > FAILED");
			return;
		}

		var names = stringData.variableNames;
		var values = valueSet.wordVariableValues;
		var infos = graphData.variableInfos;

		var groups = _alternateAnimations
			.SelectMany(a => a.Sets)
			.Select(s => s.Group)
			.Where(g => !string.IsNullOrEmpty(g))
			.Distinct()
			.OrderBy(g => g)
			.ToList();

		// FIXME?: Is it valid to add only the values that are used?(Do FNIS scripts assume that all variables are added?)
		foreach (var group in groups)
		{
			AddVar(names, values, infos, $"FNISaa{group}");
			AddVar(names, values, infos, $"FNISaa{group}_crc");
		}
		AddVar(names, values, infos, "FNISaa_crc");

		if (_logger.IsDebugEnabled)
		{
			_logger.Info($"FNISAA > PushAAVars > Added {groups.Count * 2 + 1} vars");
		}
	}

	private static void AddVar(
		IList<string> names,
		IList<hkbVariableValue> values,
		IList<hkbVariableInfo> infos,
		string name
	)
	{
		if (names.Contains(name))
			return;

		lock (names)
		{
			names.Add(name);
		}
		lock (values)
		{
			values.Add(new hkbVariableValue { value = 0 });
		}

		lock (infos)
		{
			infos.Add(new hkbVariableInfo
			{
				type = (sbyte)VariableType.VARIABLE_TYPE_INT32,
				role = new hkbRoleAttribute
				{
					role = (short)Role.ROLE_DEFAULT,
					flags = 0
				}
			});
		}
	}
}