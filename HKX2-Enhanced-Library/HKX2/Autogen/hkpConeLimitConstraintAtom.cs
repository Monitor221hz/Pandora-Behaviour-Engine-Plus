using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConeLimitConstraintAtom Signatire: 0xf19443c8 size: 20 flags: FLAGS_NONE

    // isEnabled class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // twistAxisInA class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 3 flags: FLAGS_NONE enum: 
    // refAxisInB class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // angleMeasurementMode class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 5 flags: FLAGS_NONE enum: MeasurementMode
    // memOffsetToAngleOffset class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 6 flags: FLAGS_NONE enum: 
    // minAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // maxAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // angularLimitsTauFactor class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpConeLimitConstraintAtom : hkpConstraintAtom, IEquatable<hkpConeLimitConstraintAtom?>
    {
        public byte isEnabled { set; get; }
        public byte twistAxisInA { set; get; }
        public byte refAxisInB { set; get; }
        public byte angleMeasurementMode { set; get; }
        public byte memOffsetToAngleOffset { set; get; }
        public float minAngle { set; get; }
        public float maxAngle { set; get; }
        public float angularLimitsTauFactor { set; get; }

        public override uint Signature { set; get; } = 0xf19443c8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isEnabled = br.ReadByte();
            twistAxisInA = br.ReadByte();
            refAxisInB = br.ReadByte();
            angleMeasurementMode = br.ReadByte();
            memOffsetToAngleOffset = br.ReadByte();
            br.Position += 1;
            minAngle = br.ReadSingle();
            maxAngle = br.ReadSingle();
            angularLimitsTauFactor = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(isEnabled);
            bw.WriteByte(twistAxisInA);
            bw.WriteByte(refAxisInB);
            bw.WriteByte(angleMeasurementMode);
            bw.WriteByte(memOffsetToAngleOffset);
            bw.Position += 1;
            bw.WriteSingle(minAngle);
            bw.WriteSingle(maxAngle);
            bw.WriteSingle(angularLimitsTauFactor);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isEnabled = xd.ReadByte(xe, nameof(isEnabled));
            twistAxisInA = xd.ReadByte(xe, nameof(twistAxisInA));
            refAxisInB = xd.ReadByte(xe, nameof(refAxisInB));
            angleMeasurementMode = xd.ReadFlag<MeasurementMode, byte>(xe, nameof(angleMeasurementMode));
            memOffsetToAngleOffset = xd.ReadByte(xe, nameof(memOffsetToAngleOffset));
            minAngle = xd.ReadSingle(xe, nameof(minAngle));
            maxAngle = xd.ReadSingle(xe, nameof(maxAngle));
            angularLimitsTauFactor = xd.ReadSingle(xe, nameof(angularLimitsTauFactor));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(isEnabled), isEnabled);
            xs.WriteNumber(xe, nameof(twistAxisInA), twistAxisInA);
            xs.WriteNumber(xe, nameof(refAxisInB), refAxisInB);
            xs.WriteEnum<MeasurementMode, byte>(xe, nameof(angleMeasurementMode), angleMeasurementMode);
            xs.WriteNumber(xe, nameof(memOffsetToAngleOffset), memOffsetToAngleOffset);
            xs.WriteFloat(xe, nameof(minAngle), minAngle);
            xs.WriteFloat(xe, nameof(maxAngle), maxAngle);
            xs.WriteFloat(xe, nameof(angularLimitsTauFactor), angularLimitsTauFactor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConeLimitConstraintAtom);
        }

        public bool Equals(hkpConeLimitConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isEnabled.Equals(other.isEnabled) &&
                   twistAxisInA.Equals(other.twistAxisInA) &&
                   refAxisInB.Equals(other.refAxisInB) &&
                   angleMeasurementMode.Equals(other.angleMeasurementMode) &&
                   memOffsetToAngleOffset.Equals(other.memOffsetToAngleOffset) &&
                   minAngle.Equals(other.minAngle) &&
                   maxAngle.Equals(other.maxAngle) &&
                   angularLimitsTauFactor.Equals(other.angularLimitsTauFactor) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(isEnabled);
            hashcode.Add(twistAxisInA);
            hashcode.Add(refAxisInB);
            hashcode.Add(angleMeasurementMode);
            hashcode.Add(memOffsetToAngleOffset);
            hashcode.Add(minAngle);
            hashcode.Add(maxAngle);
            hashcode.Add(angularLimitsTauFactor);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

