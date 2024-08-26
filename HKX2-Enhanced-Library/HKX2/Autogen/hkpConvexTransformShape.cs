using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConvexTransformShape Signatire: 0xae3e5017 size: 128 flags: FLAGS_NONE

    // transform class:  Type.TYPE_TRANSFORM Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpConvexTransformShape : hkpConvexTransformShapeBase, IEquatable<hkpConvexTransformShape?>
    {
        public Matrix4x4 transform { set; get; }

        public override uint Signature { set; get; } = 0xae3e5017;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            transform = des.ReadTransform(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteTransform(bw, transform);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            transform = xd.ReadTransform(xe, nameof(transform));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteTransform(xe, nameof(transform), transform);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConvexTransformShape);
        }

        public bool Equals(hkpConvexTransformShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   transform.Equals(other.transform) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(transform);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

