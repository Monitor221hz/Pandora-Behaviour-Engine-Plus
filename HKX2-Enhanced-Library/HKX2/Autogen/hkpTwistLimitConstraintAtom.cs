using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpTwistLimitConstraintAtom Signatire: 0x7c9b1052 size: 20 flags: FLAGS_NONE

    // isEnabled class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // twistAxis class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 3 flags: FLAGS_NONE enum: 
    // refAxis class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // minAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // maxAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // angularLimitsTauFactor class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpTwistLimitConstraintAtom : hkpConstraintAtom, IEquatable<hkpTwistLimitConstraintAtom?>
    {
        public byte isEnabled { set; get; }
        public byte twistAxis { set; get; }
        public byte refAxis { set; get; }
        public float minAngle { set; get; }
        public float maxAngle { set; get; }
        public float angularLimitsTauFactor { set; get; }

        public override uint Signature { set; get; } = 0x7c9b1052;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isEnabled = br.ReadByte();
            twistAxis = br.ReadByte();
            refAxis = br.ReadByte();
            br.Position += 3;
            minAngle = br.ReadSingle();
            maxAngle = br.ReadSingle();
            angularLimitsTauFactor = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(isEnabled);
            bw.WriteByte(twistAxis);
            bw.WriteByte(refAxis);
            bw.Position += 3;
            bw.WriteSingle(minAngle);
            bw.WriteSingle(maxAngle);
            bw.WriteSingle(angularLimitsTauFactor);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isEnabled = xd.ReadByte(xe, nameof(isEnabled));
            twistAxis = xd.ReadByte(xe, nameof(twistAxis));
            refAxis = xd.ReadByte(xe, nameof(refAxis));
            minAngle = xd.ReadSingle(xe, nameof(minAngle));
            maxAngle = xd.ReadSingle(xe, nameof(maxAngle));
            angularLimitsTauFactor = xd.ReadSingle(xe, nameof(angularLimitsTauFactor));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(isEnabled), isEnabled);
            xs.WriteNumber(xe, nameof(twistAxis), twistAxis);
            xs.WriteNumber(xe, nameof(refAxis), refAxis);
            xs.WriteFloat(xe, nameof(minAngle), minAngle);
            xs.WriteFloat(xe, nameof(maxAngle), maxAngle);
            xs.WriteFloat(xe, nameof(angularLimitsTauFactor), angularLimitsTauFactor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpTwistLimitConstraintAtom);
        }

        public bool Equals(hkpTwistLimitConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isEnabled.Equals(other.isEnabled) &&
                   twistAxis.Equals(other.twistAxis) &&
                   refAxis.Equals(other.refAxis) &&
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
            hashcode.Add(twistAxis);
            hashcode.Add(refAxis);
            hashcode.Add(minAngle);
            hashcode.Add(maxAngle);
            hashcode.Add(angularLimitsTauFactor);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

