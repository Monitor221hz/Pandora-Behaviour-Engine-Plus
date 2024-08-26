using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpAngMotorConstraintAtom Signatire: 0x81f087ff size: 24 flags: FLAGS_NONE

    // isEnabled class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // motorAxis class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 3 flags: FLAGS_NONE enum: 
    // initializedOffset class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // previousTargetAngleOffset class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 6 flags: FLAGS_NONE enum: 
    // correspondingAngLimitSolverResultOffset class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // targetAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // motor class: hkpConstraintMotor Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpAngMotorConstraintAtom : hkpConstraintAtom, IEquatable<hkpAngMotorConstraintAtom?>
    {
        public bool isEnabled { set; get; }
        public byte motorAxis { set; get; }
        public short initializedOffset { set; get; }
        public short previousTargetAngleOffset { set; get; }
        public short correspondingAngLimitSolverResultOffset { set; get; }
        public float targetAngle { set; get; }
        public hkpConstraintMotor? motor { set; get; }

        public override uint Signature { set; get; } = 0x81f087ff;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isEnabled = br.ReadBoolean();
            motorAxis = br.ReadByte();
            initializedOffset = br.ReadInt16();
            previousTargetAngleOffset = br.ReadInt16();
            correspondingAngLimitSolverResultOffset = br.ReadInt16();
            br.Position += 2;
            targetAngle = br.ReadSingle();
            motor = des.ReadClassPointer<hkpConstraintMotor>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(isEnabled);
            bw.WriteByte(motorAxis);
            bw.WriteInt16(initializedOffset);
            bw.WriteInt16(previousTargetAngleOffset);
            bw.WriteInt16(correspondingAngLimitSolverResultOffset);
            bw.Position += 2;
            bw.WriteSingle(targetAngle);
            s.WriteClassPointer(bw, motor);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isEnabled = xd.ReadBoolean(xe, nameof(isEnabled));
            motorAxis = xd.ReadByte(xe, nameof(motorAxis));
            initializedOffset = xd.ReadInt16(xe, nameof(initializedOffset));
            previousTargetAngleOffset = xd.ReadInt16(xe, nameof(previousTargetAngleOffset));
            correspondingAngLimitSolverResultOffset = xd.ReadInt16(xe, nameof(correspondingAngLimitSolverResultOffset));
            targetAngle = xd.ReadSingle(xe, nameof(targetAngle));
            motor = xd.ReadClassPointer<hkpConstraintMotor>(this, xe, nameof(motor));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(isEnabled), isEnabled);
            xs.WriteNumber(xe, nameof(motorAxis), motorAxis);
            xs.WriteNumber(xe, nameof(initializedOffset), initializedOffset);
            xs.WriteNumber(xe, nameof(previousTargetAngleOffset), previousTargetAngleOffset);
            xs.WriteNumber(xe, nameof(correspondingAngLimitSolverResultOffset), correspondingAngLimitSolverResultOffset);
            xs.WriteFloat(xe, nameof(targetAngle), targetAngle);
            xs.WriteClassPointer(xe, nameof(motor), motor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpAngMotorConstraintAtom);
        }

        public bool Equals(hkpAngMotorConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isEnabled.Equals(other.isEnabled) &&
                   motorAxis.Equals(other.motorAxis) &&
                   initializedOffset.Equals(other.initializedOffset) &&
                   previousTargetAngleOffset.Equals(other.previousTargetAngleOffset) &&
                   correspondingAngLimitSolverResultOffset.Equals(other.correspondingAngLimitSolverResultOffset) &&
                   targetAngle.Equals(other.targetAngle) &&
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
            hashcode.Add(previousTargetAngleOffset);
            hashcode.Add(correspondingAngLimitSolverResultOffset);
            hashcode.Add(targetAngle);
            hashcode.Add(motor);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

