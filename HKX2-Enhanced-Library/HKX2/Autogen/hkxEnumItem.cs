using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxEnumItem Signatire: 0xdf4cf1e9 size: 16 flags: FLAGS_NONE

    // value class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkxEnumItem : IHavokObject, IEquatable<hkxEnumItem?>
    {
        public int value { set; get; }
        public string name { set; get; } = "";

        public virtual uint Signature { set; get; } = 0xdf4cf1e9;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            value = br.ReadInt32();
            br.Position += 4;
            name = des.ReadStringPointer(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(value);
            bw.Position += 4;
            s.WriteStringPointer(bw, name);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            value = xd.ReadInt32(xe, nameof(value));
            name = xd.ReadString(xe, nameof(name));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(value), value);
            xs.WriteString(xe, nameof(name), name);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxEnumItem);
        }

        public bool Equals(hkxEnumItem? other)
        {
            return other is not null &&
                   value.Equals(other.value) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(value);
            hashcode.Add(name);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

