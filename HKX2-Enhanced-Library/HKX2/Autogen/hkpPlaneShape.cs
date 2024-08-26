using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPlaneShape Signatire: 0xc36bbd30 size: 80 flags: FLAGS_NONE

    // plane class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // aabbCenter class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // aabbHalfExtents class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpPlaneShape : hkpHeightFieldShape, IEquatable<hkpPlaneShape?>
    {
        public Vector4 plane { set; get; }
        public Vector4 aabbCenter { set; get; }
        public Vector4 aabbHalfExtents { set; get; }

        public override uint Signature { set; get; } = 0xc36bbd30;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            plane = br.ReadVector4();
            aabbCenter = br.ReadVector4();
            aabbHalfExtents = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(plane);
            bw.WriteVector4(aabbCenter);
            bw.WriteVector4(aabbHalfExtents);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            plane = xd.ReadVector4(xe, nameof(plane));
            aabbCenter = xd.ReadVector4(xe, nameof(aabbCenter));
            aabbHalfExtents = xd.ReadVector4(xe, nameof(aabbHalfExtents));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(plane), plane);
            xs.WriteVector4(xe, nameof(aabbCenter), aabbCenter);
            xs.WriteVector4(xe, nameof(aabbHalfExtents), aabbHalfExtents);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPlaneShape);
        }

        public bool Equals(hkpPlaneShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   plane.Equals(other.plane) &&
                   aabbCenter.Equals(other.aabbCenter) &&
                   aabbHalfExtents.Equals(other.aabbHalfExtents) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(plane);
            hashcode.Add(aabbCenter);
            hashcode.Add(aabbHalfExtents);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

