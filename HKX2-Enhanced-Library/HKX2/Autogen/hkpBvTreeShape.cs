using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBvTreeShape Signatire: 0xa823d623 size: 40 flags: FLAGS_NONE

    // bvTreeType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: BvTreeType
    public partial class hkpBvTreeShape : hkpShape, IEquatable<hkpBvTreeShape?>
    {
        public byte bvTreeType { set; get; }

        public override uint Signature { set; get; } = 0xa823d623;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            bvTreeType = br.ReadByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(bvTreeType);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            bvTreeType = xd.ReadFlag<BvTreeType, byte>(xe, nameof(bvTreeType));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<BvTreeType, byte>(xe, nameof(bvTreeType), bvTreeType);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBvTreeShape);
        }

        public bool Equals(hkpBvTreeShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   bvTreeType.Equals(other.bvTreeType) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(bvTreeType);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

