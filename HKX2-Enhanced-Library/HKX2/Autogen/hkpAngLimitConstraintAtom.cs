using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpAngLimitConstraintAtom Signatire: 0x9be0d9d size: 16 flags: FLAGS_NONE

    // isEnabled class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // limitAxis class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 3 flags: FLAGS_NONE enum: 
    // minAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // maxAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // angularLimitsTauFactor class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkpAngLimitConstraintAtom : hkpConstraintAtom, IEquatable<hkpAngLimitConstraintAtom?>
    {
        public byte isEnabled { set; get; }
        public byte limitAxis { set; get; }
        public float minAngle { set; get; }
        public float maxAngle { set; get; }
        public float angularLimitsTauFactor { set; get; }

        public override uint Signature { set; get; } = 0x9be0d9d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isEnabled = br.ReadByte();
            limitAxis = br.ReadByte();
            minAngle = br.ReadSingle();
            maxAngle = br.ReadSingle();
            angularLimitsTauFactor = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(isEnabled);
            bw.WriteByte(limitAxis);
            bw.WriteSingle(minAngle);
            bw.WriteSingle(maxAngle);
            bw.WriteSingle(angularLimitsTauFactor);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isEnabled = xd.ReadByte(xe, nameof(isEnabled));
            limitAxis = xd.ReadByte(xe, nameof(limitAxis));
            minAngle = xd.ReadSingle(xe, nameof(minAngle));
            maxAngle = xd.ReadSingle(xe, nameof(maxAngle));
            angularLimitsTauFactor = xd.ReadSingle(xe, nameof(angularLimitsTauFactor));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(isEnabled), isEnabled);
            xs.WriteNumber(xe, nameof(limitAxis), limitAxis);
            xs.WriteFloat(xe, nameof(minAngle), minAngle);
            xs.WriteFloat(xe, nameof(maxAngle), maxAngle);
            xs.WriteFloat(xe, nameof(angularLimitsTauFactor), angularLimitsTauFactor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpAngLimitConstraintAtom);
        }

        public bool Equals(hkpAngLimitConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isEnabled.Equals(other.isEnabled) &&
                   limitAxis.Equals(other.limitAxis) &&
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
            hashcode.Add(limitAxis);
            hashcode.Add(minAngle);
            hashcode.Add(maxAngle);
            hashcode.Add(angularLimitsTauFactor);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

