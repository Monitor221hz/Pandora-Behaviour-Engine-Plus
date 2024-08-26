using System;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPointToPathConstraintData Signatire: 0x8e7cb5da size: 192 flags: FLAGS_NONE

    // atoms class: hkpBridgeAtoms Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // path class: hkpParametricCurve Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // maxFrictionForce class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // angularConstrainedDOF class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 60 flags: FLAGS_NONE enum: OrientationConstraintType
    // transforOS_KS class:  Type.TYPE_TRANSFORM Type.TYPE_VOID arrSize: 2 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpPointToPathConstraintData : hkpConstraintData, IEquatable<hkpPointToPathConstraintData?>
    {
        public hkpBridgeAtoms atoms { set; get; } = new();
        public hkpParametricCurve? path { set; get; }
        public float maxFrictionForce { set; get; }
        public sbyte angularConstrainedDOF { set; get; }
        public Matrix4x4[] transforOS_KS = new Matrix4x4[2];

        public override uint Signature { set; get; } = 0x8e7cb5da;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            atoms.Read(des, br);
            path = des.ReadClassPointer<hkpParametricCurve>(br);
            maxFrictionForce = br.ReadSingle();
            angularConstrainedDOF = br.ReadSByte();
            br.Position += 3;
            transforOS_KS = des.ReadTransformCStyleArray(br, 2);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            atoms.Write(s, bw);
            s.WriteClassPointer(bw, path);
            bw.WriteSingle(maxFrictionForce);
            bw.WriteSByte(angularConstrainedDOF);
            bw.Position += 3;
            s.WriteTransformCStyleArray(bw, transforOS_KS);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            atoms = xd.ReadClass<hkpBridgeAtoms>(xe, nameof(atoms));
            path = xd.ReadClassPointer<hkpParametricCurve>(this, xe, nameof(path));
            maxFrictionForce = xd.ReadSingle(xe, nameof(maxFrictionForce));
            angularConstrainedDOF = xd.ReadFlag<OrientationConstraintType, sbyte>(xe, nameof(angularConstrainedDOF));
            transforOS_KS = xd.ReadTransformCStyleArray(xe, nameof(transforOS_KS), 2);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpBridgeAtoms>(xe, nameof(atoms), atoms);
            xs.WriteClassPointer(xe, nameof(path), path);
            xs.WriteFloat(xe, nameof(maxFrictionForce), maxFrictionForce);
            xs.WriteEnum<OrientationConstraintType, sbyte>(xe, nameof(angularConstrainedDOF), angularConstrainedDOF);
            xs.WriteTransformArray(xe, nameof(transforOS_KS), transforOS_KS);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPointToPathConstraintData);
        }

        public bool Equals(hkpPointToPathConstraintData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((atoms is null && other.atoms is null) || (atoms is not null && other.atoms is not null && atoms.Equals((IHavokObject)other.atoms))) &&
                   ((path is null && other.path is null) || (path is not null && other.path is not null && path.Equals((IHavokObject)other.path))) &&
                   maxFrictionForce.Equals(other.maxFrictionForce) &&
                   angularConstrainedDOF.Equals(other.angularConstrainedDOF) &&
                   transforOS_KS.SequenceEqual(other.transforOS_KS) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(atoms);
            hashcode.Add(path);
            hashcode.Add(maxFrictionForce);
            hashcode.Add(angularConstrainedDOF);
            hashcode.Add(transforOS_KS.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

