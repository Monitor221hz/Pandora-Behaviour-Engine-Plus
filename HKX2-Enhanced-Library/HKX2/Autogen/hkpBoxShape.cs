using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBoxShape Signatire: 0x3444d2d5 size: 64 flags: FLAGS_NONE

    // halfExtents class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpBoxShape : hkpConvexShape, IEquatable<hkpBoxShape?>
    {
        public Vector4 halfExtents { set; get; }

        public override uint Signature { set; get; } = 0x3444d2d5;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            halfExtents = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            bw.WriteVector4(halfExtents);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            halfExtents = xd.ReadVector4(xe, nameof(halfExtents));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(halfExtents), halfExtents);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBoxShape);
        }

        public bool Equals(hkpBoxShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   halfExtents.Equals(other.halfExtents) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(halfExtents);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

