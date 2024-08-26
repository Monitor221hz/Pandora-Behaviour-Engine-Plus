using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBoxMotion Signatire: 0xbafa2bb7 size: 320 flags: FLAGS_NONE


    public partial class hkpBoxMotion : hkpMotion, IEquatable<hkpBoxMotion?>
    {


        public override uint Signature { set; get; } = 0xbafa2bb7;

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
            return Equals(obj as hkpBoxMotion);
        }

        public bool Equals(hkpBoxMotion? other)
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

