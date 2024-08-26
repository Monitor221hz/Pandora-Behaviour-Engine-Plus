using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSpringDamperConstraintMotor Signatire: 0x7ead26f6 size: 40 flags: FLAGS_NONE

    // springConstant class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // springDamping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    public partial class hkpSpringDamperConstraintMotor : hkpLimitedForceConstraintMotor, IEquatable<hkpSpringDamperConstraintMotor?>
    {
        public float springConstant { set; get; }
        public float springDamping { set; get; }

        public override uint Signature { set; get; } = 0x7ead26f6;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            springConstant = br.ReadSingle();
            springDamping = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(springConstant);
            bw.WriteSingle(springDamping);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            springConstant = xd.ReadSingle(xe, nameof(springConstant));
            springDamping = xd.ReadSingle(xe, nameof(springDamping));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(springConstant), springConstant);
            xs.WriteFloat(xe, nameof(springDamping), springDamping);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSpringDamperConstraintMotor);
        }

        public bool Equals(hkpSpringDamperConstraintMotor? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   springConstant.Equals(other.springConstant) &&
                   springDamping.Equals(other.springDamping) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(springConstant);
            hashcode.Add(springDamping);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

