using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCylinderShape Signatire: 0x3e463c3a size: 112 flags: FLAGS_NONE

    // cylRadius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // cylBaseRadiusFactorForHeightFieldCollisions class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    // vertexA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // vertexB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // perpendicular1 class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // perpendicular2 class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    public partial class hkpCylinderShape : hkpConvexShape, IEquatable<hkpCylinderShape?>
    {
        public float cylRadius { set; get; }
        public float cylBaseRadiusFactorForHeightFieldCollisions { set; get; }
        public Vector4 vertexA { set; get; }
        public Vector4 vertexB { set; get; }
        public Vector4 perpendicular1 { set; get; }
        public Vector4 perpendicular2 { set; get; }

        public override uint Signature { set; get; } = 0x3e463c3a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            cylRadius = br.ReadSingle();
            cylBaseRadiusFactorForHeightFieldCollisions = br.ReadSingle();
            vertexA = br.ReadVector4();
            vertexB = br.ReadVector4();
            perpendicular1 = br.ReadVector4();
            perpendicular2 = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(cylRadius);
            bw.WriteSingle(cylBaseRadiusFactorForHeightFieldCollisions);
            bw.WriteVector4(vertexA);
            bw.WriteVector4(vertexB);
            bw.WriteVector4(perpendicular1);
            bw.WriteVector4(perpendicular2);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            cylRadius = xd.ReadSingle(xe, nameof(cylRadius));
            cylBaseRadiusFactorForHeightFieldCollisions = xd.ReadSingle(xe, nameof(cylBaseRadiusFactorForHeightFieldCollisions));
            vertexA = xd.ReadVector4(xe, nameof(vertexA));
            vertexB = xd.ReadVector4(xe, nameof(vertexB));
            perpendicular1 = xd.ReadVector4(xe, nameof(perpendicular1));
            perpendicular2 = xd.ReadVector4(xe, nameof(perpendicular2));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(cylRadius), cylRadius);
            xs.WriteFloat(xe, nameof(cylBaseRadiusFactorForHeightFieldCollisions), cylBaseRadiusFactorForHeightFieldCollisions);
            xs.WriteVector4(xe, nameof(vertexA), vertexA);
            xs.WriteVector4(xe, nameof(vertexB), vertexB);
            xs.WriteVector4(xe, nameof(perpendicular1), perpendicular1);
            xs.WriteVector4(xe, nameof(perpendicular2), perpendicular2);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCylinderShape);
        }

        public bool Equals(hkpCylinderShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   cylRadius.Equals(other.cylRadius) &&
                   cylBaseRadiusFactorForHeightFieldCollisions.Equals(other.cylBaseRadiusFactorForHeightFieldCollisions) &&
                   vertexA.Equals(other.vertexA) &&
                   vertexB.Equals(other.vertexB) &&
                   perpendicular1.Equals(other.perpendicular1) &&
                   perpendicular2.Equals(other.perpendicular2) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(cylRadius);
            hashcode.Add(cylBaseRadiusFactorForHeightFieldCollisions);
            hashcode.Add(vertexA);
            hashcode.Add(vertexB);
            hashcode.Add(perpendicular1);
            hashcode.Add(perpendicular2);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

