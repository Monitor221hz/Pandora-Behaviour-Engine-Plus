using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbComputeRotationFromAxisAngleModifier Signatire: 0x9b3f6936 size: 128 flags: FLAGS_NONE

    // rotationOut class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // axis class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // angleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkbComputeRotationFromAxisAngleModifier : hkbModifier, IEquatable<hkbComputeRotationFromAxisAngleModifier?>
    {
        public Quaternion rotationOut { set; get; }
        public Vector4 axis { set; get; }
        public float angleDegrees { set; get; }

        public override uint Signature { set; get; } = 0x9b3f6936;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rotationOut = des.ReadQuaternion(br);
            axis = br.ReadVector4();
            angleDegrees = br.ReadSingle();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQuaternion(bw, rotationOut);
            bw.WriteVector4(axis);
            bw.WriteSingle(angleDegrees);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rotationOut = xd.ReadQuaternion(xe, nameof(rotationOut));
            axis = xd.ReadVector4(xe, nameof(axis));
            angleDegrees = xd.ReadSingle(xe, nameof(angleDegrees));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQuaternion(xe, nameof(rotationOut), rotationOut);
            xs.WriteVector4(xe, nameof(axis), axis);
            xs.WriteFloat(xe, nameof(angleDegrees), angleDegrees);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbComputeRotationFromAxisAngleModifier);
        }

        public bool Equals(hkbComputeRotationFromAxisAngleModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rotationOut.Equals(other.rotationOut) &&
                   axis.Equals(other.axis) &&
                   angleDegrees.Equals(other.angleDegrees) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rotationOut);
            hashcode.Add(axis);
            hashcode.Add(angleDegrees);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

