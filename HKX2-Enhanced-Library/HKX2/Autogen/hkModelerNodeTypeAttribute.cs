using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkModelerNodeTypeAttribute Signatire: 0x338c092f size: 1 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: ModelerType
    public partial class hkModelerNodeTypeAttribute : IHavokObject, IEquatable<hkModelerNodeTypeAttribute?>
    {
        public sbyte type { set; get; }

        public virtual uint Signature { set; get; } = 0x338c092f;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            type = br.ReadSByte();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSByte(type);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            type = xd.ReadFlag<ModelerType, sbyte>(xe, nameof(type));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteEnum<ModelerType, sbyte>(xe, nameof(type), type);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkModelerNodeTypeAttribute);
        }

        public bool Equals(hkModelerNodeTypeAttribute? other)
        {
            return other is not null &&
                   type.Equals(other.type) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(type);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

