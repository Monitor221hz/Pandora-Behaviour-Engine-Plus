using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbFootIkModifier Signatire: 0xed8966c0 size: 256 flags: FLAGS_NONE

    // gains class: hkbFootIkGains Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // legs class: hkbFootIkModifierLeg Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // raycastDistanceUp class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // raycastDistanceDown class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 148 flags: FLAGS_NONE enum: 
    // originalGroundHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // errorOut class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 156 flags: FLAGS_NONE enum: 
    // errorOutTranslation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // alignWithGroundRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // verticalOffset class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 192 flags: FLAGS_NONE enum: 
    // collisionFilterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 196 flags: FLAGS_NONE enum: 
    // forwardAlignFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 200 flags: FLAGS_NONE enum: 
    // sidewaysAlignFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 204 flags: FLAGS_NONE enum: 
    // sidewaysSampleWidth class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 208 flags: FLAGS_NONE enum: 
    // useTrackData class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 212 flags: FLAGS_NONE enum: 
    // lockFeetWhenPlanted class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 213 flags: FLAGS_NONE enum: 
    // useCharacterUpVector class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 214 flags: FLAGS_NONE enum: 
    // alignMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 215 flags: FLAGS_NONE enum: AlignMode
    // internalLegData class: hkbFootIkModifierInternalLegData Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 216 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // prevIsFootIkEnabled class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 232 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // isSetUp class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 236 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // isGroundPositionValid class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 237 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 240 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbFootIkModifier : hkbModifier, IEquatable<hkbFootIkModifier?>
    {
        public hkbFootIkGains gains { set; get; } = new();
        public IList<hkbFootIkModifierLeg> legs { set; get; } = Array.Empty<hkbFootIkModifierLeg>();
        public float raycastDistanceUp { set; get; }
        public float raycastDistanceDown { set; get; }
        public float originalGroundHeightMS { set; get; }
        public float errorOut { set; get; }
        public Vector4 errorOutTranslation { set; get; }
        public Quaternion alignWithGroundRotation { set; get; }
        public float verticalOffset { set; get; }
        public uint collisionFilterInfo { set; get; }
        public float forwardAlignFraction { set; get; }
        public float sidewaysAlignFraction { set; get; }
        public float sidewaysSampleWidth { set; get; }
        public bool useTrackData { set; get; }
        public bool lockFeetWhenPlanted { set; get; }
        public bool useCharacterUpVector { set; get; }
        public sbyte alignMode { set; get; }
        public IList<hkbFootIkModifierInternalLegData> internalLegData { set; get; } = Array.Empty<hkbFootIkModifierInternalLegData>();
        private float prevIsFootIkEnabled { set; get; }
        private bool isSetUp { set; get; }
        private bool isGroundPositionValid { set; get; }
        private float timeStep { set; get; }

        public override uint Signature { set; get; } = 0xed8966c0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            gains.Read(des, br);
            legs = des.ReadClassArray<hkbFootIkModifierLeg>(br);
            raycastDistanceUp = br.ReadSingle();
            raycastDistanceDown = br.ReadSingle();
            originalGroundHeightMS = br.ReadSingle();
            errorOut = br.ReadSingle();
            errorOutTranslation = br.ReadVector4();
            alignWithGroundRotation = des.ReadQuaternion(br);
            verticalOffset = br.ReadSingle();
            collisionFilterInfo = br.ReadUInt32();
            forwardAlignFraction = br.ReadSingle();
            sidewaysAlignFraction = br.ReadSingle();
            sidewaysSampleWidth = br.ReadSingle();
            useTrackData = br.ReadBoolean();
            lockFeetWhenPlanted = br.ReadBoolean();
            useCharacterUpVector = br.ReadBoolean();
            alignMode = br.ReadSByte();
            internalLegData = des.ReadClassArray<hkbFootIkModifierInternalLegData>(br);
            prevIsFootIkEnabled = br.ReadSingle();
            isSetUp = br.ReadBoolean();
            isGroundPositionValid = br.ReadBoolean();
            br.Position += 2;
            timeStep = br.ReadSingle();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            gains.Write(s, bw);
            s.WriteClassArray(bw, legs);
            bw.WriteSingle(raycastDistanceUp);
            bw.WriteSingle(raycastDistanceDown);
            bw.WriteSingle(originalGroundHeightMS);
            bw.WriteSingle(errorOut);
            bw.WriteVector4(errorOutTranslation);
            s.WriteQuaternion(bw, alignWithGroundRotation);
            bw.WriteSingle(verticalOffset);
            bw.WriteUInt32(collisionFilterInfo);
            bw.WriteSingle(forwardAlignFraction);
            bw.WriteSingle(sidewaysAlignFraction);
            bw.WriteSingle(sidewaysSampleWidth);
            bw.WriteBoolean(useTrackData);
            bw.WriteBoolean(lockFeetWhenPlanted);
            bw.WriteBoolean(useCharacterUpVector);
            bw.WriteSByte(alignMode);
            s.WriteClassArray(bw, internalLegData);
            bw.WriteSingle(prevIsFootIkEnabled);
            bw.WriteBoolean(isSetUp);
            bw.WriteBoolean(isGroundPositionValid);
            bw.Position += 2;
            bw.WriteSingle(timeStep);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            gains = xd.ReadClass<hkbFootIkGains>(xe, nameof(gains));
            legs = xd.ReadClassArray<hkbFootIkModifierLeg>(xe, nameof(legs));
            raycastDistanceUp = xd.ReadSingle(xe, nameof(raycastDistanceUp));
            raycastDistanceDown = xd.ReadSingle(xe, nameof(raycastDistanceDown));
            originalGroundHeightMS = xd.ReadSingle(xe, nameof(originalGroundHeightMS));
            errorOut = xd.ReadSingle(xe, nameof(errorOut));
            errorOutTranslation = xd.ReadVector4(xe, nameof(errorOutTranslation));
            alignWithGroundRotation = xd.ReadQuaternion(xe, nameof(alignWithGroundRotation));
            verticalOffset = xd.ReadSingle(xe, nameof(verticalOffset));
            collisionFilterInfo = xd.ReadUInt32(xe, nameof(collisionFilterInfo));
            forwardAlignFraction = xd.ReadSingle(xe, nameof(forwardAlignFraction));
            sidewaysAlignFraction = xd.ReadSingle(xe, nameof(sidewaysAlignFraction));
            sidewaysSampleWidth = xd.ReadSingle(xe, nameof(sidewaysSampleWidth));
            useTrackData = xd.ReadBoolean(xe, nameof(useTrackData));
            lockFeetWhenPlanted = xd.ReadBoolean(xe, nameof(lockFeetWhenPlanted));
            useCharacterUpVector = xd.ReadBoolean(xe, nameof(useCharacterUpVector));
            alignMode = xd.ReadFlag<AlignMode, sbyte>(xe, nameof(alignMode));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbFootIkGains>(xe, nameof(gains), gains);
            xs.WriteClassArray(xe, nameof(legs), legs);
            xs.WriteFloat(xe, nameof(raycastDistanceUp), raycastDistanceUp);
            xs.WriteFloat(xe, nameof(raycastDistanceDown), raycastDistanceDown);
            xs.WriteFloat(xe, nameof(originalGroundHeightMS), originalGroundHeightMS);
            xs.WriteFloat(xe, nameof(errorOut), errorOut);
            xs.WriteVector4(xe, nameof(errorOutTranslation), errorOutTranslation);
            xs.WriteQuaternion(xe, nameof(alignWithGroundRotation), alignWithGroundRotation);
            xs.WriteFloat(xe, nameof(verticalOffset), verticalOffset);
            xs.WriteNumber(xe, nameof(collisionFilterInfo), collisionFilterInfo);
            xs.WriteFloat(xe, nameof(forwardAlignFraction), forwardAlignFraction);
            xs.WriteFloat(xe, nameof(sidewaysAlignFraction), sidewaysAlignFraction);
            xs.WriteFloat(xe, nameof(sidewaysSampleWidth), sidewaysSampleWidth);
            xs.WriteBoolean(xe, nameof(useTrackData), useTrackData);
            xs.WriteBoolean(xe, nameof(lockFeetWhenPlanted), lockFeetWhenPlanted);
            xs.WriteBoolean(xe, nameof(useCharacterUpVector), useCharacterUpVector);
            xs.WriteEnum<AlignMode, sbyte>(xe, nameof(alignMode), alignMode);
            xs.WriteSerializeIgnored(xe, nameof(internalLegData));
            xs.WriteSerializeIgnored(xe, nameof(prevIsFootIkEnabled));
            xs.WriteSerializeIgnored(xe, nameof(isSetUp));
            xs.WriteSerializeIgnored(xe, nameof(isGroundPositionValid));
            xs.WriteSerializeIgnored(xe, nameof(timeStep));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbFootIkModifier);
        }

        public bool Equals(hkbFootIkModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((gains is null && other.gains is null) || (gains is not null && other.gains is not null && gains.Equals((IHavokObject)other.gains))) &&
                   legs.SequenceEqual(other.legs) &&
                   raycastDistanceUp.Equals(other.raycastDistanceUp) &&
                   raycastDistanceDown.Equals(other.raycastDistanceDown) &&
                   originalGroundHeightMS.Equals(other.originalGroundHeightMS) &&
                   errorOut.Equals(other.errorOut) &&
                   errorOutTranslation.Equals(other.errorOutTranslation) &&
                   alignWithGroundRotation.Equals(other.alignWithGroundRotation) &&
                   verticalOffset.Equals(other.verticalOffset) &&
                   collisionFilterInfo.Equals(other.collisionFilterInfo) &&
                   forwardAlignFraction.Equals(other.forwardAlignFraction) &&
                   sidewaysAlignFraction.Equals(other.sidewaysAlignFraction) &&
                   sidewaysSampleWidth.Equals(other.sidewaysSampleWidth) &&
                   useTrackData.Equals(other.useTrackData) &&
                   lockFeetWhenPlanted.Equals(other.lockFeetWhenPlanted) &&
                   useCharacterUpVector.Equals(other.useCharacterUpVector) &&
                   alignMode.Equals(other.alignMode) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(gains);
            hashcode.Add(legs.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(raycastDistanceUp);
            hashcode.Add(raycastDistanceDown);
            hashcode.Add(originalGroundHeightMS);
            hashcode.Add(errorOut);
            hashcode.Add(errorOutTranslation);
            hashcode.Add(alignWithGroundRotation);
            hashcode.Add(verticalOffset);
            hashcode.Add(collisionFilterInfo);
            hashcode.Add(forwardAlignFraction);
            hashcode.Add(sidewaysAlignFraction);
            hashcode.Add(sidewaysSampleWidth);
            hashcode.Add(useTrackData);
            hashcode.Add(lockFeetWhenPlanted);
            hashcode.Add(useCharacterUpVector);
            hashcode.Add(alignMode);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

