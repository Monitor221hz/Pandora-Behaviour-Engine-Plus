using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConstraintAtom Signatire: 0x59d67ef6 size: 2 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_UINT16 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: AtomType
    public partial class hkpConstraintAtom : IHavokObject, IEquatable<hkpConstraintAtom?>
    {
        public ushort type { set; get; }

        public virtual uint Signature { set; get; } = 0x59d67ef6;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            type = br.ReadUInt16();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt16(type);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            type = xd.ReadFlag<AtomType, ushort>(xe, nameof(type));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteEnum<AtomType, ushort>(xe, nameof(type), type);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConstraintAtom);
        }

        public bool Equals(hkpConstraintAtom? other)
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

