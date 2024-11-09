using Pandora.Core.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using XmlCake.Linq;
using HKX2E;
using Pandora.Core;
using Pandora.API.Patch.Engine.Skyrim64; 

namespace Pandora.Patch.Patchers.Skyrim.Hkx;

public class PackFile : IEquatable<PackFile>, IPackFile
{

	public XMap Map { get; private set; }
	public HavokReferenceXmlDeserializer Deserializer { get; private set; } = new();
	public HavokXmlPartialDeserializer PartialDeserializer { get; private set; } = new();
	public HavokXmlPartialSerializer PartialSerializer { get; private set; } = new();
	public HavokXmlSerializer Serializer { get; private set; } = new();
	public hkRootLevelContainer Container { get; private set; }

	public static readonly string ROOT_CONTAINER_NAME = "__data__";

	public static readonly string ROOT_CONTAINER_INSERT_PATH = "__data__/top";


	public string Name { get; private set; }
	public FileInfo InputHandle { get; private set; }

	public FileInfo OutputHandle { get; private set; }

	private readonly string relativeOutputFilePath;
	public string RelativeOutputDirectoryPath { get; }

	public FileInfo RebaseOutput(DirectoryInfo exportDirectory)
	{
		OutputHandle = new FileInfo(Path.Join(exportDirectory.FullName, relativeOutputFilePath));
		return OutputHandle;
	}
	public FileInfo GetRebasedOutput(DirectoryInfo exportDirectory)
	{
		return new FileInfo(Path.Join(exportDirectory.FullName, relativeOutputFilePath));
	}

	public static bool DebugFiles { get; set; } = false;

	public PackFileEditor Editor { get; private set; } = new PackFileEditor();

	public XElement ContainerNode { get; private set; }

	public PackFileDispatcher Dispatcher { get; private set; } = new PackFileDispatcher();

	public bool ExportSuccess { get; private set; } = true;

	private static HashSet<FileInfo> exportedFiles = new HashSet<FileInfo>();

	protected readonly Dictionary<IHavokObject, XMapElement> objectElementMap = new(ReferenceEqualityComparer.Instance);

	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
	public void ApplyChanges() => Dispatcher.ApplyChanges(this);
	public IEnumerable<XElement> IndexedElements => objectElementMap.Values;

	public Project? ParentProject
	{
		get => parentProject;
		set
		{
			parentProject = value;
			UniqueName = $"{ParentProject?.Identifier}~{Name}";
		}
	}
	public IProject GetProject() => ParentProject as IProject; 

	protected ILookup<string, XElement>? classLookup = null;
	public int NodeCount => classLookup == null ? Map.NavigateTo(PackFile.ROOT_CONTAINER_NAME).Elements().Count() : classLookup.Count;

	private bool active = false;
	private Project? parentProject;


	public PackFile(FileInfo file, Project? project)
	{
		InputHandle = file;
		OutputHandle = file;
		relativeOutputFilePath = Path.GetRelativePath(BehaviourEngine.AssemblyDirectory.FullName, file.FullName.Replace("Pandora_Engine\\Skyrim\\Template", "meshes", StringComparison.OrdinalIgnoreCase));
		RelativeOutputDirectoryPath = Path.GetDirectoryName(relativeOutputFilePath)!;
		using (var stream = file.OpenRead())
		{
			Map = XMap.Load(stream);
		}
		using (var stream = file.OpenRead())
		{
			Container = (hkRootLevelContainer)Deserializer.Deserialize(stream, HKXHeader.SkyrimSE());
		}
		PartialSerializer.ShareContext(Deserializer.Context);
		PartialDeserializer.ShareContext(Deserializer.Context);
		Serializer.ShareContext(Deserializer.Context);
		ParentProject = project;
		ContainerNode = Map.NavigateTo(ROOT_CONTAINER_NAME);
		Name = Path.GetFileNameWithoutExtension(InputHandle.Name).ToLower();

		UniqueName = $"{ParentProject?.Identifier}~{Name}";
	}
	public PackFile(FileInfo file) : this(file, null)
	{

	}
	public virtual void Load()
	{

	}
	[MemberNotNull(nameof(classLookup))]
	public void BuildClassLookup()
	{
		//classLookup = Map.NavigateTo(PackFile.ROOT_CONTAINER_NAME).Elements().ToLookup(e => e.Attribute("class")!.Value);
	}

	protected bool CanActivate() => !active;

	/// <summary>
	/// This should only be called by the project manager.
	/// </summary>
	public virtual void Activate()
	{
		if (!CanActivate()) return;
		active = true;
	}
	public virtual void PopPriorityXmlAsObjects()
	{

	}


	public PackFile(string filePath) : this(new FileInfo(filePath)) { }

	public string UniqueName { get; private set; }
	public XElement SafeNavigateTo(string path) => Map.NavigateTo(path, ContainerNode);

	public bool PathExists(string nodeName, string path)
	{
		if (Deserializer.TryGetObject(nodeName, out var keyObject) && objectElementMap.TryGetValue(keyObject, out var xmap) && xmap.PathExists(path))
		{
			return true;
		}
		return false;
	}
	public bool TargetExists(string nodeName)
	{
		return Deserializer.TryGetObject(nodeName, out var keyObject) && objectElementMap.ContainsKey(keyObject);
	}
	public bool TryGetXMap(string nodeName, [NotNullWhen(true)] out XMapElement? xmap)
	{
		if (Deserializer.TryGetObject(nodeName, out var keyObject))
		{
			return objectElementMap.TryGetValue(keyObject, out xmap);
		}
		xmap = null;
		return false;
	}

	[MemberNotNull(nameof(classLookup))]
	public void TryBuildClassLookup() { if (classLookup == null) { BuildClassLookup(); } }
	public HashSet<string> UsedNodeNames => Map.NavigateTo(PackFile.ROOT_CONTAINER_NAME).Elements().Select(e => e.Attribute("name")!.Value).ToHashSet();
	public XElement GetFirstNodeOfClass(string className)
	{
		TryBuildClassLookup();

		return classLookup[className].First();
	}

	public bool PopObjectAsXml<T>(T node) where T : IHavokObject
	{
		if (objectElementMap.ContainsKey(node))
		{
			return false;
		}
		XMapElement mappedElement = new XMapElement(PartialSerializer.SerializeObject(node));
		mappedElement.MapSlice(mappedElement);
		objectElementMap.Add(node, mappedElement);
		return true;
	}
	public bool PopObjectAsXml(string nodeName)
	{
		if (Deserializer.TryGetObject(nodeName, out var havokObj))
		{
			return PopObjectAsXml(havokObj);
		}
		return false;
	}
	public bool PushXmlAsObject<T>(T targetObject) where T : IHavokObject
	{
		XMapElement? element;
		string? name;
		lock (objectElementMap)
		{
			if (!objectElementMap.TryGetValue(targetObject, out element) || !Serializer.TryGetName(targetObject, out name))
			{
				return false;
			}
		}
		try
		{
			var obj = PartialDeserializer.DeserializeRuntimeObjectOverwrite(element);
			Deserializer.UpdateDirectReference(targetObject, obj);
			Deserializer.UpdatePropertyReferences(name, obj);
			Deserializer.UpdateMapping(name, obj);
			lock (objectElementMap)
			{
				objectElementMap.Remove(targetObject);
			}
		}
		catch (Exception ex)
		{
			Logger.Error($"Packfile > Active Nodes > Push > FAILED > {ex.ToString()}");
		}
		return true;
	}
	public T GetPushXmlAsObject<T>(T targetObject) where T : class, IHavokObject
	{
		XMapElement? element;
		string? name;
		lock (objectElementMap)
		{
			if (!objectElementMap.TryGetValue(targetObject, out element) || !Serializer.TryGetName(targetObject, out name))
			{
				return targetObject;
			}
		}
		try
		{
			var obj = PartialDeserializer.DeserializeRuntimeObjectOverwrite(element);
			Deserializer.UpdateDirectReference(targetObject, obj);
			Deserializer.UpdatePropertyReferences(name, obj);
			Deserializer.UpdateMapping(name, obj);
			lock (objectElementMap)
			{
				objectElementMap.Remove(targetObject);
			}
		}
		catch (Exception ex)
		{
			Logger.Error($"Packfile > Active Nodes > Push > FAILED > {ex.ToString()}");
		}
		if (!Deserializer.TryGetObjectAs<T>(name, out var newObj))
		{
			return targetObject;
		}
		return newObj;
	}
	public T GetPushedObjectAs<T>(string name) where T : class, IHavokObject
	{
		return GetPushXmlAsObject<T>(Deserializer.GetObjectAs<T>(name));
	}
	public virtual void ApplyPriorityChanges(PackFileDispatcher dispatcher)
	{

	}
	public virtual void PushPriorityObjects()
	{

	}
	public virtual void PushXmlAsObjects()
	{
		foreach (var kvp in objectElementMap.OrderBy(kvp => Deserializer.GetOrder(Serializer.GetName(kvp.Key))))
		{
			IHavokObject? obj = null;
			try
			{
				obj = PartialDeserializer.DeserializeRuntimeObjectOverwrite(kvp.Value);
			}
			catch (Exception ex)
			{
				Logger.Error($"Packfile > Active Nodes > Push > FAILED > {ex.ToString()}");
				continue;
			}
			string name = Serializer.GetName(kvp.Key);
			Deserializer.UpdateDirectReference(kvp.Key, obj);
			Deserializer.UpdatePropertyReferences(name, obj);
			Deserializer.UpdateMapping(name, obj);

		}
		objectElementMap.Clear();
	}
	public bool Equals(PackFile? other)
	{
		return other != null && other.InputHandle.FullName.Equals(this.InputHandle.FullName, StringComparison.OrdinalIgnoreCase);
	}
	public override int GetHashCode()
	{
		return InputHandle.FullName.GetHashCode(StringComparison.OrdinalIgnoreCase);
	}

	public override bool Equals(object? obj)
	{
		if (obj == null || !(obj is PackFile)) return false;

		return Equals((PackFile)obj);
	}
}
