using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpLinFrictionConstraintAtom Signatire: 0x3e94ef7c size: 8 flags: FLAGS_NONE

    // isEnabled class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // frictionAxis class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 3 flags: FLAGS_NONE enum: 
    // maxFrictionForce class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkpLinFrictionConstraintAtom : hkpConstraintAtom, IEquatable<hkpLinFrictionConstraintAtom?>
    {
        public byte isEnabled { set; get; }
        public byte frictionAxis { set; get; }
        public float maxFrictionForce { set; get; }

        public override uint Signature { set; get; } = 0x3e94ef7c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isEnabled = br.ReadByte();
            frictionAxis = br.ReadByte();
            maxFrictionForce = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(isEnabled);
            bw.WriteByte(frictionAxis);
            bw.WriteSingle(maxFrictionForce);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isEnabled = xd.ReadByte(xe, nameof(isEnabled));
            frictionAxis = xd.ReadByte(xe, nameof(frictionAxis));
            maxFrictionForce = xd.ReadSingle(xe, nameof(maxFrictionForce));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(isEnabled), isEnabled);
            xs.WriteNumber(xe, nameof(frictionAxis), frictionAxis);
            xs.WriteFloat(xe, nameof(maxFrictionForce), maxFrictionForce);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpLinFrictionConstraintAtom);
        }

        public bool Equals(hkpLinFrictionConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isEnabled.Equals(other.isEnabled) &&
                   frictionAxis.Equals(other.frictionAxis) &&
                   maxFrictionForce.Equals(other.maxFrictionForce) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(isEnabled);
            hashcode.Add(frictionAxis);
            hashcode.Add(maxFrictionForce);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

