using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbFootIkDriverInfo Signatire: 0xc6a09dbf size: 72 flags: FLAGS_NONE

    // legs class: hkbFootIkDriverInfoLeg Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // raycastDistanceUp class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // raycastDistanceDown class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // originalGroundHeightMS class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // verticalOffset class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    // collisionFilterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // forwardAlignFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 52 flags: FLAGS_NONE enum: 
    // sidewaysAlignFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // sidewaysSampleWidth class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // lockFeetWhenPlanted class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // useCharacterUpVector class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 65 flags: FLAGS_NONE enum: 
    // isQuadrupedNarrow class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 66 flags: FLAGS_NONE enum: 
    public partial class hkbFootIkDriverInfo : hkReferencedObject, IEquatable<hkbFootIkDriverInfo?>
    {
        public IList<hkbFootIkDriverInfoLeg> legs { set; get; } = Array.Empty<hkbFootIkDriverInfoLeg>();
        public float raycastDistanceUp { set; get; }
        public float raycastDistanceDown { set; get; }
        public float originalGroundHeightMS { set; get; }
        public float verticalOffset { set; get; }
        public uint collisionFilterInfo { set; get; }
        public float forwardAlignFraction { set; get; }
        public float sidewaysAlignFraction { set; get; }
        public float sidewaysSampleWidth { set; get; }
        public bool lockFeetWhenPlanted { set; get; }
        public bool useCharacterUpVector { set; get; }
        public bool isQuadrupedNarrow { set; get; }

        public override uint Signature { set; get; } = 0xc6a09dbf;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            legs = des.ReadClassArray<hkbFootIkDriverInfoLeg>(br);
            raycastDistanceUp = br.ReadSingle();
            raycastDistanceDown = br.ReadSingle();
            originalGroundHeightMS = br.ReadSingle();
            verticalOffset = br.ReadSingle();
            collisionFilterInfo = br.ReadUInt32();
            forwardAlignFraction = br.ReadSingle();
            sidewaysAlignFraction = br.ReadSingle();
            sidewaysSampleWidth = br.ReadSingle();
            lockFeetWhenPlanted = br.ReadBoolean();
            useCharacterUpVector = br.ReadBoolean();
            isQuadrupedNarrow = br.ReadBoolean();
            br.Position += 5;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, legs);
            bw.WriteSingle(raycastDistanceUp);
            bw.WriteSingle(raycastDistanceDown);
            bw.WriteSingle(originalGroundHeightMS);
            bw.WriteSingle(verticalOffset);
            bw.WriteUInt32(collisionFilterInfo);
            bw.WriteSingle(forwardAlignFraction);
            bw.WriteSingle(sidewaysAlignFraction);
            bw.WriteSingle(sidewaysSampleWidth);
            bw.WriteBoolean(lockFeetWhenPlanted);
            bw.WriteBoolean(useCharacterUpVector);
            bw.WriteBoolean(isQuadrupedNarrow);
            bw.Position += 5;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            legs = xd.ReadClassArray<hkbFootIkDriverInfoLeg>(xe, nameof(legs));
            raycastDistanceUp = xd.ReadSingle(xe, nameof(raycastDistanceUp));
            raycastDistanceDown = xd.ReadSingle(xe, nameof(raycastDistanceDown));
            originalGroundHeightMS = xd.ReadSingle(xe, nameof(originalGroundHeightMS));
            verticalOffset = xd.ReadSingle(xe, nameof(verticalOffset));
            collisionFilterInfo = xd.ReadUInt32(xe, nameof(collisionFilterInfo));
            forwardAlignFraction = xd.ReadSingle(xe, nameof(forwardAlignFraction));
            sidewaysAlignFraction = xd.ReadSingle(xe, nameof(sidewaysAlignFraction));
            sidewaysSampleWidth = xd.ReadSingle(xe, nameof(sidewaysSampleWidth));
            lockFeetWhenPlanted = xd.ReadBoolean(xe, nameof(lockFeetWhenPlanted));
            useCharacterUpVector = xd.ReadBoolean(xe, nameof(useCharacterUpVector));
            isQuadrupedNarrow = xd.ReadBoolean(xe, nameof(isQuadrupedNarrow));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(legs), legs);
            xs.WriteFloat(xe, nameof(raycastDistanceUp), raycastDistanceUp);
            xs.WriteFloat(xe, nameof(raycastDistanceDown), raycastDistanceDown);
            xs.WriteFloat(xe, nameof(originalGroundHeightMS), originalGroundHeightMS);
            xs.WriteFloat(xe, nameof(verticalOffset), verticalOffset);
            xs.WriteNumber(xe, nameof(collisionFilterInfo), collisionFilterInfo);
            xs.WriteFloat(xe, nameof(forwardAlignFraction), forwardAlignFraction);
            xs.WriteFloat(xe, nameof(sidewaysAlignFraction), sidewaysAlignFraction);
            xs.WriteFloat(xe, nameof(sidewaysSampleWidth), sidewaysSampleWidth);
            xs.WriteBoolean(xe, nameof(lockFeetWhenPlanted), lockFeetWhenPlanted);
            xs.WriteBoolean(xe, nameof(useCharacterUpVector), useCharacterUpVector);
            xs.WriteBoolean(xe, nameof(isQuadrupedNarrow), isQuadrupedNarrow);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbFootIkDriverInfo);
        }

        public bool Equals(hkbFootIkDriverInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   legs.SequenceEqual(other.legs) &&
                   raycastDistanceUp.Equals(other.raycastDistanceUp) &&
                   raycastDistanceDown.Equals(other.raycastDistanceDown) &&
                   originalGroundHeightMS.Equals(other.originalGroundHeightMS) &&
                   verticalOffset.Equals(other.verticalOffset) &&
                   collisionFilterInfo.Equals(other.collisionFilterInfo) &&
                   forwardAlignFraction.Equals(other.forwardAlignFraction) &&
                   sidewaysAlignFraction.Equals(other.sidewaysAlignFraction) &&
                   sidewaysSampleWidth.Equals(other.sidewaysSampleWidth) &&
                   lockFeetWhenPlanted.Equals(other.lockFeetWhenPlanted) &&
                   useCharacterUpVector.Equals(other.useCharacterUpVector) &&
                   isQuadrupedNarrow.Equals(other.isQuadrupedNarrow) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(legs.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(raycastDistanceUp);
            hashcode.Add(raycastDistanceDown);
            hashcode.Add(originalGroundHeightMS);
            hashcode.Add(verticalOffset);
            hashcode.Add(collisionFilterInfo);
            hashcode.Add(forwardAlignFraction);
            hashcode.Add(sidewaysAlignFraction);
            hashcode.Add(sidewaysSampleWidth);
            hashcode.Add(lockFeetWhenPlanted);
            hashcode.Add(useCharacterUpVector);
            hashcode.Add(isQuadrupedNarrow);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

