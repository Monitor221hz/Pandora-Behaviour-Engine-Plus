using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaSplineCompressedAnimationTrackCompressionParams Signatire: 0x42e878d3 size: 28 flags: FLAGS_NONE

    // rotationTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // translationTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // scaleTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // floatingTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // rotationDegree class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // translationDegree class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 18 flags: FLAGS_NONE enum: 
    // scaleDegree class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // floatingDegree class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 22 flags: FLAGS_NONE enum: 
    // rotationQuantizationType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: RotationQuantization
    // translationQuantizationType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 25 flags: FLAGS_NONE enum: ScalarQuantization
    // scaleQuantizationType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 26 flags: FLAGS_NONE enum: ScalarQuantization
    // floatQuantizationType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 27 flags: FLAGS_NONE enum: ScalarQuantization
    public partial class hkaSplineCompressedAnimationTrackCompressionParams : IHavokObject, IEquatable<hkaSplineCompressedAnimationTrackCompressionParams?>
    {
        public float rotationTolerance { set; get; }
        public float translationTolerance { set; get; }
        public float scaleTolerance { set; get; }
        public float floatingTolerance { set; get; }
        public ushort rotationDegree { set; get; }
        public ushort translationDegree { set; get; }
        public ushort scaleDegree { set; get; }
        public ushort floatingDegree { set; get; }
        public byte rotationQuantizationType { set; get; }
        public byte translationQuantizationType { set; get; }
        public byte scaleQuantizationType { set; get; }
        public byte floatQuantizationType { set; get; }

        public virtual uint Signature { set; get; } = 0x42e878d3;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            rotationTolerance = br.ReadSingle();
            translationTolerance = br.ReadSingle();
            scaleTolerance = br.ReadSingle();
            floatingTolerance = br.ReadSingle();
            rotationDegree = br.ReadUInt16();
            translationDegree = br.ReadUInt16();
            scaleDegree = br.ReadUInt16();
            floatingDegree = br.ReadUInt16();
            rotationQuantizationType = br.ReadByte();
            translationQuantizationType = br.ReadByte();
            scaleQuantizationType = br.ReadByte();
            floatQuantizationType = br.ReadByte();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(rotationTolerance);
            bw.WriteSingle(translationTolerance);
            bw.WriteSingle(scaleTolerance);
            bw.WriteSingle(floatingTolerance);
            bw.WriteUInt16(rotationDegree);
            bw.WriteUInt16(translationDegree);
            bw.WriteUInt16(scaleDegree);
            bw.WriteUInt16(floatingDegree);
            bw.WriteByte(rotationQuantizationType);
            bw.WriteByte(translationQuantizationType);
            bw.WriteByte(scaleQuantizationType);
            bw.WriteByte(floatQuantizationType);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            rotationTolerance = xd.ReadSingle(xe, nameof(rotationTolerance));
            translationTolerance = xd.ReadSingle(xe, nameof(translationTolerance));
            scaleTolerance = xd.ReadSingle(xe, nameof(scaleTolerance));
            floatingTolerance = xd.ReadSingle(xe, nameof(floatingTolerance));
            rotationDegree = xd.ReadUInt16(xe, nameof(rotationDegree));
            translationDegree = xd.ReadUInt16(xe, nameof(translationDegree));
            scaleDegree = xd.ReadUInt16(xe, nameof(scaleDegree));
            floatingDegree = xd.ReadUInt16(xe, nameof(floatingDegree));
            rotationQuantizationType = xd.ReadFlag<RotationQuantization, byte>(xe, nameof(rotationQuantizationType));
            translationQuantizationType = xd.ReadFlag<ScalarQuantization, byte>(xe, nameof(translationQuantizationType));
            scaleQuantizationType = xd.ReadFlag<ScalarQuantization, byte>(xe, nameof(scaleQuantizationType));
            floatQuantizationType = xd.ReadFlag<ScalarQuantization, byte>(xe, nameof(floatQuantizationType));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(rotationTolerance), rotationTolerance);
            xs.WriteFloat(xe, nameof(translationTolerance), translationTolerance);
            xs.WriteFloat(xe, nameof(scaleTolerance), scaleTolerance);
            xs.WriteFloat(xe, nameof(floatingTolerance), floatingTolerance);
            xs.WriteNumber(xe, nameof(rotationDegree), rotationDegree);
            xs.WriteNumber(xe, nameof(translationDegree), translationDegree);
            xs.WriteNumber(xe, nameof(scaleDegree), scaleDegree);
            xs.WriteNumber(xe, nameof(floatingDegree), floatingDegree);
            xs.WriteEnum<RotationQuantization, byte>(xe, nameof(rotationQuantizationType), rotationQuantizationType);
            xs.WriteEnum<ScalarQuantization, byte>(xe, nameof(translationQuantizationType), translationQuantizationType);
            xs.WriteEnum<ScalarQuantization, byte>(xe, nameof(scaleQuantizationType), scaleQuantizationType);
            xs.WriteEnum<ScalarQuantization, byte>(xe, nameof(floatQuantizationType), floatQuantizationType);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaSplineCompressedAnimationTrackCompressionParams);
        }

        public bool Equals(hkaSplineCompressedAnimationTrackCompressionParams? other)
        {
            return other is not null &&
                   rotationTolerance.Equals(other.rotationTolerance) &&
                   translationTolerance.Equals(other.translationTolerance) &&
                   scaleTolerance.Equals(other.scaleTolerance) &&
                   floatingTolerance.Equals(other.floatingTolerance) &&
                   rotationDegree.Equals(other.rotationDegree) &&
                   translationDegree.Equals(other.translationDegree) &&
                   scaleDegree.Equals(other.scaleDegree) &&
                   floatingDegree.Equals(other.floatingDegree) &&
                   rotationQuantizationType.Equals(other.rotationQuantizationType) &&
                   translationQuantizationType.Equals(other.translationQuantizationType) &&
                   scaleQuantizationType.Equals(other.scaleQuantizationType) &&
                   floatQuantizationType.Equals(other.floatQuantizationType) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(rotationTolerance);
            hashcode.Add(translationTolerance);
            hashcode.Add(scaleTolerance);
            hashcode.Add(floatingTolerance);
            hashcode.Add(rotationDegree);
            hashcode.Add(translationDegree);
            hashcode.Add(scaleDegree);
            hashcode.Add(floatingDegree);
            hashcode.Add(rotationQuantizationType);
            hashcode.Add(translationQuantizationType);
            hashcode.Add(scaleQuantizationType);
            hashcode.Add(floatQuantizationType);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

