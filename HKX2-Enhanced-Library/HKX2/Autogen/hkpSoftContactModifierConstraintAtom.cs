using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSoftContactModifierConstraintAtom Signatire: 0xecb34e27 size: 64 flags: FLAGS_NONE

    // tau class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // maxAcceleration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 52 flags: FLAGS_NONE enum: 
    public partial class hkpSoftContactModifierConstraintAtom : hkpModifierConstraintAtom, IEquatable<hkpSoftContactModifierConstraintAtom?>
    {
        public float tau { set; get; }
        public float maxAcceleration { set; get; }

        public override uint Signature { set; get; } = 0xecb34e27;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            tau = br.ReadSingle();
            maxAcceleration = br.ReadSingle();
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(tau);
            bw.WriteSingle(maxAcceleration);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            tau = xd.ReadSingle(xe, nameof(tau));
            maxAcceleration = xd.ReadSingle(xe, nameof(maxAcceleration));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(tau), tau);
            xs.WriteFloat(xe, nameof(maxAcceleration), maxAcceleration);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSoftContactModifierConstraintAtom);
        }

        public bool Equals(hkpSoftContactModifierConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   tau.Equals(other.tau) &&
                   maxAcceleration.Equals(other.maxAcceleration) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(tau);
            hashcode.Add(maxAcceleration);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

