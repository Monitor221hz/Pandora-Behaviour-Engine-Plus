using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterControllerModifier Signatire: 0xf675d6fb size: 176 flags: FLAGS_NONE

    // controlData class: hkbCharacterControllerControlData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // initialVelocity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // initialVelocityCoordinates class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 128 flags: FLAGS_NONE enum: InitialVelocityCoordinates
    // motionMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 129 flags: FLAGS_NONE enum: MotionMode
    // forceDownwardMomentum class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 130 flags: FLAGS_NONE enum: 
    // applyGravity class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 131 flags: FLAGS_NONE enum: 
    // setInitialVelocity class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 132 flags: FLAGS_NONE enum: 
    // isTouchingGround class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 133 flags: FLAGS_NONE enum: 
    // gravity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timestep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 160 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // isInitialVelocityAdded class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 164 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbCharacterControllerModifier : hkbModifier, IEquatable<hkbCharacterControllerModifier?>
    {
        public hkbCharacterControllerControlData controlData { set; get; } = new();
        public Vector4 initialVelocity { set; get; }
        public sbyte initialVelocityCoordinates { set; get; }
        public sbyte motionMode { set; get; }
        public bool forceDownwardMomentum { set; get; }
        public bool applyGravity { set; get; }
        public bool setInitialVelocity { set; get; }
        public bool isTouchingGround { set; get; }
        private Vector4 gravity { set; get; }
        private float timestep { set; get; }
        private bool isInitialVelocityAdded { set; get; }

        public override uint Signature { set; get; } = 0xf675d6fb;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            controlData.Read(des, br);
            initialVelocity = br.ReadVector4();
            initialVelocityCoordinates = br.ReadSByte();
            motionMode = br.ReadSByte();
            forceDownwardMomentum = br.ReadBoolean();
            applyGravity = br.ReadBoolean();
            setInitialVelocity = br.ReadBoolean();
            isTouchingGround = br.ReadBoolean();
            br.Position += 10;
            gravity = br.ReadVector4();
            timestep = br.ReadSingle();
            isInitialVelocityAdded = br.ReadBoolean();
            br.Position += 11;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            controlData.Write(s, bw);
            bw.WriteVector4(initialVelocity);
            bw.WriteSByte(initialVelocityCoordinates);
            bw.WriteSByte(motionMode);
            bw.WriteBoolean(forceDownwardMomentum);
            bw.WriteBoolean(applyGravity);
            bw.WriteBoolean(setInitialVelocity);
            bw.WriteBoolean(isTouchingGround);
            bw.Position += 10;
            bw.WriteVector4(gravity);
            bw.WriteSingle(timestep);
            bw.WriteBoolean(isInitialVelocityAdded);
            bw.Position += 11;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            controlData = xd.ReadClass<hkbCharacterControllerControlData>(xe, nameof(controlData));
            initialVelocity = xd.ReadVector4(xe, nameof(initialVelocity));
            initialVelocityCoordinates = xd.ReadFlag<InitialVelocityCoordinates, sbyte>(xe, nameof(initialVelocityCoordinates));
            motionMode = xd.ReadFlag<MotionMode, sbyte>(xe, nameof(motionMode));
            forceDownwardMomentum = xd.ReadBoolean(xe, nameof(forceDownwardMomentum));
            applyGravity = xd.ReadBoolean(xe, nameof(applyGravity));
            setInitialVelocity = xd.ReadBoolean(xe, nameof(setInitialVelocity));
            isTouchingGround = xd.ReadBoolean(xe, nameof(isTouchingGround));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbCharacterControllerControlData>(xe, nameof(controlData), controlData);
            xs.WriteVector4(xe, nameof(initialVelocity), initialVelocity);
            xs.WriteEnum<InitialVelocityCoordinates, sbyte>(xe, nameof(initialVelocityCoordinates), initialVelocityCoordinates);
            xs.WriteEnum<MotionMode, sbyte>(xe, nameof(motionMode), motionMode);
            xs.WriteBoolean(xe, nameof(forceDownwardMomentum), forceDownwardMomentum);
            xs.WriteBoolean(xe, nameof(applyGravity), applyGravity);
            xs.WriteBoolean(xe, nameof(setInitialVelocity), setInitialVelocity);
            xs.WriteBoolean(xe, nameof(isTouchingGround), isTouchingGround);
            xs.WriteSerializeIgnored(xe, nameof(gravity));
            xs.WriteSerializeIgnored(xe, nameof(timestep));
            xs.WriteSerializeIgnored(xe, nameof(isInitialVelocityAdded));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterControllerModifier);
        }

        public bool Equals(hkbCharacterControllerModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((controlData is null && other.controlData is null) || (controlData is not null && other.controlData is not null && controlData.Equals((IHavokObject)other.controlData))) &&
                   initialVelocity.Equals(other.initialVelocity) &&
                   initialVelocityCoordinates.Equals(other.initialVelocityCoordinates) &&
                   motionMode.Equals(other.motionMode) &&
                   forceDownwardMomentum.Equals(other.forceDownwardMomentum) &&
                   applyGravity.Equals(other.applyGravity) &&
                   setInitialVelocity.Equals(other.setInitialVelocity) &&
                   isTouchingGround.Equals(other.isTouchingGround) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(controlData);
            hashcode.Add(initialVelocity);
            hashcode.Add(initialVelocityCoordinates);
            hashcode.Add(motionMode);
            hashcode.Add(forceDownwardMomentum);
            hashcode.Add(applyGravity);
            hashcode.Add(setInitialVelocity);
            hashcode.Add(isTouchingGround);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

