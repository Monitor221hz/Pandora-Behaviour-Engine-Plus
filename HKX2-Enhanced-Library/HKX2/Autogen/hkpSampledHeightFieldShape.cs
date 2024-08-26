using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSampledHeightFieldShape Signatire: 0x11213421 size: 112 flags: FLAGS_NONE

    // xRes class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // zRes class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // heightCenter class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // useProjectionBasedHeight class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    // heightfieldType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 45 flags: FLAGS_NONE enum: HeightFieldType
    // intToFloatScale class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // floatToIntScale class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // floatToIntOffsetFloorCorrected class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // extents class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    public partial class hkpSampledHeightFieldShape : hkpHeightFieldShape, IEquatable<hkpSampledHeightFieldShape?>
    {
        public int xRes { set; get; }
        public int zRes { set; get; }
        public float heightCenter { set; get; }
        public bool useProjectionBasedHeight { set; get; }
        public byte heightfieldType { set; get; }
        public Vector4 intToFloatScale { set; get; }
        public Vector4 floatToIntScale { set; get; }
        public Vector4 floatToIntOffsetFloorCorrected { set; get; }
        public Vector4 extents { set; get; }

        public override uint Signature { set; get; } = 0x11213421;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            xRes = br.ReadInt32();
            zRes = br.ReadInt32();
            heightCenter = br.ReadSingle();
            useProjectionBasedHeight = br.ReadBoolean();
            heightfieldType = br.ReadByte();
            br.Position += 2;
            intToFloatScale = br.ReadVector4();
            floatToIntScale = br.ReadVector4();
            floatToIntOffsetFloorCorrected = br.ReadVector4();
            extents = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(xRes);
            bw.WriteInt32(zRes);
            bw.WriteSingle(heightCenter);
            bw.WriteBoolean(useProjectionBasedHeight);
            bw.WriteByte(heightfieldType);
            bw.Position += 2;
            bw.WriteVector4(intToFloatScale);
            bw.WriteVector4(floatToIntScale);
            bw.WriteVector4(floatToIntOffsetFloorCorrected);
            bw.WriteVector4(extents);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            xRes = xd.ReadInt32(xe, nameof(xRes));
            zRes = xd.ReadInt32(xe, nameof(zRes));
            heightCenter = xd.ReadSingle(xe, nameof(heightCenter));
            useProjectionBasedHeight = xd.ReadBoolean(xe, nameof(useProjectionBasedHeight));
            heightfieldType = xd.ReadFlag<HeightFieldType, byte>(xe, nameof(heightfieldType));
            intToFloatScale = xd.ReadVector4(xe, nameof(intToFloatScale));
            floatToIntScale = xd.ReadVector4(xe, nameof(floatToIntScale));
            floatToIntOffsetFloorCorrected = xd.ReadVector4(xe, nameof(floatToIntOffsetFloorCorrected));
            extents = xd.ReadVector4(xe, nameof(extents));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(xRes), xRes);
            xs.WriteNumber(xe, nameof(zRes), zRes);
            xs.WriteFloat(xe, nameof(heightCenter), heightCenter);
            xs.WriteBoolean(xe, nameof(useProjectionBasedHeight), useProjectionBasedHeight);
            xs.WriteEnum<HeightFieldType, byte>(xe, nameof(heightfieldType), heightfieldType);
            xs.WriteVector4(xe, nameof(intToFloatScale), intToFloatScale);
            xs.WriteVector4(xe, nameof(floatToIntScale), floatToIntScale);
            xs.WriteVector4(xe, nameof(floatToIntOffsetFloorCorrected), floatToIntOffsetFloorCorrected);
            xs.WriteVector4(xe, nameof(extents), extents);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSampledHeightFieldShape);
        }

        public bool Equals(hkpSampledHeightFieldShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   xRes.Equals(other.xRes) &&
                   zRes.Equals(other.zRes) &&
                   heightCenter.Equals(other.heightCenter) &&
                   useProjectionBasedHeight.Equals(other.useProjectionBasedHeight) &&
                   heightfieldType.Equals(other.heightfieldType) &&
                   intToFloatScale.Equals(other.intToFloatScale) &&
                   floatToIntScale.Equals(other.floatToIntScale) &&
                   floatToIntOffsetFloorCorrected.Equals(other.floatToIntOffsetFloorCorrected) &&
                   extents.Equals(other.extents) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(xRes);
            hashcode.Add(zRes);
            hashcode.Add(heightCenter);
            hashcode.Add(useProjectionBasedHeight);
            hashcode.Add(heightfieldType);
            hashcode.Add(intToFloatScale);
            hashcode.Add(floatToIntScale);
            hashcode.Add(floatToIntOffsetFloorCorrected);
            hashcode.Add(extents);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

