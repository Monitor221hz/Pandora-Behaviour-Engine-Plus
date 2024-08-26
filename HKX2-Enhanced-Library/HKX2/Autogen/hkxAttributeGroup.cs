using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxAttributeGroup Signatire: 0x345ca95d size: 24 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // attributes class: hkxAttribute Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkxAttributeGroup : IHavokObject, IEquatable<hkxAttributeGroup?>
    {
        public string name { set; get; } = "";
        public IList<hkxAttribute> attributes { set; get; } = Array.Empty<hkxAttribute>();

        public virtual uint Signature { set; get; } = 0x345ca95d;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            name = des.ReadStringPointer(br);
            attributes = des.ReadClassArray<hkxAttribute>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteStringPointer(bw, name);
            s.WriteClassArray(bw, attributes);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            name = xd.ReadString(xe, nameof(name));
            attributes = xd.ReadClassArray<hkxAttribute>(xe, nameof(attributes));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(name), name);
            xs.WriteClassArray(xe, nameof(attributes), attributes);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxAttributeGroup);
        }

        public bool Equals(hkxAttributeGroup? other)
        {
            return other is not null &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   attributes.SequenceEqual(other.attributes) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(name);
            hashcode.Add(attributes.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

