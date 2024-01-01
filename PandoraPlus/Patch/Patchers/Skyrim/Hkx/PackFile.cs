using HKX2;
using Pandora.Core.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using XmlCake.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;

public class PackFile : IPatchFile, IEquatable<PackFile>
{
	public XMap Map { get; private set; }

	public static readonly string ROOT_CONTAINER_NAME = "__data__";

	public static readonly string ROOT_CONTAINER_INSERT_PATH = "__data__/top";

	private static HashSet<FileInfo> exportedFiles = new HashSet<FileInfo>();

	public string Name { get; private set; }
	public FileInfo InputHandle { get; private set; }

	public FileInfo OutputHandle { get; private set; }

	public static bool DebugFiles { get; set; } = false;

	public PackFileEditor Editor { get; private set; } = new PackFileEditor();

	public XElement ContainerNode { get; private set; }

	public PackFileDispatcher Dispatcher { get; private set; } = new PackFileDispatcher();

	public bool ExportSuccess { get; private set; } = true;

	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	public void ApplyChanges() => Dispatcher.ApplyChanges(this);

	private HashSet<string> mappedNodeNames = new HashSet<string>();

	public Project? ParentProject { get => parentProject; 
		set { 
			parentProject = value;
			UniqueName = $"{ParentProject?.Identifier}~{Name}";
		} }


	protected ILookup<string, XElement>? classLookup = null;

	private bool active = false;
	private Project? parentProject;

	public PackFile(FileInfo file)
	{

		InputHandle = file;
		OutputHandle = new FileInfo(file.FullName.Replace("Template", "meshes").Replace("\\Pandora_Engine\\Skyrim", ""));
		using (var stream = file.OpenRead())
		{
			Map = XMap.Load(stream);
		}

		ContainerNode = Map.NavigateTo(ROOT_CONTAINER_NAME);
		Name = Path.GetFileNameWithoutExtension(InputHandle.Name).ToLower();

		UniqueName = Name;
//#if DEBUG
//		Debug.WriteLine($"- {UniqueName}");
//#endif
	}
	public PackFile(FileInfo file, Project project)
	{
		InputHandle = file;
		OutputHandle = new FileInfo(file.FullName.Replace("Template", "meshes").Replace("\\Pandora_Engine\\Skyrim", ""));
		using (var stream = file.OpenRead())
		{
			Map = XMap.Load(stream);
		}
		ParentProject = project;
		ContainerNode = Map.NavigateTo(ROOT_CONTAINER_NAME);
		Name = Path.GetFileNameWithoutExtension(InputHandle.Name).ToLower();

		UniqueName = $"{ParentProject?.Identifier}~{Name}";
//#if DEBUG
//		Debug.WriteLine(UniqueName);
//#endif
	}

	[MemberNotNull(nameof(classLookup))]
	public void BuildClassLookup()
	{
		classLookup = Map.NavigateTo(PackFile.ROOT_CONTAINER_NAME).Elements().ToLookup(e => e.Attribute("class")!.Value);
	}

	protected bool CanActivate() => !active;
	public virtual void Activate()
	{
		if (!CanActivate()) return;
		Map.MapLayer(PackFile.ROOT_CONTAINER_NAME, true);
		active = true; 
	}

	
	public PackFile(string filePath) : this(new FileInfo(filePath)) { }

	public string UniqueName { get; private set; }
	public XElement SafeNavigateTo(string path) => Map.NavigateTo(path, ContainerNode);

	[MemberNotNull(nameof(classLookup))]
	public void TryBuildClassLookup() {  if (classLookup == null) {  BuildClassLookup(); } }
	public XElement GetFirstNodeOfClass(string className)
	{
		TryBuildClassLookup();

		return classLookup[className].First();
	}

	public void DeleteExistingOutput()
	{
		if (OutputHandle.Exists)
		{
			OutputHandle.Delete();
		}
#if DEBUG
		var debugOuputHandle = new FileInfo(OutputHandle.FullName + ".xml");
		if (debugOuputHandle.Exists) { debugOuputHandle.Delete(); }

		debugOuputHandle = new FileInfo(debugOuputHandle.DirectoryName + "\\m_" + debugOuputHandle.Name);
		if (debugOuputHandle.Exists) { debugOuputHandle.Delete();  }
#endif
	}
	
	public void MapNode(string nodeName)
	{	
		lock(mappedNodeNames)
		{
			if (mappedNodeNames.Contains(nodeName)) return;
			mappedNodeNames.Add(nodeName);
		}
		lock(Map)
		{
			Map.MapSlice(nodeName);
		}
	}

	public bool Export()
	{


			if (OutputHandle.Directory == null) return false;
			if (!OutputHandle.Directory.Exists) { OutputHandle.Directory.Create(); }
			if (OutputHandle.Exists) { OutputHandle.Delete(); }
			HKXHeader header = HKXHeader.SkyrimSE();
			IHavokObject rootObject;
			ExportSuccess = true;
#if DEBUG
		Debug.WriteLine($"Export: {OutputHandle.FullName}");
		using (var memoryStream = new MemoryStream())
		{
			Map.Save(memoryStream);
			memoryStream.Position = 0;
			var deserializer = new XmlDeserializer();
			rootObject = deserializer.Deserialize(memoryStream, header, false);
		}
		using (var writeStream = OutputHandle.Create())
		{
			var binaryWriter = new BinaryWriterEx(writeStream);
			var serializer = new PackFileSerializer();
			serializer.Serialize(rootObject, binaryWriter, header);
		}


		var debugOuputHandle = new FileInfo(OutputHandle.FullName + ".xml");

		using (var writeStream = debugOuputHandle.Create())
		{

			var xmlSerializer = new HKX2.XmlSerializer();
			xmlSerializer.Serialize(rootObject, header, writeStream);
		}
		debugOuputHandle = new FileInfo(debugOuputHandle.DirectoryName + "\\m_" + debugOuputHandle.Name);

		using (var writeStream = debugOuputHandle.Create())
		{
			Map.Save(writeStream);
		}

#else
		try
		{
		using (var memoryStream = new MemoryStream())
		{
			Map.Save(memoryStream);
			memoryStream.Position = 0;
			var deserializer = new XmlDeserializer();
			rootObject = deserializer.Deserialize(memoryStream, header, false);
		}
		using (var writeStream = OutputHandle.Create())
		{
			var binaryWriter = new BinaryWriterEx(writeStream);
			var serializer = new PackFileSerializer();
			serializer.Serialize(rootObject, binaryWriter, header);
		}
		}
		catch(Exception ex) 
		{
			Logger.Fatal($"Export > {ParentProject?.Identifier}~{Name} > FAILED > {ex.Message}");
			using (var writeStream = OutputHandle.Create())
			{
				Map.Save(writeStream);
			}
			ExportSuccess = false;
		}
#endif

		return ExportSuccess;
	}
	public static void Unpack(FileInfo inputHandle)
	{
		HKXHeader header = HKXHeader.SkyrimSE();
		IHavokObject rootObject;
		using (var readStream = inputHandle.OpenRead())
		{
			var deserializer = new XmlDeserializer();
			rootObject = deserializer.Deserialize(readStream, header, false);
		}
		using (var writeStream = inputHandle.Create())
		{
			var binaryWriter = new BinaryWriterEx(writeStream);
			var serializer = new PackFileSerializer();
			serializer.Serialize(rootObject, binaryWriter, header);
		}
		var outputHandle = new FileInfo(Path.ChangeExtension(inputHandle.FullName, ".xml"));
		if (outputHandle.Exists) { outputHandle.Delete(); }
		using (var writeStream = outputHandle.Create())
		{

			var xmlSerializer = new HKX2.XmlSerializer();
			xmlSerializer.Serialize(rootObject, header, writeStream);
		}
	}
	public static FileInfo GetUnpackedHandle(FileInfo inputHandle)
	{
		HKXHeader header = HKXHeader.SkyrimSE();
		IHavokObject rootObject;
		using (var readStream = inputHandle.OpenRead())
		{
			var deserializer = new PackFileDeserializer();
			var binaryReaderEx = new BinaryReaderEx(readStream);
			rootObject = deserializer.Deserialize(binaryReaderEx);
		}

		var outputHandle = new FileInfo(Path.ChangeExtension(inputHandle.FullName, ".xml"));
		if (outputHandle.Exists) { outputHandle.Delete(); }
		using (var writeStream = outputHandle.Create())
		{

			var xmlSerializer = new HKX2.XmlSerializer();
			xmlSerializer.Serialize(rootObject, header, writeStream);
		}
		

		return outputHandle;
	}

	public bool Equals(PackFile? other)
	{
		return other != null && other.OutputHandle.FullName.Equals(this.OutputHandle.FullName, StringComparison.OrdinalIgnoreCase);
	}
	public override int GetHashCode()
	{
		return OutputHandle.FullName.GetHashCode(StringComparison.OrdinalIgnoreCase);
	}

	public override bool Equals(object? obj)
	{
		if (obj == null || ! (obj is PackFile)) return false;

		return Equals((PackFile)obj);
	}
}
