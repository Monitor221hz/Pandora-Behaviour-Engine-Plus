using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbLookAtModifier Signatire: 0x3d28e066 size: 240 flags: FLAGS_NONE

    // targetWS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // headForwardLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // neckForwardLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // neckRightLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // eyePositionHS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // newTargetGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // onGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 164 flags: FLAGS_NONE enum: 
    // offGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // limitAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 172 flags: FLAGS_NONE enum: 
    // limitAngleLeft class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // limitAngleRight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 180 flags: FLAGS_NONE enum: 
    // limitAngleUp class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // limitAngleDown class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 188 flags: FLAGS_NONE enum: 
    // headIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 192 flags: FLAGS_NONE enum: 
    // neckIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 194 flags: FLAGS_NONE enum: 
    // isOn class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 196 flags: FLAGS_NONE enum: 
    // individualLimitsOn class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 197 flags: FLAGS_NONE enum: 
    // isTargetInsideLimitCone class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 198 flags: FLAGS_NONE enum: 
    // lookAtLastTargetWS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 208 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // lookAtWeight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbLookAtModifier : hkbModifier, IEquatable<hkbLookAtModifier?>
    {
        public Vector4 targetWS { set; get; }
        public Vector4 headForwardLS { set; get; }
        public Vector4 neckForwardLS { set; get; }
        public Vector4 neckRightLS { set; get; }
        public Vector4 eyePositionHS { set; get; }
        public float newTargetGain { set; get; }
        public float onGain { set; get; }
        public float offGain { set; get; }
        public float limitAngleDegrees { set; get; }
        public float limitAngleLeft { set; get; }
        public float limitAngleRight { set; get; }
        public float limitAngleUp { set; get; }
        public float limitAngleDown { set; get; }
        public short headIndex { set; get; }
        public short neckIndex { set; get; }
        public bool isOn { set; get; }
        public bool individualLimitsOn { set; get; }
        public bool isTargetInsideLimitCone { set; get; }
        private Vector4 lookAtLastTargetWS { set; get; }
        private float lookAtWeight { set; get; }

        public override uint Signature { set; get; } = 0x3d28e066;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            targetWS = br.ReadVector4();
            headForwardLS = br.ReadVector4();
            neckForwardLS = br.ReadVector4();
            neckRightLS = br.ReadVector4();
            eyePositionHS = br.ReadVector4();
            newTargetGain = br.ReadSingle();
            onGain = br.ReadSingle();
            offGain = br.ReadSingle();
            limitAngleDegrees = br.ReadSingle();
            limitAngleLeft = br.ReadSingle();
            limitAngleRight = br.ReadSingle();
            limitAngleUp = br.ReadSingle();
            limitAngleDown = br.ReadSingle();
            headIndex = br.ReadInt16();
            neckIndex = br.ReadInt16();
            isOn = br.ReadBoolean();
            individualLimitsOn = br.ReadBoolean();
            isTargetInsideLimitCone = br.ReadBoolean();
            br.Position += 9;
            lookAtLastTargetWS = br.ReadVector4();
            lookAtWeight = br.ReadSingle();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(targetWS);
            bw.WriteVector4(headForwardLS);
            bw.WriteVector4(neckForwardLS);
            bw.WriteVector4(neckRightLS);
            bw.WriteVector4(eyePositionHS);
            bw.WriteSingle(newTargetGain);
            bw.WriteSingle(onGain);
            bw.WriteSingle(offGain);
            bw.WriteSingle(limitAngleDegrees);
            bw.WriteSingle(limitAngleLeft);
            bw.WriteSingle(limitAngleRight);
            bw.WriteSingle(limitAngleUp);
            bw.WriteSingle(limitAngleDown);
            bw.WriteInt16(headIndex);
            bw.WriteInt16(neckIndex);
            bw.WriteBoolean(isOn);
            bw.WriteBoolean(individualLimitsOn);
            bw.WriteBoolean(isTargetInsideLimitCone);
            bw.Position += 9;
            bw.WriteVector4(lookAtLastTargetWS);
            bw.WriteSingle(lookAtWeight);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            targetWS = xd.ReadVector4(xe, nameof(targetWS));
            headForwardLS = xd.ReadVector4(xe, nameof(headForwardLS));
            neckForwardLS = xd.ReadVector4(xe, nameof(neckForwardLS));
            neckRightLS = xd.ReadVector4(xe, nameof(neckRightLS));
            eyePositionHS = xd.ReadVector4(xe, nameof(eyePositionHS));
            newTargetGain = xd.ReadSingle(xe, nameof(newTargetGain));
            onGain = xd.ReadSingle(xe, nameof(onGain));
            offGain = xd.ReadSingle(xe, nameof(offGain));
            limitAngleDegrees = xd.ReadSingle(xe, nameof(limitAngleDegrees));
            limitAngleLeft = xd.ReadSingle(xe, nameof(limitAngleLeft));
            limitAngleRight = xd.ReadSingle(xe, nameof(limitAngleRight));
            limitAngleUp = xd.ReadSingle(xe, nameof(limitAngleUp));
            limitAngleDown = xd.ReadSingle(xe, nameof(limitAngleDown));
            headIndex = xd.ReadInt16(xe, nameof(headIndex));
            neckIndex = xd.ReadInt16(xe, nameof(neckIndex));
            isOn = xd.ReadBoolean(xe, nameof(isOn));
            individualLimitsOn = xd.ReadBoolean(xe, nameof(individualLimitsOn));
            isTargetInsideLimitCone = xd.ReadBoolean(xe, nameof(isTargetInsideLimitCone));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(targetWS), targetWS);
            xs.WriteVector4(xe, nameof(headForwardLS), headForwardLS);
            xs.WriteVector4(xe, nameof(neckForwardLS), neckForwardLS);
            xs.WriteVector4(xe, nameof(neckRightLS), neckRightLS);
            xs.WriteVector4(xe, nameof(eyePositionHS), eyePositionHS);
            xs.WriteFloat(xe, nameof(newTargetGain), newTargetGain);
            xs.WriteFloat(xe, nameof(onGain), onGain);
            xs.WriteFloat(xe, nameof(offGain), offGain);
            xs.WriteFloat(xe, nameof(limitAngleDegrees), limitAngleDegrees);
            xs.WriteFloat(xe, nameof(limitAngleLeft), limitAngleLeft);
            xs.WriteFloat(xe, nameof(limitAngleRight), limitAngleRight);
            xs.WriteFloat(xe, nameof(limitAngleUp), limitAngleUp);
            xs.WriteFloat(xe, nameof(limitAngleDown), limitAngleDown);
            xs.WriteNumber(xe, nameof(headIndex), headIndex);
            xs.WriteNumber(xe, nameof(neckIndex), neckIndex);
            xs.WriteBoolean(xe, nameof(isOn), isOn);
            xs.WriteBoolean(xe, nameof(individualLimitsOn), individualLimitsOn);
            xs.WriteBoolean(xe, nameof(isTargetInsideLimitCone), isTargetInsideLimitCone);
            xs.WriteSerializeIgnored(xe, nameof(lookAtLastTargetWS));
            xs.WriteSerializeIgnored(xe, nameof(lookAtWeight));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbLookAtModifier);
        }

        public bool Equals(hkbLookAtModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   targetWS.Equals(other.targetWS) &&
                   headForwardLS.Equals(other.headForwardLS) &&
                   neckForwardLS.Equals(other.neckForwardLS) &&
                   neckRightLS.Equals(other.neckRightLS) &&
                   eyePositionHS.Equals(other.eyePositionHS) &&
                   newTargetGain.Equals(other.newTargetGain) &&
                   onGain.Equals(other.onGain) &&
                   offGain.Equals(other.offGain) &&
                   limitAngleDegrees.Equals(other.limitAngleDegrees) &&
                   limitAngleLeft.Equals(other.limitAngleLeft) &&
                   limitAngleRight.Equals(other.limitAngleRight) &&
                   limitAngleUp.Equals(other.limitAngleUp) &&
                   limitAngleDown.Equals(other.limitAngleDown) &&
                   headIndex.Equals(other.headIndex) &&
                   neckIndex.Equals(other.neckIndex) &&
                   isOn.Equals(other.isOn) &&
                   individualLimitsOn.Equals(other.individualLimitsOn) &&
                   isTargetInsideLimitCone.Equals(other.isTargetInsideLimitCone) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(targetWS);
            hashcode.Add(headForwardLS);
            hashcode.Add(neckForwardLS);
            hashcode.Add(neckRightLS);
            hashcode.Add(eyePositionHS);
            hashcode.Add(newTargetGain);
            hashcode.Add(onGain);
            hashcode.Add(offGain);
            hashcode.Add(limitAngleDegrees);
            hashcode.Add(limitAngleLeft);
            hashcode.Add(limitAngleRight);
            hashcode.Add(limitAngleUp);
            hashcode.Add(limitAngleDown);
            hashcode.Add(headIndex);
            hashcode.Add(neckIndex);
            hashcode.Add(isOn);
            hashcode.Add(individualLimitsOn);
            hashcode.Add(isTargetInsideLimitCone);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

