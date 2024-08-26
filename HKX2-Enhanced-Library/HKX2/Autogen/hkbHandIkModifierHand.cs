using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbHandIkModifierHand Signatire: 0x14dfe1dd size: 96 flags: FLAGS_NONE

    // elbowAxisLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // backHandNormalLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // handOffsetLS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // handOrienationOffsetLS class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // maxElbowAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // minElbowAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // shoulderIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // shoulderSiblingIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 74 flags: FLAGS_NONE enum: 
    // elbowIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    // elbowSiblingIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 78 flags: FLAGS_NONE enum: 
    // wristIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // enforceEndPosition class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 82 flags: FLAGS_NONE enum: 
    // enforceEndRotation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 83 flags: FLAGS_NONE enum: 
    // localFrameName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    public partial class hkbHandIkModifierHand : IHavokObject, IEquatable<hkbHandIkModifierHand?>
    {
        public Vector4 elbowAxisLS { set; get; }
        public Vector4 backHandNormalLS { set; get; }
        public Vector4 handOffsetLS { set; get; }
        public Quaternion handOrienationOffsetLS { set; get; }
        public float maxElbowAngleDegrees { set; get; }
        public float minElbowAngleDegrees { set; get; }
        public short shoulderIndex { set; get; }
        public short shoulderSiblingIndex { set; get; }
        public short elbowIndex { set; get; }
        public short elbowSiblingIndex { set; get; }
        public short wristIndex { set; get; }
        public bool enforceEndPosition { set; get; }
        public bool enforceEndRotation { set; get; }
        public string localFrameName { set; get; } = "";

        public virtual uint Signature { set; get; } = 0x14dfe1dd;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            elbowAxisLS = br.ReadVector4();
            backHandNormalLS = br.ReadVector4();
            handOffsetLS = br.ReadVector4();
            handOrienationOffsetLS = des.ReadQuaternion(br);
            maxElbowAngleDegrees = br.ReadSingle();
            minElbowAngleDegrees = br.ReadSingle();
            shoulderIndex = br.ReadInt16();
            shoulderSiblingIndex = br.ReadInt16();
            elbowIndex = br.ReadInt16();
            elbowSiblingIndex = br.ReadInt16();
            wristIndex = br.ReadInt16();
            enforceEndPosition = br.ReadBoolean();
            enforceEndRotation = br.ReadBoolean();
            br.Position += 4;
            localFrameName = des.ReadStringPointer(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(elbowAxisLS);
            bw.WriteVector4(backHandNormalLS);
            bw.WriteVector4(handOffsetLS);
            s.WriteQuaternion(bw, handOrienationOffsetLS);
            bw.WriteSingle(maxElbowAngleDegrees);
            bw.WriteSingle(minElbowAngleDegrees);
            bw.WriteInt16(shoulderIndex);
            bw.WriteInt16(shoulderSiblingIndex);
            bw.WriteInt16(elbowIndex);
            bw.WriteInt16(elbowSiblingIndex);
            bw.WriteInt16(wristIndex);
            bw.WriteBoolean(enforceEndPosition);
            bw.WriteBoolean(enforceEndRotation);
            bw.Position += 4;
            s.WriteStringPointer(bw, localFrameName);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            elbowAxisLS = xd.ReadVector4(xe, nameof(elbowAxisLS));
            backHandNormalLS = xd.ReadVector4(xe, nameof(backHandNormalLS));
            handOffsetLS = xd.ReadVector4(xe, nameof(handOffsetLS));
            handOrienationOffsetLS = xd.ReadQuaternion(xe, nameof(handOrienationOffsetLS));
            maxElbowAngleDegrees = xd.ReadSingle(xe, nameof(maxElbowAngleDegrees));
            minElbowAngleDegrees = xd.ReadSingle(xe, nameof(minElbowAngleDegrees));
            shoulderIndex = xd.ReadInt16(xe, nameof(shoulderIndex));
            shoulderSiblingIndex = xd.ReadInt16(xe, nameof(shoulderSiblingIndex));
            elbowIndex = xd.ReadInt16(xe, nameof(elbowIndex));
            elbowSiblingIndex = xd.ReadInt16(xe, nameof(elbowSiblingIndex));
            wristIndex = xd.ReadInt16(xe, nameof(wristIndex));
            enforceEndPosition = xd.ReadBoolean(xe, nameof(enforceEndPosition));
            enforceEndRotation = xd.ReadBoolean(xe, nameof(enforceEndRotation));
            localFrameName = xd.ReadString(xe, nameof(localFrameName));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(elbowAxisLS), elbowAxisLS);
            xs.WriteVector4(xe, nameof(backHandNormalLS), backHandNormalLS);
            xs.WriteVector4(xe, nameof(handOffsetLS), handOffsetLS);
            xs.WriteQuaternion(xe, nameof(handOrienationOffsetLS), handOrienationOffsetLS);
            xs.WriteFloat(xe, nameof(maxElbowAngleDegrees), maxElbowAngleDegrees);
            xs.WriteFloat(xe, nameof(minElbowAngleDegrees), minElbowAngleDegrees);
            xs.WriteNumber(xe, nameof(shoulderIndex), shoulderIndex);
            xs.WriteNumber(xe, nameof(shoulderSiblingIndex), shoulderSiblingIndex);
            xs.WriteNumber(xe, nameof(elbowIndex), elbowIndex);
            xs.WriteNumber(xe, nameof(elbowSiblingIndex), elbowSiblingIndex);
            xs.WriteNumber(xe, nameof(wristIndex), wristIndex);
            xs.WriteBoolean(xe, nameof(enforceEndPosition), enforceEndPosition);
            xs.WriteBoolean(xe, nameof(enforceEndRotation), enforceEndRotation);
            xs.WriteString(xe, nameof(localFrameName), localFrameName);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbHandIkModifierHand);
        }

        public bool Equals(hkbHandIkModifierHand? other)
        {
            return other is not null &&
                   elbowAxisLS.Equals(other.elbowAxisLS) &&
                   backHandNormalLS.Equals(other.backHandNormalLS) &&
                   handOffsetLS.Equals(other.handOffsetLS) &&
                   handOrienationOffsetLS.Equals(other.handOrienationOffsetLS) &&
                   maxElbowAngleDegrees.Equals(other.maxElbowAngleDegrees) &&
                   minElbowAngleDegrees.Equals(other.minElbowAngleDegrees) &&
                   shoulderIndex.Equals(other.shoulderIndex) &&
                   shoulderSiblingIndex.Equals(other.shoulderSiblingIndex) &&
                   elbowIndex.Equals(other.elbowIndex) &&
                   elbowSiblingIndex.Equals(other.elbowSiblingIndex) &&
                   wristIndex.Equals(other.wristIndex) &&
                   enforceEndPosition.Equals(other.enforceEndPosition) &&
                   enforceEndRotation.Equals(other.enforceEndRotation) &&
                   (localFrameName is null && other.localFrameName is null || localFrameName == other.localFrameName || localFrameName is null && other.localFrameName == "" || localFrameName == "" && other.localFrameName is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(elbowAxisLS);
            hashcode.Add(backHandNormalLS);
            hashcode.Add(handOffsetLS);
            hashcode.Add(handOrienationOffsetLS);
            hashcode.Add(maxElbowAngleDegrees);
            hashcode.Add(minElbowAngleDegrees);
            hashcode.Add(shoulderIndex);
            hashcode.Add(shoulderSiblingIndex);
            hashcode.Add(elbowIndex);
            hashcode.Add(elbowSiblingIndex);
            hashcode.Add(wristIndex);
            hashcode.Add(enforceEndPosition);
            hashcode.Add(enforceEndRotation);
            hashcode.Add(localFrameName);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

