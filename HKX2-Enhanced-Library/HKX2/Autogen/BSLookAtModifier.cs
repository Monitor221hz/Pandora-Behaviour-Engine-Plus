using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // BSLookAtModifier Signatire: 0xd756fc25 size: 224 flags: FLAGS_NONE

    // lookAtTarget class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // bones class: BSLookAtModifierBoneData Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // eyeBones class: BSLookAtModifierBoneData Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // limitAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // limitAngleThresholdDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 124 flags: FLAGS_NONE enum: 
    // continueLookOutsideOfLimit class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // onGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 132 flags: FLAGS_NONE enum: 
    // offGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // useBoneGains class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 140 flags: FLAGS_NONE enum: 
    // targetLocation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // targetOutsideLimits class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // targetOutOfLimitEvent class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // lookAtCamera class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // lookAtCameraX class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 188 flags: FLAGS_NONE enum: 
    // lookAtCameraY class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 192 flags: FLAGS_NONE enum: 
    // lookAtCameraZ class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 196 flags: FLAGS_NONE enum: 
    // timeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 200 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // ballBonesValid class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 204 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pSkeletonMemory class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 208 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSLookAtModifier : hkbModifier, IEquatable<BSLookAtModifier?>
    {
        public bool lookAtTarget { set; get; }
        public IList<BSLookAtModifierBoneData> bones { set; get; } = Array.Empty<BSLookAtModifierBoneData>();
        public IList<BSLookAtModifierBoneData> eyeBones { set; get; } = Array.Empty<BSLookAtModifierBoneData>();
        public float limitAngleDegrees { set; get; }
        public float limitAngleThresholdDegrees { set; get; }
        public bool continueLookOutsideOfLimit { set; get; }
        public float onGain { set; get; }
        public float offGain { set; get; }
        public bool useBoneGains { set; get; }
        public Vector4 targetLocation { set; get; }
        public bool targetOutsideLimits { set; get; }
        public hkbEventProperty targetOutOfLimitEvent { set; get; } = new();
        public bool lookAtCamera { set; get; }
        public float lookAtCameraX { set; get; }
        public float lookAtCameraY { set; get; }
        public float lookAtCameraZ { set; get; }
        private float timeStep { set; get; }
        private bool ballBonesValid { set; get; }
        private object? pSkeletonMemory { set; get; }

        public override uint Signature { set; get; } = 0xd756fc25;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            lookAtTarget = br.ReadBoolean();
            br.Position += 7;
            bones = des.ReadClassArray<BSLookAtModifierBoneData>(br);
            eyeBones = des.ReadClassArray<BSLookAtModifierBoneData>(br);
            limitAngleDegrees = br.ReadSingle();
            limitAngleThresholdDegrees = br.ReadSingle();
            continueLookOutsideOfLimit = br.ReadBoolean();
            br.Position += 3;
            onGain = br.ReadSingle();
            offGain = br.ReadSingle();
            useBoneGains = br.ReadBoolean();
            br.Position += 3;
            targetLocation = br.ReadVector4();
            targetOutsideLimits = br.ReadBoolean();
            br.Position += 7;
            targetOutOfLimitEvent.Read(des, br);
            lookAtCamera = br.ReadBoolean();
            br.Position += 3;
            lookAtCameraX = br.ReadSingle();
            lookAtCameraY = br.ReadSingle();
            lookAtCameraZ = br.ReadSingle();
            timeStep = br.ReadSingle();
            ballBonesValid = br.ReadBoolean();
            br.Position += 3;
            des.ReadEmptyPointer(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(lookAtTarget);
            bw.Position += 7;
            s.WriteClassArray(bw, bones);
            s.WriteClassArray(bw, eyeBones);
            bw.WriteSingle(limitAngleDegrees);
            bw.WriteSingle(limitAngleThresholdDegrees);
            bw.WriteBoolean(continueLookOutsideOfLimit);
            bw.Position += 3;
            bw.WriteSingle(onGain);
            bw.WriteSingle(offGain);
            bw.WriteBoolean(useBoneGains);
            bw.Position += 3;
            bw.WriteVector4(targetLocation);
            bw.WriteBoolean(targetOutsideLimits);
            bw.Position += 7;
            targetOutOfLimitEvent.Write(s, bw);
            bw.WriteBoolean(lookAtCamera);
            bw.Position += 3;
            bw.WriteSingle(lookAtCameraX);
            bw.WriteSingle(lookAtCameraY);
            bw.WriteSingle(lookAtCameraZ);
            bw.WriteSingle(timeStep);
            bw.WriteBoolean(ballBonesValid);
            bw.Position += 3;
            s.WriteVoidPointer(bw);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            lookAtTarget = xd.ReadBoolean(xe, nameof(lookAtTarget));
            bones = xd.ReadClassArray<BSLookAtModifierBoneData>(xe, nameof(bones));
            eyeBones = xd.ReadClassArray<BSLookAtModifierBoneData>(xe, nameof(eyeBones));
            limitAngleDegrees = xd.ReadSingle(xe, nameof(limitAngleDegrees));
            limitAngleThresholdDegrees = xd.ReadSingle(xe, nameof(limitAngleThresholdDegrees));
            continueLookOutsideOfLimit = xd.ReadBoolean(xe, nameof(continueLookOutsideOfLimit));
            onGain = xd.ReadSingle(xe, nameof(onGain));
            offGain = xd.ReadSingle(xe, nameof(offGain));
            useBoneGains = xd.ReadBoolean(xe, nameof(useBoneGains));
            targetLocation = xd.ReadVector4(xe, nameof(targetLocation));
            targetOutsideLimits = xd.ReadBoolean(xe, nameof(targetOutsideLimits));
            targetOutOfLimitEvent = xd.ReadClass<hkbEventProperty>(xe, nameof(targetOutOfLimitEvent));
            lookAtCamera = xd.ReadBoolean(xe, nameof(lookAtCamera));
            lookAtCameraX = xd.ReadSingle(xe, nameof(lookAtCameraX));
            lookAtCameraY = xd.ReadSingle(xe, nameof(lookAtCameraY));
            lookAtCameraZ = xd.ReadSingle(xe, nameof(lookAtCameraZ));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(lookAtTarget), lookAtTarget);
            xs.WriteClassArray(xe, nameof(bones), bones);
            xs.WriteClassArray(xe, nameof(eyeBones), eyeBones);
            xs.WriteFloat(xe, nameof(limitAngleDegrees), limitAngleDegrees);
            xs.WriteFloat(xe, nameof(limitAngleThresholdDegrees), limitAngleThresholdDegrees);
            xs.WriteBoolean(xe, nameof(continueLookOutsideOfLimit), continueLookOutsideOfLimit);
            xs.WriteFloat(xe, nameof(onGain), onGain);
            xs.WriteFloat(xe, nameof(offGain), offGain);
            xs.WriteBoolean(xe, nameof(useBoneGains), useBoneGains);
            xs.WriteVector4(xe, nameof(targetLocation), targetLocation);
            xs.WriteBoolean(xe, nameof(targetOutsideLimits), targetOutsideLimits);
            xs.WriteClass<hkbEventProperty>(xe, nameof(targetOutOfLimitEvent), targetOutOfLimitEvent);
            xs.WriteBoolean(xe, nameof(lookAtCamera), lookAtCamera);
            xs.WriteFloat(xe, nameof(lookAtCameraX), lookAtCameraX);
            xs.WriteFloat(xe, nameof(lookAtCameraY), lookAtCameraY);
            xs.WriteFloat(xe, nameof(lookAtCameraZ), lookAtCameraZ);
            xs.WriteSerializeIgnored(xe, nameof(timeStep));
            xs.WriteSerializeIgnored(xe, nameof(ballBonesValid));
            xs.WriteSerializeIgnored(xe, nameof(pSkeletonMemory));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSLookAtModifier);
        }

        public bool Equals(BSLookAtModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   lookAtTarget.Equals(other.lookAtTarget) &&
                   bones.SequenceEqual(other.bones) &&
                   eyeBones.SequenceEqual(other.eyeBones) &&
                   limitAngleDegrees.Equals(other.limitAngleDegrees) &&
                   limitAngleThresholdDegrees.Equals(other.limitAngleThresholdDegrees) &&
                   continueLookOutsideOfLimit.Equals(other.continueLookOutsideOfLimit) &&
                   onGain.Equals(other.onGain) &&
                   offGain.Equals(other.offGain) &&
                   useBoneGains.Equals(other.useBoneGains) &&
                   targetLocation.Equals(other.targetLocation) &&
                   targetOutsideLimits.Equals(other.targetOutsideLimits) &&
                   ((targetOutOfLimitEvent is null && other.targetOutOfLimitEvent is null) || (targetOutOfLimitEvent is not null && other.targetOutOfLimitEvent is not null && targetOutOfLimitEvent.Equals((IHavokObject)other.targetOutOfLimitEvent))) &&
                   lookAtCamera.Equals(other.lookAtCamera) &&
                   lookAtCameraX.Equals(other.lookAtCameraX) &&
                   lookAtCameraY.Equals(other.lookAtCameraY) &&
                   lookAtCameraZ.Equals(other.lookAtCameraZ) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(lookAtTarget);
            hashcode.Add(bones.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(eyeBones.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(limitAngleDegrees);
            hashcode.Add(limitAngleThresholdDegrees);
            hashcode.Add(continueLookOutsideOfLimit);
            hashcode.Add(onGain);
            hashcode.Add(offGain);
            hashcode.Add(useBoneGains);
            hashcode.Add(targetLocation);
            hashcode.Add(targetOutsideLimits);
            hashcode.Add(targetOutOfLimitEvent);
            hashcode.Add(lookAtCamera);
            hashcode.Add(lookAtCameraX);
            hashcode.Add(lookAtCameraY);
            hashcode.Add(lookAtCameraZ);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

