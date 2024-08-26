using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPositionConstraintMotor Signatire: 0x748fb303 size: 48 flags: FLAGS_NONE

    // tau class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // damping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // proportionalRecoveryVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // constantRecoveryVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    public partial class hkpPositionConstraintMotor : hkpLimitedForceConstraintMotor, IEquatable<hkpPositionConstraintMotor?>
    {
        public float tau { set; get; }
        public float damping { set; get; }
        public float proportionalRecoveryVelocity { set; get; }
        public float constantRecoveryVelocity { set; get; }

        public override uint Signature { set; get; } = 0x748fb303;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            tau = br.ReadSingle();
            damping = br.ReadSingle();
            proportionalRecoveryVelocity = br.ReadSingle();
            constantRecoveryVelocity = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(tau);
            bw.WriteSingle(damping);
            bw.WriteSingle(proportionalRecoveryVelocity);
            bw.WriteSingle(constantRecoveryVelocity);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            tau = xd.ReadSingle(xe, nameof(tau));
            damping = xd.ReadSingle(xe, nameof(damping));
            proportionalRecoveryVelocity = xd.ReadSingle(xe, nameof(proportionalRecoveryVelocity));
            constantRecoveryVelocity = xd.ReadSingle(xe, nameof(constantRecoveryVelocity));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(tau), tau);
            xs.WriteFloat(xe, nameof(damping), damping);
            xs.WriteFloat(xe, nameof(proportionalRecoveryVelocity), proportionalRecoveryVelocity);
            xs.WriteFloat(xe, nameof(constantRecoveryVelocity), constantRecoveryVelocity);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPositionConstraintMotor);
        }

        public bool Equals(hkpPositionConstraintMotor? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   tau.Equals(other.tau) &&
                   damping.Equals(other.damping) &&
                   proportionalRecoveryVelocity.Equals(other.proportionalRecoveryVelocity) &&
                   constantRecoveryVelocity.Equals(other.constantRecoveryVelocity) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(tau);
            hashcode.Add(damping);
            hashcode.Add(proportionalRecoveryVelocity);
            hashcode.Add(constantRecoveryVelocity);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

