using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterControllerModifierInternalState Signatire: 0xf8dfec0d size: 48 flags: FLAGS_NONE

    // gravity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // timestep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // isInitialVelocityAdded class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // isTouchingGround class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 37 flags: FLAGS_NONE enum: 
    public partial class hkbCharacterControllerModifierInternalState : hkReferencedObject, IEquatable<hkbCharacterControllerModifierInternalState?>
    {
        public Vector4 gravity { set; get; }
        public float timestep { set; get; }
        public bool isInitialVelocityAdded { set; get; }
        public bool isTouchingGround { set; get; }

        public override uint Signature { set; get; } = 0xf8dfec0d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            gravity = br.ReadVector4();
            timestep = br.ReadSingle();
            isInitialVelocityAdded = br.ReadBoolean();
            isTouchingGround = br.ReadBoolean();
            br.Position += 10;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(gravity);
            bw.WriteSingle(timestep);
            bw.WriteBoolean(isInitialVelocityAdded);
            bw.WriteBoolean(isTouchingGround);
            bw.Position += 10;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            gravity = xd.ReadVector4(xe, nameof(gravity));
            timestep = xd.ReadSingle(xe, nameof(timestep));
            isInitialVelocityAdded = xd.ReadBoolean(xe, nameof(isInitialVelocityAdded));
            isTouchingGround = xd.ReadBoolean(xe, nameof(isTouchingGround));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(gravity), gravity);
            xs.WriteFloat(xe, nameof(timestep), timestep);
            xs.WriteBoolean(xe, nameof(isInitialVelocityAdded), isInitialVelocityAdded);
            xs.WriteBoolean(xe, nameof(isTouchingGround), isTouchingGround);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterControllerModifierInternalState);
        }

        public bool Equals(hkbCharacterControllerModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   gravity.Equals(other.gravity) &&
                   timestep.Equals(other.timestep) &&
                   isInitialVelocityAdded.Equals(other.isInitialVelocityAdded) &&
                   isTouchingGround.Equals(other.isTouchingGround) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(gravity);
            hashcode.Add(timestep);
            hashcode.Add(isInitialVelocityAdded);
            hashcode.Add(isTouchingGround);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

