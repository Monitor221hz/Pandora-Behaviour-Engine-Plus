using System;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpRagdollMotorConstraintAtom Signatire: 0x71013826 size: 96 flags: FLAGS_NONE

    // isEnabled class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // initializedOffset class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // previousTargetAnglesOffset class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 6 flags: FLAGS_NONE enum: 
    // target_bRca class:  Type.TYPE_MATRIX3 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // motors class: hkpConstraintMotor Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 3 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpRagdollMotorConstraintAtom : hkpConstraintAtom, IEquatable<hkpRagdollMotorConstraintAtom?>
    {
        public bool isEnabled { set; get; }
        public short initializedOffset { set; get; }
        public short previousTargetAnglesOffset { set; get; }
        public Matrix4x4 target_bRca { set; get; }
        public hkpConstraintMotor?[] motors = new hkpConstraintMotor?[3];

        public override uint Signature { set; get; } = 0x71013826;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isEnabled = br.ReadBoolean();
            br.Position += 1;
            initializedOffset = br.ReadInt16();
            previousTargetAnglesOffset = br.ReadInt16();
            br.Position += 8;
            target_bRca = des.ReadMatrix3(br);
            motors = des.ReadClassPointerCStyleArray<hkpConstraintMotor>(br, 3);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(isEnabled);
            bw.Position += 1;
            bw.WriteInt16(initializedOffset);
            bw.WriteInt16(previousTargetAnglesOffset);
            bw.Position += 8;
            s.WriteMatrix3(bw, target_bRca);
            s.WriteClassPointerCStyleArray(bw, motors);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isEnabled = xd.ReadBoolean(xe, nameof(isEnabled));
            initializedOffset = xd.ReadInt16(xe, nameof(initializedOffset));
            previousTargetAnglesOffset = xd.ReadInt16(xe, nameof(previousTargetAnglesOffset));
            target_bRca = xd.ReadMatrix3(xe, nameof(target_bRca));
            motors = xd.ReadClassPointerCStyleArray<hkpConstraintMotor>(this, xe, nameof(motors), 3);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(isEnabled), isEnabled);
            xs.WriteNumber(xe, nameof(initializedOffset), initializedOffset);
            xs.WriteNumber(xe, nameof(previousTargetAnglesOffset), previousTargetAnglesOffset);
            xs.WriteMatrix3(xe, nameof(target_bRca), target_bRca);
            xs.WriteClassPointerArray(xe, nameof(motors), motors);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpRagdollMotorConstraintAtom);
        }

        public bool Equals(hkpRagdollMotorConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isEnabled.Equals(other.isEnabled) &&
                   initializedOffset.Equals(other.initializedOffset) &&
                   previousTargetAnglesOffset.Equals(other.previousTargetAnglesOffset) &&
                   target_bRca.Equals(other.target_bRca) &&
                   motors.SequenceEqual(other.motors) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(isEnabled);
            hashcode.Add(initializedOffset);
            hashcode.Add(previousTargetAnglesOffset);
            hashcode.Add(target_bRca);
            hashcode.Add(motors.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

