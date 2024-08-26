using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkClassEnumItem Signatire: 0xce6f8a6c size: 16 flags: FLAGS_NONE

    // value class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkClassEnumItem : IHavokObject, IEquatable<hkClassEnumItem?>
    {
        public int value { set; get; }
        public string name { set; get; } = "";

        public virtual uint Signature { set; get; } = 0xce6f8a6c;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            value = br.ReadInt32();
            br.Position += 4;
            name = des.ReadCString(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(value);
            bw.Position += 4;
            s.WriteCString(bw, name);
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
            return Equals(obj as hkClassEnumItem);
        }

        public bool Equals(hkClassEnumItem? other)
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

