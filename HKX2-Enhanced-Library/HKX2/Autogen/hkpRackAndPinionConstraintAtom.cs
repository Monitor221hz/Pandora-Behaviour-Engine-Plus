using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpRackAndPinionConstraintAtom Signatire: 0x30cae006 size: 12 flags: FLAGS_NONE

    // pinionRadiusOrScrewPitch class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // isScrew class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // memOffsetToInitialAngleOffset class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 9 flags: FLAGS_NONE enum: 
    // memOffsetToPrevAngle class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 10 flags: FLAGS_NONE enum: 
    // memOffsetToRevolutionCounter class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 11 flags: FLAGS_NONE enum: 
    public partial class hkpRackAndPinionConstraintAtom : hkpConstraintAtom, IEquatable<hkpRackAndPinionConstraintAtom?>
    {
        public float pinionRadiusOrScrewPitch { set; get; }
        public bool isScrew { set; get; }
        public sbyte memOffsetToInitialAngleOffset { set; get; }
        public sbyte memOffsetToPrevAngle { set; get; }
        public sbyte memOffsetToRevolutionCounter { set; get; }

        public override uint Signature { set; get; } = 0x30cae006;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 2;
            pinionRadiusOrScrewPitch = br.ReadSingle();
            isScrew = br.ReadBoolean();
            memOffsetToInitialAngleOffset = br.ReadSByte();
            memOffsetToPrevAngle = br.ReadSByte();
            memOffsetToRevolutionCounter = br.ReadSByte();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 2;
            bw.WriteSingle(pinionRadiusOrScrewPitch);
            bw.WriteBoolean(isScrew);
            bw.WriteSByte(memOffsetToInitialAngleOffset);
            bw.WriteSByte(memOffsetToPrevAngle);
            bw.WriteSByte(memOffsetToRevolutionCounter);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pinionRadiusOrScrewPitch = xd.ReadSingle(xe, nameof(pinionRadiusOrScrewPitch));
            isScrew = xd.ReadBoolean(xe, nameof(isScrew));
            memOffsetToInitialAngleOffset = xd.ReadSByte(xe, nameof(memOffsetToInitialAngleOffset));
            memOffsetToPrevAngle = xd.ReadSByte(xe, nameof(memOffsetToPrevAngle));
            memOffsetToRevolutionCounter = xd.ReadSByte(xe, nameof(memOffsetToRevolutionCounter));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(pinionRadiusOrScrewPitch), pinionRadiusOrScrewPitch);
            xs.WriteBoolean(xe, nameof(isScrew), isScrew);
            xs.WriteNumber(xe, nameof(memOffsetToInitialAngleOffset), memOffsetToInitialAngleOffset);
            xs.WriteNumber(xe, nameof(memOffsetToPrevAngle), memOffsetToPrevAngle);
            xs.WriteNumber(xe, nameof(memOffsetToRevolutionCounter), memOffsetToRevolutionCounter);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpRackAndPinionConstraintAtom);
        }

        public bool Equals(hkpRackAndPinionConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   pinionRadiusOrScrewPitch.Equals(other.pinionRadiusOrScrewPitch) &&
                   isScrew.Equals(other.isScrew) &&
                   memOffsetToInitialAngleOffset.Equals(other.memOffsetToInitialAngleOffset) &&
                   memOffsetToPrevAngle.Equals(other.memOffsetToPrevAngle) &&
                   memOffsetToRevolutionCounter.Equals(other.memOffsetToRevolutionCounter) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pinionRadiusOrScrewPitch);
            hashcode.Add(isScrew);
            hashcode.Add(memOffsetToInitialAngleOffset);
            hashcode.Add(memOffsetToPrevAngle);
            hashcode.Add(memOffsetToRevolutionCounter);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

