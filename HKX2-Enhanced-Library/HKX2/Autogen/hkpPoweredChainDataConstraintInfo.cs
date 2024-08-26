using System;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPoweredChainDataConstraintInfo Signatire: 0xf88aee25 size: 96 flags: FLAGS_NONE

    // pivotInA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // pivotInB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // aTc class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // bTc class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // motors class: hkpConstraintMotor Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 3 offset: 64 flags: FLAGS_NONE enum: 
    // switchBodies class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    public partial class hkpPoweredChainDataConstraintInfo : IHavokObject, IEquatable<hkpPoweredChainDataConstraintInfo?>
    {
        public Vector4 pivotInA { set; get; }
        public Vector4 pivotInB { set; get; }
        public Quaternion aTc { set; get; }
        public Quaternion bTc { set; get; }
        public hkpConstraintMotor?[] motors = new hkpConstraintMotor?[3];
        public bool switchBodies { set; get; }

        public virtual uint Signature { set; get; } = 0xf88aee25;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            pivotInA = br.ReadVector4();
            pivotInB = br.ReadVector4();
            aTc = des.ReadQuaternion(br);
            bTc = des.ReadQuaternion(br);
            motors = des.ReadClassPointerCStyleArray<hkpConstraintMotor>(br, 3);
            switchBodies = br.ReadBoolean();
            br.Position += 7;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(pivotInA);
            bw.WriteVector4(pivotInB);
            s.WriteQuaternion(bw, aTc);
            s.WriteQuaternion(bw, bTc);
            s.WriteClassPointerCStyleArray(bw, motors);
            bw.WriteBoolean(switchBodies);
            bw.Position += 7;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            pivotInA = xd.ReadVector4(xe, nameof(pivotInA));
            pivotInB = xd.ReadVector4(xe, nameof(pivotInB));
            aTc = xd.ReadQuaternion(xe, nameof(aTc));
            bTc = xd.ReadQuaternion(xe, nameof(bTc));
            motors = xd.ReadClassPointerCStyleArray<hkpConstraintMotor>(this, xe, nameof(motors), 3);
            switchBodies = xd.ReadBoolean(xe, nameof(switchBodies));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(pivotInA), pivotInA);
            xs.WriteVector4(xe, nameof(pivotInB), pivotInB);
            xs.WriteQuaternion(xe, nameof(aTc), aTc);
            xs.WriteQuaternion(xe, nameof(bTc), bTc);
            xs.WriteClassPointerArray(xe, nameof(motors), motors);
            xs.WriteBoolean(xe, nameof(switchBodies), switchBodies);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPoweredChainDataConstraintInfo);
        }

        public bool Equals(hkpPoweredChainDataConstraintInfo? other)
        {
            return other is not null &&
                   pivotInA.Equals(other.pivotInA) &&
                   pivotInB.Equals(other.pivotInB) &&
                   aTc.Equals(other.aTc) &&
                   bTc.Equals(other.bTc) &&
                   motors.SequenceEqual(other.motors) &&
                   switchBodies.Equals(other.switchBodies) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(pivotInA);
            hashcode.Add(pivotInB);
            hashcode.Add(aTc);
            hashcode.Add(bTc);
            hashcode.Add(motors.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(switchBodies);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

