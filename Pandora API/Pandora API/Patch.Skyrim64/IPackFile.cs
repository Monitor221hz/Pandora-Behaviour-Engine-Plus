// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using HKX2E;
using XmlCake.Linq;

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFile : IEquatable<IPackFile>
{
    MetaPackFileDeserializer BinaryDeserializer { get; }
    hkRootLevelContainer Container { get; }
    IPackFileDispatcher Dispatcher { get; }
    bool ExportSuccess { get; }
    IEnumerable<XElement> IndexedElements { get; }
    FileInfo InputHandle { get; }
    string Name { get; }
    int NodeCount { get; }
    FileInfo OutputHandle { get; }
    IProject? ParentProject { get; set; }
    string RelativeOutputDirectoryPath { get; }
    HavokXmlMetaDataSerializer Serializer { get; }
    string UniqueName { get; }
    HavokXmlDeserializer XmlDeserializer { get; }
    public int GetHashCode();
    void Activate();
    void ApplyChanges();
    void ApplyPriorityChanges(IPackFileDispatcher dispatcher);
    void BuildClassLookup();
    bool Equals(object? obj);
    XElement GetFirstNodeOfClass(string className);
    FileInfo GetOutputHandle(DirectoryInfo exportDirectory);
    T GetPushedObjectAs<T>(string name)
        where T : class, IHavokObject;
    T GetPushXmlAsObject<T>(T targetObject)
        where T : class, IHavokObject;
    void Load();
    bool PathExists(string nodeName, string path);
    bool PopObjectAsXml(string nodeName);
    bool PopObjectAsXml<T>(T node)
        where T : IHavokObject;
    void PopPriorityXmlAsObjects();
    void PushPriorityObjects();
    bool PushXmlAsObject<T>(T targetObject)
        where T : IHavokObject;
    void PushXmlAsObjects();
    FileInfo RebaseOutput(DirectoryInfo exportDirectory);
    bool TargetExists(string nodeName);
    void TryBuildClassLookup();
    bool TryGetXMap(string nodeName, [NotNullWhen(true)] out XMapElement? xmap);
}
