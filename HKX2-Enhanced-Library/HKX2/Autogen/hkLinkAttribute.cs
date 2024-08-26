using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkLinkAttribute Signatire: 0x255d8164 size: 1 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: Link
    public partial class hkLinkAttribute : IHavokObject, IEquatable<hkLinkAttribute?>
    {
        public sbyte type { set; get; }

        public virtual uint Signature { set; get; } = 0x255d8164;

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
            type = xd.ReadFlag<Link, sbyte>(xe, nameof(type));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteEnum<Link, sbyte>(xe, nameof(type), type);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkLinkAttribute);
        }

        public bool Equals(hkLinkAttribute? other)
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

