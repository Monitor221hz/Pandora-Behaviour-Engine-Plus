using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMoppCodeCodeInfo Signatire: 0xd8fdbb08 size: 16 flags: FLAGS_NONE

    // offset class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkpMoppCodeCodeInfo : IHavokObject, IEquatable<hkpMoppCodeCodeInfo?>
    {
        public Vector4 offset { set; get; }

        public virtual uint Signature { set; get; } = 0xd8fdbb08;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            offset = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(offset);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            offset = xd.ReadVector4(xe, nameof(offset));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(offset), offset);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMoppCodeCodeInfo);
        }

        public bool Equals(hkpMoppCodeCodeInfo? other)
        {
            return other is not null &&
                   offset.Equals(other.offset) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(offset);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

