using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPhantomCallbackShape Signatire: 0xe7eca7eb size: 32 flags: FLAGS_NONE


    public partial class hkpPhantomCallbackShape : hkpShape, IEquatable<hkpPhantomCallbackShape?>
    {


        public override uint Signature { set; get; } = 0xe7eca7eb;

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
            return Equals(obj as hkpPhantomCallbackShape);
        }

        public bool Equals(hkpPhantomCallbackShape? other)
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

