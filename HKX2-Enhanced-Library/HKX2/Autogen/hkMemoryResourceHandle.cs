using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkMemoryResourceHandle Signatire: 0xbffac086 size: 48 flags: FLAGS_NONE

    // variant class: hkReferencedObject Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // references class: hkMemoryResourceHandleExternalLink Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkMemoryResourceHandle : hkResourceHandle, IEquatable<hkMemoryResourceHandle?>
    {
        public hkReferencedObject? variant { set; get; }
        public string name { set; get; } = "";
        public IList<hkMemoryResourceHandleExternalLink> references { set; get; } = Array.Empty<hkMemoryResourceHandleExternalLink>();

        public override uint Signature { set; get; } = 0xbffac086;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            variant = des.ReadClassPointer<hkReferencedObject>(br);
            name = des.ReadStringPointer(br);
            references = des.ReadClassArray<hkMemoryResourceHandleExternalLink>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, variant);
            s.WriteStringPointer(bw, name);
            s.WriteClassArray(bw, references);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            variant = xd.ReadClassPointer<hkReferencedObject>(this, xe, nameof(variant));
            name = xd.ReadString(xe, nameof(name));
            references = xd.ReadClassArray<hkMemoryResourceHandleExternalLink>(xe, nameof(references));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(variant), variant);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteClassArray(xe, nameof(references), references);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkMemoryResourceHandle);
        }

        public bool Equals(hkMemoryResourceHandle? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((variant is null && other.variant is null) || (variant is not null && other.variant is not null && variant.Equals((IHavokObject)other.variant))) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   references.SequenceEqual(other.references) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(variant);
            hashcode.Add(name);
            hashcode.Add(references.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

