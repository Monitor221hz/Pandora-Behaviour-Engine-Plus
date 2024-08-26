using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpLimitedForceConstraintMotor Signatire: 0x3377b0b0 size: 32 flags: FLAGS_NONE

    // minForce class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // maxForce class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    public partial class hkpLimitedForceConstraintMotor : hkpConstraintMotor, IEquatable<hkpLimitedForceConstraintMotor?>
    {
        public float minForce { set; get; }
        public float maxForce { set; get; }

        public override uint Signature { set; get; } = 0x3377b0b0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            minForce = br.ReadSingle();
            maxForce = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(minForce);
            bw.WriteSingle(maxForce);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            minForce = xd.ReadSingle(xe, nameof(minForce));
            maxForce = xd.ReadSingle(xe, nameof(maxForce));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(minForce), minForce);
            xs.WriteFloat(xe, nameof(maxForce), maxForce);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpLimitedForceConstraintMotor);
        }

        public bool Equals(hkpLimitedForceConstraintMotor? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   minForce.Equals(other.minForce) &&
                   maxForce.Equals(other.maxForce) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(minForce);
            hashcode.Add(maxForce);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

