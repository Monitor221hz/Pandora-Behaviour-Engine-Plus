using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkMemoryResourceContainer Signatire: 0x4762f92a size: 64 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // parent class: hkMemoryResourceContainer Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // resourceHandles class: hkMemoryResourceHandle Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // children class: hkMemoryResourceContainer Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkMemoryResourceContainer : hkResourceContainer, IEquatable<hkMemoryResourceContainer?>
    {
        public string name { set; get; } = "";
        private hkMemoryResourceContainer? parent { set; get; }
        public IList<hkMemoryResourceHandle> resourceHandles { set; get; } = Array.Empty<hkMemoryResourceHandle>();
        public IList<hkMemoryResourceContainer> children { set; get; } = Array.Empty<hkMemoryResourceContainer>();

        public override uint Signature { set; get; } = 0x4762f92a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            name = des.ReadStringPointer(br);
            parent = des.ReadClassPointer<hkMemoryResourceContainer>(br);
            resourceHandles = des.ReadClassPointerArray<hkMemoryResourceHandle>(br);
            children = des.ReadClassPointerArray<hkMemoryResourceContainer>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, name);
            s.WriteClassPointer(bw, parent);
            s.WriteClassPointerArray(bw, resourceHandles);
            s.WriteClassPointerArray(bw, children);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
            resourceHandles = xd.ReadClassPointerArray<hkMemoryResourceHandle>(this, xe, nameof(resourceHandles));
            children = xd.ReadClassPointerArray<hkMemoryResourceContainer>(this, xe, nameof(children));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteSerializeIgnored(xe, nameof(parent));
            xs.WriteClassPointerArray(xe, nameof(resourceHandles), resourceHandles!);
            xs.WriteClassPointerArray(xe, nameof(children), children!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkMemoryResourceContainer);
        }

        public bool Equals(hkMemoryResourceContainer? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   resourceHandles.SequenceEqual(other.resourceHandles) &&
                   children.SequenceEqual(other.children) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name);
            hashcode.Add(resourceHandles.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(children.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

