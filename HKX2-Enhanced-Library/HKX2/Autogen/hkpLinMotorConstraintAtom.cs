using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpLinMotorConstraintAtom Signatire: 0x10312464 size: 24 flags: FLAGS_NONE

    // isEnabled class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // motorAxis class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 3 flags: FLAGS_NONE enum: 
    // initializedOffset class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // previousTargetPositionOffset class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 6 flags: FLAGS_NONE enum: 
    // targetPosition class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // motor class: hkpConstraintMotor Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpLinMotorConstraintAtom : hkpConstraintAtom, IEquatable<hkpLinMotorConstraintAtom?>
    {
        public bool isEnabled { set; get; }
        public byte motorAxis { set; get; }
        public short initializedOffset { set; get; }
        public short previousTargetPositionOffset { set; get; }
        public float targetPosition { set; get; }
        public hkpConstraintMotor? motor { set; get; }

        public override uint Signature { set; get; } = 0x10312464;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isEnabled = br.ReadBoolean();
            motorAxis = br.ReadByte();
            initializedOffset = br.ReadInt16();
            previousTargetPositionOffset = br.ReadInt16();
            targetPosition = br.ReadSingle();
            br.Position += 4;
            motor = des.ReadClassPointer<hkpConstraintMotor>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(isEnabled);
            bw.WriteByte(motorAxis);
            bw.WriteInt16(initializedOffset);
            bw.WriteInt16(previousTargetPositionOffset);
            bw.WriteSingle(targetPosition);
            bw.Position += 4;
            s.WriteClassPointer(bw, motor);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isEnabled = xd.ReadBoolean(xe, nameof(isEnabled));
            motorAxis = xd.ReadByte(xe, nameof(motorAxis));
            initializedOffset = xd.ReadInt16(xe, nameof(initializedOffset));
            previousTargetPositionOffset = xd.ReadInt16(xe, nameof(previousTargetPositionOffset));
            targetPosition = xd.ReadSingle(xe, nameof(targetPosition));
            motor = xd.ReadClassPointer<hkpConstraintMotor>(this, xe, nameof(motor));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(isEnabled), isEnabled);
            xs.WriteNumber(xe, nameof(motorAxis), motorAxis);
            xs.WriteNumber(xe, nameof(initializedOffset), initializedOffset);
            xs.WriteNumber(xe, nameof(previousTargetPositionOffset), previousTargetPositionOffset);
            xs.WriteFloat(xe, nameof(targetPosition), targetPosition);
            xs.WriteClassPointer(xe, nameof(motor), motor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpLinMotorConstraintAtom);
        }

        public bool Equals(hkpLinMotorConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isEnabled.Equals(other.isEnabled) &&
                   motorAxis.Equals(other.motorAxis) &&
                   initializedOffset.Equals(other.initializedOffset) &&
                   previousTargetPositionOffset.Equals(other.previousTargetPositionOffset) &&
                   targetPosition.Equals(other.targetPosition) &&
                   ((motor is null && other.motor is null) || (motor is not null && other.motor is not null && motor.Equals((IHavokObject)other.motor))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(isEnabled);
            hashcode.Add(motorAxis);
            hashcode.Add(initializedOffset);
            hashcode.Add(previousTargetPositionOffset);
            hashcode.Add(targetPosition);
            hashcode.Add(motor);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

