using System;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMotion Signatire: 0x98aadb4f size: 320 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: MotionType
    // deactivationIntegrateCounter class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 17 flags: FLAGS_NONE enum: 
    // deactivationNumInactiveFrames class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 2 offset: 18 flags: FLAGS_NONE enum: 
    // motionState class: hkMotionState Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // inertiaAndMassInv class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 208 flags: FLAGS_NONE enum: 
    // linearVelocity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 224 flags: FLAGS_NONE enum: 
    // angularVelocity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 240 flags: FLAGS_NONE enum: 
    // deactivationRefPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 2 offset: 256 flags: FLAGS_NONE enum: 
    // deactivationRefOrientation class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 2 offset: 288 flags: FLAGS_NONE enum: 
    // savedMotion class: hkpMaxSizeMotion Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 296 flags: FLAGS_NONE enum: 
    // savedQualityTypeIndex class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 304 flags: FLAGS_NONE enum: 
    // gravityFactor class:  Type.TYPE_HALF Type.TYPE_VOID arrSize: 0 offset: 306 flags: FLAGS_NONE enum: 
    public partial class hkpMotion : hkReferencedObject, IEquatable<hkpMotion?>
    {
        public byte type { set; get; }
        public byte deactivationIntegrateCounter { set; get; }
        public ushort[] deactivationNumInactiveFrames = new ushort[2];
        public hkMotionState motionState { set; get; } = new();
        public Vector4 inertiaAndMassInv { set; get; }
        public Vector4 linearVelocity { set; get; }
        public Vector4 angularVelocity { set; get; }
        public Vector4[] deactivationRefPosition = new Vector4[2];
        public uint[] deactivationRefOrientation = new uint[2];
        public hkpMaxSizeMotion? savedMotion { set; get; }
        public ushort savedQualityTypeIndex { set; get; }
        public Half gravityFactor { set; get; }

        public override uint Signature { set; get; } = 0x98aadb4f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            type = br.ReadByte();
            deactivationIntegrateCounter = br.ReadByte();
            deactivationNumInactiveFrames = des.ReadUInt16CStyleArray(br, 2);
            br.Position += 10;
            motionState.Read(des, br);
            inertiaAndMassInv = br.ReadVector4();
            linearVelocity = br.ReadVector4();
            angularVelocity = br.ReadVector4();
            deactivationRefPosition = des.ReadVector4CStyleArray(br, 2);
            deactivationRefOrientation = des.ReadUInt32CStyleArray(br, 2);
            savedMotion = des.ReadClassPointer<hkpMaxSizeMotion>(br);
            savedQualityTypeIndex = br.ReadUInt16();
            gravityFactor = br.ReadHalf();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(type);
            bw.WriteByte(deactivationIntegrateCounter);
            s.WriteUInt16CStyleArray(bw, deactivationNumInactiveFrames);
            bw.Position += 10;
            motionState.Write(s, bw);
            bw.WriteVector4(inertiaAndMassInv);
            bw.WriteVector4(linearVelocity);
            bw.WriteVector4(angularVelocity);
            s.WriteVector4CStyleArray(bw, deactivationRefPosition);
            s.WriteUInt32CStyleArray(bw, deactivationRefOrientation);
            s.WriteClassPointer(bw, savedMotion);
            bw.WriteUInt16(savedQualityTypeIndex);
            bw.WriteHalf(gravityFactor);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            type = xd.ReadFlag<MotionType, byte>(xe, nameof(type));
            deactivationIntegrateCounter = xd.ReadByte(xe, nameof(deactivationIntegrateCounter));
            deactivationNumInactiveFrames = xd.ReadUInt16CStyleArray(xe, nameof(deactivationNumInactiveFrames), 2);
            motionState = xd.ReadClass<hkMotionState>(xe, nameof(motionState));
            inertiaAndMassInv = xd.ReadVector4(xe, nameof(inertiaAndMassInv));
            linearVelocity = xd.ReadVector4(xe, nameof(linearVelocity));
            angularVelocity = xd.ReadVector4(xe, nameof(angularVelocity));
            deactivationRefPosition = xd.ReadVector4CStyleArray(xe, nameof(deactivationRefPosition), 2);
            deactivationRefOrientation = xd.ReadUInt32CStyleArray(xe, nameof(deactivationRefOrientation), 2);
            savedMotion = xd.ReadClassPointer<hkpMaxSizeMotion>(this, xe, nameof(savedMotion));
            savedQualityTypeIndex = xd.ReadUInt16(xe, nameof(savedQualityTypeIndex));
            gravityFactor = xd.ReadHalf(xe, nameof(gravityFactor));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<MotionType, byte>(xe, nameof(type), type);
            xs.WriteNumber(xe, nameof(deactivationIntegrateCounter), deactivationIntegrateCounter);
            xs.WriteNumberArray(xe, nameof(deactivationNumInactiveFrames), deactivationNumInactiveFrames);
            xs.WriteClass<hkMotionState>(xe, nameof(motionState), motionState);
            xs.WriteVector4(xe, nameof(inertiaAndMassInv), inertiaAndMassInv);
            xs.WriteVector4(xe, nameof(linearVelocity), linearVelocity);
            xs.WriteVector4(xe, nameof(angularVelocity), angularVelocity);
            xs.WriteVector4Array(xe, nameof(deactivationRefPosition), deactivationRefPosition);
            xs.WriteNumberArray(xe, nameof(deactivationRefOrientation), deactivationRefOrientation);
            xs.WriteClassPointer(xe, nameof(savedMotion), savedMotion);
            xs.WriteNumber(xe, nameof(savedQualityTypeIndex), savedQualityTypeIndex);
            xs.WriteFloat(xe, nameof(gravityFactor), gravityFactor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMotion);
        }

        public bool Equals(hkpMotion? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   type.Equals(other.type) &&
                   deactivationIntegrateCounter.Equals(other.deactivationIntegrateCounter) &&
                   deactivationNumInactiveFrames.SequenceEqual(other.deactivationNumInactiveFrames) &&
                   ((motionState is null && other.motionState is null) || (motionState is not null && other.motionState is not null && motionState.Equals((IHavokObject)other.motionState))) &&
                   inertiaAndMassInv.Equals(other.inertiaAndMassInv) &&
                   linearVelocity.Equals(other.linearVelocity) &&
                   angularVelocity.Equals(other.angularVelocity) &&
                   deactivationRefPosition.SequenceEqual(other.deactivationRefPosition) &&
                   deactivationRefOrientation.SequenceEqual(other.deactivationRefOrientation) &&
                   ((savedMotion is null && other.savedMotion is null) || (savedMotion is not null && other.savedMotion is not null && savedMotion.Equals((IHavokObject)other.savedMotion))) &&
                   savedQualityTypeIndex.Equals(other.savedQualityTypeIndex) &&
                   gravityFactor.Equals(other.gravityFactor) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(type);
            hashcode.Add(deactivationIntegrateCounter);
            hashcode.Add(deactivationNumInactiveFrames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(motionState);
            hashcode.Add(inertiaAndMassInv);
            hashcode.Add(linearVelocity);
            hashcode.Add(angularVelocity);
            hashcode.Add(deactivationRefPosition.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(deactivationRefOrientation.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(savedMotion);
            hashcode.Add(savedQualityTypeIndex);
            hashcode.Add(gravityFactor);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

