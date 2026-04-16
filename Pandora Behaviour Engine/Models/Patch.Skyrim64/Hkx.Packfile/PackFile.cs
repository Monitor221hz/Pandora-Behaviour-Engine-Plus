// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using HKX2E;
using HKX2E.Mapper;
using Pandora.API.Patch.Skyrim64;
using XmlCake.Linq;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public class PackFile : IEquatable<PackFile>, IPackFile
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    public MetaPackFileDeserializer BinaryDeserializer { get; private set; } = new();
    public HavokXmlDeserializer XmlDeserializer { get; private set; } = new();
    public HavokXmlMetaDataSerializer Serializer { get; private set; } = new();
    public hkRootLevelContainer Container { get; private set; }

    public static readonly string RootContainerName = "__data__";

    public static readonly string RootContainerInsertPath = "__data__/top";

    private static readonly string TemplateRootPath = Path.Combine(
        Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)!,
        "Pandora_Engine",
        "Skyrim",
        "Template"
    );

    public string Name { get; private set; }
    public FileInfo InputHandle { get; private set; }

    public FileInfo OutputHandle { get; private set; }

    private readonly string _relativeOutputFilePath;
    public string RelativeOutputDirectoryPath { get; }

    public FileInfo RebaseOutput(DirectoryInfo exportDirectory)
    {
        OutputHandle = new FileInfo(Path.Join(exportDirectory.FullName, _relativeOutputFilePath));
        return OutputHandle;
    }

    public FileInfo GetOutputHandle(DirectoryInfo exportDirectory)
    {
        return new FileInfo(Path.Join(exportDirectory.FullName, _relativeOutputFilePath));
    }

    public static bool DebugFiles { get; set; } = false;


    public IPackFileDispatcher Dispatcher { get; private set; } = new PackFileDispatcher();

    public bool ExportSuccess { get; private set; } = true;

    private static HashSet<FileInfo> _exportedFiles = [];

    protected readonly Dictionary<IHavokObject, XMapElement> ObjectElementMap = new(
        ReferenceEqualityComparer.Instance
    );

    public void ApplyChanges() => Dispatcher.ApplyChanges(this);

    public IEnumerable<XElement> IndexedElements => ObjectElementMap.Values;

    public IProject? ParentProject
    {
        get => _parentProject;
        set
        {
            _parentProject = value;
            UniqueName = $"{ParentProject?.Identifier}~{Name}";
        }
    }
    protected ILookup<string, XElement>? ClassLookup = null;
    public int NodeCount => ClassLookup == null ? 0 : ClassLookup.Count;

    private bool _active = false;
    private IProject? _parentProject;

    public PackFile(FileInfo file, IProject? project)
    {
        InputHandle = file;
        OutputHandle = file;

        var relativePathInsideTemplate = Path.GetRelativePath(TemplateRootPath, file.FullName);

        _relativeOutputFilePath = Path.Combine("meshes", relativePathInsideTemplate);
        RelativeOutputDirectoryPath = Path.GetDirectoryName(_relativeOutputFilePath)!;

        using (var stream = file.OpenRead())
        {
            var reader = new BinaryReaderEx(stream);
            Container = (hkRootLevelContainer)BinaryDeserializer.Deserialize(reader, true);
        }
        var metaContext = BinaryDeserializer.Context;
        var deserializerContext = (HavokXmlDeserializerContext)metaContext;

        XmlDeserializer.ShareContext(deserializerContext);
        Serializer.ShareContext(metaContext);
        Serializer.ShareContext(deserializerContext);
        ParentProject = project;
        Name = Path.GetFileNameWithoutExtension(InputHandle.Name).ToLower();

        UniqueName = $"{ParentProject?.Identifier}~{Name}";
    }

    public PackFile(FileInfo file)
        : this(file, null) { }

    public virtual void Load() { }

    [MemberNotNull(nameof(ClassLookup))]
    public void BuildClassLookup()
    {
        //classLookup = Map.NavigateTo(PackFile.ROOT_CONTAINER_NAME).Elements().ToLookup(e => e.Attribute("class")!.Value);
    }

    protected bool CanActivate() => !_active;

    /// <summary>
    /// This should only be called by the project manager.
    /// </summary>
    public virtual void Activate()
    {
        if (!CanActivate())
            return;
        _active = true;
    }

    public virtual void PopPriorityXmlAsObjects() { }

    public PackFile(string filePath)
        : this(new FileInfo(filePath)) { }

    public string UniqueName { get; private set; }

    public bool PathExists(string nodeName, string path)
    {
        if (
            XmlDeserializer.TryGetObject(nodeName, out var keyObject)
            && ObjectElementMap.TryGetValue(keyObject, out var xmap)
            && xmap.PathExists(path)
        )
        {
            return true;
        }
        return false;
    }

    public bool TargetExists(string nodeName)
    {
        return XmlDeserializer.TryGetObject(nodeName, out var keyObject)
            && ObjectElementMap.ContainsKey(keyObject);
    }

    public bool TryGetXMap(string nodeName, [NotNullWhen(true)] out XMapElement? xmap)
    {
        if (XmlDeserializer.TryGetObject(nodeName, out var keyObject))
        {
            return ObjectElementMap.TryGetValue(keyObject, out xmap);
        }
        xmap = null;
        return false;
    }

    [MemberNotNull(nameof(ClassLookup))]
    public void TryBuildClassLookup()
    {
        if (ClassLookup == null)
        {
            BuildClassLookup();
        }
    }

    public XElement GetFirstNodeOfClass(string className)
    {
        TryBuildClassLookup();

        return ClassLookup[className].First();
    }

    public bool PopObjectAsXml<T>(T node)
        where T : IHavokObject
    {
        if (ObjectElementMap.ContainsKey(node))
        {
            return false;
        }
        XMapElement mappedElement = new(Serializer.SerializeObject(node));
        mappedElement.MapSlice(mappedElement);
        ObjectElementMap.Add(node, mappedElement);
        return true;
    }

    public bool PopObjectAsXml(string nodeName)
    {
        if (XmlDeserializer.TryGetObject(nodeName, out var havokObj))
        {
            return PopObjectAsXml(havokObj);
        }
        return false;
    }

    public bool PushXmlAsObject<T>(T targetObject)
        where T : IHavokObject
    {
        XMapElement? element;
        string? name;
        lock (ObjectElementMap)
        {
            if (
                !ObjectElementMap.TryGetValue(targetObject, out element)
                || !Serializer.TryGetName(targetObject, out name)
            )
            {
                return false;
            }
        }
        try
        {
            var obj = XmlDeserializer.DeserializeRuntimeObjectOverwrite(element);
            lock (XmlDeserializer)
            {
                HavokMove.Move<T>((T)obj, targetObject);
                //PartialDeserializer.UpdateDirectReference(targetObject, obj);
                //PartialDeserializer.UpdatePropertyReferences(name, obj);
                XmlDeserializer.UpdateMapping(name, targetObject);
            }
            lock (ObjectElementMap)
            {
                ObjectElementMap.Remove(targetObject);
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"Packfile > Active Nodes > Push > FAILED > {ex}");
        }
        return true;
    }

    public T GetPushXmlAsObject<T>(T targetObject)
        where T : class, IHavokObject
    {
        lock (ObjectElementMap)
        {
            if (
                !ObjectElementMap.TryGetValue(targetObject, out XMapElement? element)
                || !Serializer.TryGetName(targetObject, out string? name)
            )
            {
                return targetObject;
            }

            try
            {
                var obj = XmlDeserializer.DeserializeRuntimeObjectOverwrite(element);
                lock (XmlDeserializer)
                {
                    HavokMove.Move<T>((T)obj, targetObject);
                    //PartialDeserializer.UpdateDirectReference(targetObject, obj);
                    //PartialDeserializer.UpdatePropertyReferences(name, obj);
                    XmlDeserializer.UpdateMapping(name, targetObject);
                }
                ObjectElementMap.Remove(targetObject);
            }
            catch (Exception ex)
            {
                Logger.Error($"Packfile > Active Nodes > Push > FAILED > {ex}");
            }
            if (!XmlDeserializer.TryGetObjectAs<T>(name, out var newObj))
            {
                return targetObject;
            }
            return newObj;
        }
    }

    public T GetPushedObjectAs<T>(string name)
        where T : class, IHavokObject
    {
        lock (XmlDeserializer) //Lock important! Prevents mapping and deserialization from getting de-synced on high parallelization machines
        {
            return GetPushXmlAsObject(XmlDeserializer.GetObjectAs<T>(name));
        }
    }

    public virtual void ApplyPriorityChanges(IPackFileDispatcher dispatcher) { }

    public virtual void PushPriorityObjects() { }

    public virtual void PushXmlAsObjects()
    {
        foreach (var kvp in ObjectElementMap.OrderBy(kvp => BinaryDeserializer.GetOrder(kvp.Key)))
        {
            IHavokObject? obj = null;
            try
            {
                obj = XmlDeserializer.DeserializeRuntimeObjectOverwrite(kvp.Value);
            }
            catch (Exception ex)
            {
                Logger.Error($"Packfile > Active Nodes > Push > FAILED > {ex}");
                continue;
            }
            string name = Serializer.GetName(kvp.Key);
            HavokMove.Move(obj, kvp.Key);
            //PartialDeserializer.UpdateDirectReference(kvp.Key, obj);
            //PartialDeserializer.UpdatePropertyReferences(name, obj);
            XmlDeserializer.UpdateMapping(name, kvp.Key);
        }
        ObjectElementMap.Clear();
    }

    public bool Equals(PackFile? other)
    {
        return other != null
            && other.InputHandle.FullName.Equals(
                InputHandle.FullName,
                StringComparison.OrdinalIgnoreCase
            );
    }

    public bool Equals(IPackFile? other)
    {
        if (other == null)
            return false;
        return other.InputHandle.FullName.Equals(
            InputHandle.FullName,
            StringComparison.OrdinalIgnoreCase
        );
    }

    public override int GetHashCode()
    {
        return InputHandle.FullName.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not PackFile)
            return false;

        return Equals((PackFile)obj);
    }
}
