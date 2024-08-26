using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConstraintChainData Signatire: 0x5facc7ff size: 24 flags: FLAGS_NONE


    public partial class hkpConstraintChainData : hkpConstraintData, IEquatable<hkpConstraintChainData?>
    {


        public override uint Signature { set; get; } = 0x5facc7ff;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConstraintChainData);
        }

        public bool Equals(hkpConstraintChainData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

