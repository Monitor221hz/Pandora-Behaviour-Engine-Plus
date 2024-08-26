using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbTwistModifier Signatire: 0xb6b76b32 size: 144 flags: FLAGS_NONE

    // axisOfRotation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // twistAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // startBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // endBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 102 flags: FLAGS_NONE enum: 
    // setAngleMethod class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 104 flags: FLAGS_NONE enum: SetAngleMethod
    // rotationAxisCoordinates class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 105 flags: FLAGS_NONE enum: RotationAxisCoordinates
    // isAdditive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 106 flags: FLAGS_NONE enum: 
    // boneChainIndices class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // parentBoneIndices class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 128 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbTwistModifier : hkbModifier, IEquatable<hkbTwistModifier?>
    {
        public Vector4 axisOfRotation { set; get; }
        public float twistAngle { set; get; }
        public short startBoneIndex { set; get; }
        public short endBoneIndex { set; get; }
        public sbyte setAngleMethod { set; get; }
        public sbyte rotationAxisCoordinates { set; get; }
        public bool isAdditive { set; get; }
        public IList<object> boneChainIndices { set; get; } = Array.Empty<object>();
        public IList<object> parentBoneIndices { set; get; } = Array.Empty<object>();

        public override uint Signature { set; get; } = 0xb6b76b32;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            axisOfRotation = br.ReadVector4();
            twistAngle = br.ReadSingle();
            startBoneIndex = br.ReadInt16();
            endBoneIndex = br.ReadInt16();
            setAngleMethod = br.ReadSByte();
            rotationAxisCoordinates = br.ReadSByte();
            isAdditive = br.ReadBoolean();
            br.Position += 5;
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(axisOfRotation);
            bw.WriteSingle(twistAngle);
            bw.WriteInt16(startBoneIndex);
            bw.WriteInt16(endBoneIndex);
            bw.WriteSByte(setAngleMethod);
            bw.WriteSByte(rotationAxisCoordinates);
            bw.WriteBoolean(isAdditive);
            bw.Position += 5;
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            axisOfRotation = xd.ReadVector4(xe, nameof(axisOfRotation));
            twistAngle = xd.ReadSingle(xe, nameof(twistAngle));
            startBoneIndex = xd.ReadInt16(xe, nameof(startBoneIndex));
            endBoneIndex = xd.ReadInt16(xe, nameof(endBoneIndex));
            setAngleMethod = xd.ReadFlag<SetAngleMethod, sbyte>(xe, nameof(setAngleMethod));
            rotationAxisCoordinates = xd.ReadFlag<RotationAxisCoordinates, sbyte>(xe, nameof(rotationAxisCoordinates));
            isAdditive = xd.ReadBoolean(xe, nameof(isAdditive));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(axisOfRotation), axisOfRotation);
            xs.WriteFloat(xe, nameof(twistAngle), twistAngle);
            xs.WriteNumber(xe, nameof(startBoneIndex), startBoneIndex);
            xs.WriteNumber(xe, nameof(endBoneIndex), endBoneIndex);
            xs.WriteEnum<SetAngleMethod, sbyte>(xe, nameof(setAngleMethod), setAngleMethod);
            xs.WriteEnum<RotationAxisCoordinates, sbyte>(xe, nameof(rotationAxisCoordinates), rotationAxisCoordinates);
            xs.WriteBoolean(xe, nameof(isAdditive), isAdditive);
            xs.WriteSerializeIgnored(xe, nameof(boneChainIndices));
            xs.WriteSerializeIgnored(xe, nameof(parentBoneIndices));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbTwistModifier);
        }

        public bool Equals(hkbTwistModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   axisOfRotation.Equals(other.axisOfRotation) &&
                   twistAngle.Equals(other.twistAngle) &&
                   startBoneIndex.Equals(other.startBoneIndex) &&
                   endBoneIndex.Equals(other.endBoneIndex) &&
                   setAngleMethod.Equals(other.setAngleMethod) &&
                   rotationAxisCoordinates.Equals(other.rotationAxisCoordinates) &&
                   isAdditive.Equals(other.isAdditive) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(axisOfRotation);
            hashcode.Add(twistAngle);
            hashcode.Add(startBoneIndex);
            hashcode.Add(endBoneIndex);
            hashcode.Add(setAngleMethod);
            hashcode.Add(rotationAxisCoordinates);
            hashcode.Add(isAdditive);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

