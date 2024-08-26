using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkHalf8 Signatire: 0x7684dc80 size: 16 flags: FLAGS_NONE

    // quad class:  Type.TYPE_HALF Type.TYPE_VOID arrSize: 8 offset: 0 flags: ALIGN_16|FLAGS_NONE enum: 
    public partial class hkHalf8 : IHavokObject, IEquatable<hkHalf8?>
    {
        public Half[] quad = new Half[8];

        public virtual uint Signature { set; get; } = 0x7684dc80;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            quad = des.ReadHalfCStyleArray(br, 8);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteHalfCStyleArray(bw, quad);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            quad = xd.ReadHalfCStyleArray(xe, nameof(quad), 8);
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloatArray(xe, nameof(quad), quad);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkHalf8);
        }

        public bool Equals(hkHalf8? other)
        {
            return other is not null &&
                   quad.SequenceEqual(other.quad) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(quad.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

