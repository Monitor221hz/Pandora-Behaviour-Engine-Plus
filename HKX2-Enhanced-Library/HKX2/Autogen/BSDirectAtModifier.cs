using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // BSDirectAtModifier Signatire: 0x19a005c0 size: 224 flags: FLAGS_NONE

    // directAtTarget class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // sourceBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 82 flags: FLAGS_NONE enum: 
    // startBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // endBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 86 flags: FLAGS_NONE enum: 
    // limitHeadingDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // limitPitchDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // offsetHeadingDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // offsetPitchDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // onGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // offGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // targetLocation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // userInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // directAtCamera class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 132 flags: FLAGS_NONE enum: 
    // directAtCameraX class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // directAtCameraY class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 140 flags: FLAGS_NONE enum: 
    // directAtCameraZ class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // active class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 148 flags: FLAGS_NONE enum: 
    // currentHeadingOffset class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // currentPitchOffset class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 156 flags: FLAGS_NONE enum: 
    // timeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 160 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pSkeletonMemory class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 168 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // hasTarget class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 176 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // directAtTargetLocation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 192 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // boneChainIndices class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 208 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSDirectAtModifier : hkbModifier, IEquatable<BSDirectAtModifier?>
    {
        public bool directAtTarget { set; get; }
        public short sourceBoneIndex { set; get; }
        public short startBoneIndex { set; get; }
        public short endBoneIndex { set; get; }
        public float limitHeadingDegrees { set; get; }
        public float limitPitchDegrees { set; get; }
        public float offsetHeadingDegrees { set; get; }
        public float offsetPitchDegrees { set; get; }
        public float onGain { set; get; }
        public float offGain { set; get; }
        public Vector4 targetLocation { set; get; }
        public uint userInfo { set; get; }
        public bool directAtCamera { set; get; }
        public float directAtCameraX { set; get; }
        public float directAtCameraY { set; get; }
        public float directAtCameraZ { set; get; }
        public bool active { set; get; }
        public float currentHeadingOffset { set; get; }
        public float currentPitchOffset { set; get; }
        private float timeStep { set; get; }
        private object? pSkeletonMemory { set; get; }
        private bool hasTarget { set; get; }
        private Vector4 directAtTargetLocation { set; get; }
        public IList<object> boneChainIndices { set; get; } = Array.Empty<object>();

        public override uint Signature { set; get; } = 0x19a005c0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            directAtTarget = br.ReadBoolean();
            br.Position += 1;
            sourceBoneIndex = br.ReadInt16();
            startBoneIndex = br.ReadInt16();
            endBoneIndex = br.ReadInt16();
            limitHeadingDegrees = br.ReadSingle();
            limitPitchDegrees = br.ReadSingle();
            offsetHeadingDegrees = br.ReadSingle();
            offsetPitchDegrees = br.ReadSingle();
            onGain = br.ReadSingle();
            offGain = br.ReadSingle();
            targetLocation = br.ReadVector4();
            userInfo = br.ReadUInt32();
            directAtCamera = br.ReadBoolean();
            br.Position += 3;
            directAtCameraX = br.ReadSingle();
            directAtCameraY = br.ReadSingle();
            directAtCameraZ = br.ReadSingle();
            active = br.ReadBoolean();
            br.Position += 3;
            currentHeadingOffset = br.ReadSingle();
            currentPitchOffset = br.ReadSingle();
            timeStep = br.ReadSingle();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            hasTarget = br.ReadBoolean();
            br.Position += 15;
            directAtTargetLocation = br.ReadVector4();
            des.ReadEmptyArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(directAtTarget);
            bw.Position += 1;
            bw.WriteInt16(sourceBoneIndex);
            bw.WriteInt16(startBoneIndex);
            bw.WriteInt16(endBoneIndex);
            bw.WriteSingle(limitHeadingDegrees);
            bw.WriteSingle(limitPitchDegrees);
            bw.WriteSingle(offsetHeadingDegrees);
            bw.WriteSingle(offsetPitchDegrees);
            bw.WriteSingle(onGain);
            bw.WriteSingle(offGain);
            bw.WriteVector4(targetLocation);
            bw.WriteUInt32(userInfo);
            bw.WriteBoolean(directAtCamera);
            bw.Position += 3;
            bw.WriteSingle(directAtCameraX);
            bw.WriteSingle(directAtCameraY);
            bw.WriteSingle(directAtCameraZ);
            bw.WriteBoolean(active);
            bw.Position += 3;
            bw.WriteSingle(currentHeadingOffset);
            bw.WriteSingle(currentPitchOffset);
            bw.WriteSingle(timeStep);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            bw.WriteBoolean(hasTarget);
            bw.Position += 15;
            bw.WriteVector4(directAtTargetLocation);
            s.WriteVoidArray(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            directAtTarget = xd.ReadBoolean(xe, nameof(directAtTarget));
            sourceBoneIndex = xd.ReadInt16(xe, nameof(sourceBoneIndex));
            startBoneIndex = xd.ReadInt16(xe, nameof(startBoneIndex));
            endBoneIndex = xd.ReadInt16(xe, nameof(endBoneIndex));
            limitHeadingDegrees = xd.ReadSingle(xe, nameof(limitHeadingDegrees));
            limitPitchDegrees = xd.ReadSingle(xe, nameof(limitPitchDegrees));
            offsetHeadingDegrees = xd.ReadSingle(xe, nameof(offsetHeadingDegrees));
            offsetPitchDegrees = xd.ReadSingle(xe, nameof(offsetPitchDegrees));
            onGain = xd.ReadSingle(xe, nameof(onGain));
            offGain = xd.ReadSingle(xe, nameof(offGain));
            targetLocation = xd.ReadVector4(xe, nameof(targetLocation));
            userInfo = xd.ReadUInt32(xe, nameof(userInfo));
            directAtCamera = xd.ReadBoolean(xe, nameof(directAtCamera));
            directAtCameraX = xd.ReadSingle(xe, nameof(directAtCameraX));
            directAtCameraY = xd.ReadSingle(xe, nameof(directAtCameraY));
            directAtCameraZ = xd.ReadSingle(xe, nameof(directAtCameraZ));
            active = xd.ReadBoolean(xe, nameof(active));
            currentHeadingOffset = xd.ReadSingle(xe, nameof(currentHeadingOffset));
            currentPitchOffset = xd.ReadSingle(xe, nameof(currentPitchOffset));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(directAtTarget), directAtTarget);
            xs.WriteNumber(xe, nameof(sourceBoneIndex), sourceBoneIndex);
            xs.WriteNumber(xe, nameof(startBoneIndex), startBoneIndex);
            xs.WriteNumber(xe, nameof(endBoneIndex), endBoneIndex);
            xs.WriteFloat(xe, nameof(limitHeadingDegrees), limitHeadingDegrees);
            xs.WriteFloat(xe, nameof(limitPitchDegrees), limitPitchDegrees);
            xs.WriteFloat(xe, nameof(offsetHeadingDegrees), offsetHeadingDegrees);
            xs.WriteFloat(xe, nameof(offsetPitchDegrees), offsetPitchDegrees);
            xs.WriteFloat(xe, nameof(onGain), onGain);
            xs.WriteFloat(xe, nameof(offGain), offGain);
            xs.WriteVector4(xe, nameof(targetLocation), targetLocation);
            xs.WriteNumber(xe, nameof(userInfo), userInfo);
            xs.WriteBoolean(xe, nameof(directAtCamera), directAtCamera);
            xs.WriteFloat(xe, nameof(directAtCameraX), directAtCameraX);
            xs.WriteFloat(xe, nameof(directAtCameraY), directAtCameraY);
            xs.WriteFloat(xe, nameof(directAtCameraZ), directAtCameraZ);
            xs.WriteBoolean(xe, nameof(active), active);
            xs.WriteFloat(xe, nameof(currentHeadingOffset), currentHeadingOffset);
            xs.WriteFloat(xe, nameof(currentPitchOffset), currentPitchOffset);
            xs.WriteSerializeIgnored(xe, nameof(timeStep));
            xs.WriteSerializeIgnored(xe, nameof(pSkeletonMemory));
            xs.WriteSerializeIgnored(xe, nameof(hasTarget));
            xs.WriteSerializeIgnored(xe, nameof(directAtTargetLocation));
            xs.WriteSerializeIgnored(xe, nameof(boneChainIndices));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSDirectAtModifier);
        }

        public bool Equals(BSDirectAtModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   directAtTarget.Equals(other.directAtTarget) &&
                   sourceBoneIndex.Equals(other.sourceBoneIndex) &&
                   startBoneIndex.Equals(other.startBoneIndex) &&
                   endBoneIndex.Equals(other.endBoneIndex) &&
                   limitHeadingDegrees.Equals(other.limitHeadingDegrees) &&
                   limitPitchDegrees.Equals(other.limitPitchDegrees) &&
                   offsetHeadingDegrees.Equals(other.offsetHeadingDegrees) &&
                   offsetPitchDegrees.Equals(other.offsetPitchDegrees) &&
                   onGain.Equals(other.onGain) &&
                   offGain.Equals(other.offGain) &&
                   targetLocation.Equals(other.targetLocation) &&
                   userInfo.Equals(other.userInfo) &&
                   directAtCamera.Equals(other.directAtCamera) &&
                   directAtCameraX.Equals(other.directAtCameraX) &&
                   directAtCameraY.Equals(other.directAtCameraY) &&
                   directAtCameraZ.Equals(other.directAtCameraZ) &&
                   active.Equals(other.active) &&
                   currentHeadingOffset.Equals(other.currentHeadingOffset) &&
                   currentPitchOffset.Equals(other.currentPitchOffset) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(directAtTarget);
            hashcode.Add(sourceBoneIndex);
            hashcode.Add(startBoneIndex);
            hashcode.Add(endBoneIndex);
            hashcode.Add(limitHeadingDegrees);
            hashcode.Add(limitPitchDegrees);
            hashcode.Add(offsetHeadingDegrees);
            hashcode.Add(offsetPitchDegrees);
            hashcode.Add(onGain);
            hashcode.Add(offGain);
            hashcode.Add(targetLocation);
            hashcode.Add(userInfo);
            hashcode.Add(directAtCamera);
            hashcode.Add(directAtCameraX);
            hashcode.Add(directAtCameraY);
            hashcode.Add(directAtCameraZ);
            hashcode.Add(active);
            hashcode.Add(currentHeadingOffset);
            hashcode.Add(currentPitchOffset);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

