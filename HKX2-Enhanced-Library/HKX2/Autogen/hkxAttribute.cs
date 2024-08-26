using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxAttribute Signatire: 0x7375cae3 size: 16 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // value class: hkReferencedObject Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkxAttribute : IHavokObject, IEquatable<hkxAttribute?>
    {
        public string name { set; get; } = "";
        public hkReferencedObject? value { set; get; }

        public virtual uint Signature { set; get; } = 0x7375cae3;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            name = des.ReadStringPointer(br);
            value = des.ReadClassPointer<hkReferencedObject>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteStringPointer(bw, name);
            s.WriteClassPointer(bw, value);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            name = xd.ReadString(xe, nameof(name));
            value = xd.ReadClassPointer<hkReferencedObject>(this, xe, nameof(value));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(name), name);
            xs.WriteClassPointer(xe, nameof(value), value);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxAttribute);
        }

        public bool Equals(hkxAttribute? other)
        {
            return other is not null &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   ((value is null && other.value is null) || (value is not null && other.value is not null && value.Equals((IHavokObject)other.value))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(name);
            hashcode.Add(value);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

