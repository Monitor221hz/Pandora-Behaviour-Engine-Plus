using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkPackedVector3 Signatire: 0x9c16df5b size: 8 flags: FLAGS_NONE

    // values class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 4 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkPackedVector3 : IHavokObject, IEquatable<hkPackedVector3?>
    {
        public short[] values = new short[4];

        public virtual uint Signature { set; get; } = 0x9c16df5b;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            values = des.ReadInt16CStyleArray(br, 4);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteInt16CStyleArray(bw, values);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            values = xd.ReadInt16CStyleArray(xe, nameof(values), 4);
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumberArray(xe, nameof(values), values);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkPackedVector3);
        }

        public bool Equals(hkPackedVector3? other)
        {
            return other is not null &&
                   values.SequenceEqual(other.values) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(values.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

