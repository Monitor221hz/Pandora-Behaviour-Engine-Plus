using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpGravityGun Signatire: 0x5e2754cd size: 128 flags: FLAGS_NONE

    // grabbedBodies class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // maxNumObjectsPicked class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // maxMassOfObjectPicked class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    // maxDistOfObjectPicked class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // impulseAppliedWhenObjectNotPicked class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // throwVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // capturedObjectPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // capturedObjectsOffset class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkpGravityGun : hkpFirstPersonGun, IEquatable<hkpGravityGun?>
    {
        public IList<object> grabbedBodies { set; get; } = Array.Empty<object>();
        public int maxNumObjectsPicked { set; get; }
        public float maxMassOfObjectPicked { set; get; }
        public float maxDistOfObjectPicked { set; get; }
        public float impulseAppliedWhenObjectNotPicked { set; get; }
        public float throwVelocity { set; get; }
        public Vector4 capturedObjectPosition { set; get; }
        public Vector4 capturedObjectsOffset { set; get; }

        public override uint Signature { set; get; } = 0x5e2754cd;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyArray(br);
            maxNumObjectsPicked = br.ReadInt32();
            maxMassOfObjectPicked = br.ReadSingle();
            maxDistOfObjectPicked = br.ReadSingle();
            impulseAppliedWhenObjectNotPicked = br.ReadSingle();
            throwVelocity = br.ReadSingle();
            br.Position += 4;
            capturedObjectPosition = br.ReadVector4();
            capturedObjectsOffset = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidArray(bw);
            bw.WriteInt32(maxNumObjectsPicked);
            bw.WriteSingle(maxMassOfObjectPicked);
            bw.WriteSingle(maxDistOfObjectPicked);
            bw.WriteSingle(impulseAppliedWhenObjectNotPicked);
            bw.WriteSingle(throwVelocity);
            bw.Position += 4;
            bw.WriteVector4(capturedObjectPosition);
            bw.WriteVector4(capturedObjectsOffset);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            maxNumObjectsPicked = xd.ReadInt32(xe, nameof(maxNumObjectsPicked));
            maxMassOfObjectPicked = xd.ReadSingle(xe, nameof(maxMassOfObjectPicked));
            maxDistOfObjectPicked = xd.ReadSingle(xe, nameof(maxDistOfObjectPicked));
            impulseAppliedWhenObjectNotPicked = xd.ReadSingle(xe, nameof(impulseAppliedWhenObjectNotPicked));
            throwVelocity = xd.ReadSingle(xe, nameof(throwVelocity));
            capturedObjectPosition = xd.ReadVector4(xe, nameof(capturedObjectPosition));
            capturedObjectsOffset = xd.ReadVector4(xe, nameof(capturedObjectsOffset));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(grabbedBodies));
            xs.WriteNumber(xe, nameof(maxNumObjectsPicked), maxNumObjectsPicked);
            xs.WriteFloat(xe, nameof(maxMassOfObjectPicked), maxMassOfObjectPicked);
            xs.WriteFloat(xe, nameof(maxDistOfObjectPicked), maxDistOfObjectPicked);
            xs.WriteFloat(xe, nameof(impulseAppliedWhenObjectNotPicked), impulseAppliedWhenObjectNotPicked);
            xs.WriteFloat(xe, nameof(throwVelocity), throwVelocity);
            xs.WriteVector4(xe, nameof(capturedObjectPosition), capturedObjectPosition);
            xs.WriteVector4(xe, nameof(capturedObjectsOffset), capturedObjectsOffset);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpGravityGun);
        }

        public bool Equals(hkpGravityGun? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   maxNumObjectsPicked.Equals(other.maxNumObjectsPicked) &&
                   maxMassOfObjectPicked.Equals(other.maxMassOfObjectPicked) &&
                   maxDistOfObjectPicked.Equals(other.maxDistOfObjectPicked) &&
                   impulseAppliedWhenObjectNotPicked.Equals(other.impulseAppliedWhenObjectNotPicked) &&
                   throwVelocity.Equals(other.throwVelocity) &&
                   capturedObjectPosition.Equals(other.capturedObjectPosition) &&
                   capturedObjectsOffset.Equals(other.capturedObjectsOffset) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(maxNumObjectsPicked);
            hashcode.Add(maxMassOfObjectPicked);
            hashcode.Add(maxDistOfObjectPicked);
            hashcode.Add(impulseAppliedWhenObjectNotPicked);
            hashcode.Add(throwVelocity);
            hashcode.Add(capturedObjectPosition);
            hashcode.Add(capturedObjectsOffset);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

