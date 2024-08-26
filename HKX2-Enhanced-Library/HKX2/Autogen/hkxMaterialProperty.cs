using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxMaterialProperty Signatire: 0xd295234d size: 8 flags: FLAGS_NONE

    // key class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // value class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkxMaterialProperty : IHavokObject, IEquatable<hkxMaterialProperty?>
    {
        public uint key { set; get; }
        public uint value { set; get; }

        public virtual uint Signature { set; get; } = 0xd295234d;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            key = br.ReadUInt32();
            value = br.ReadUInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(key);
            bw.WriteUInt32(value);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            key = xd.ReadUInt32(xe, nameof(key));
            value = xd.ReadUInt32(xe, nameof(value));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(key), key);
            xs.WriteNumber(xe, nameof(value), value);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxMaterialProperty);
        }

        public bool Equals(hkxMaterialProperty? other)
        {
            return other is not null &&
                   key.Equals(other.key) &&
                   value.Equals(other.value) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(key);
            hashcode.Add(value);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

