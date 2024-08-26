using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbPoweredRagdollControlData Signatire: 0xf5ba21b size: 32 flags: FLAGS_NONE

    // maxForce class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: ALIGN_16|FLAGS_NONE enum: 
    // tau class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // damping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // proportionalRecoveryVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // constantRecoveryVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbPoweredRagdollControlData : IHavokObject, IEquatable<hkbPoweredRagdollControlData?>
    {
        public float maxForce { set; get; }
        public float tau { set; get; }
        public float damping { set; get; }
        public float proportionalRecoveryVelocity { set; get; }
        public float constantRecoveryVelocity { set; get; }

        public virtual uint Signature { set; get; } = 0xf5ba21b;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            maxForce = br.ReadSingle();
            tau = br.ReadSingle();
            damping = br.ReadSingle();
            proportionalRecoveryVelocity = br.ReadSingle();
            constantRecoveryVelocity = br.ReadSingle();
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(maxForce);
            bw.WriteSingle(tau);
            bw.WriteSingle(damping);
            bw.WriteSingle(proportionalRecoveryVelocity);
            bw.WriteSingle(constantRecoveryVelocity);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            maxForce = xd.ReadSingle(xe, nameof(maxForce));
            tau = xd.ReadSingle(xe, nameof(tau));
            damping = xd.ReadSingle(xe, nameof(damping));
            proportionalRecoveryVelocity = xd.ReadSingle(xe, nameof(proportionalRecoveryVelocity));
            constantRecoveryVelocity = xd.ReadSingle(xe, nameof(constantRecoveryVelocity));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(maxForce), maxForce);
            xs.WriteFloat(xe, nameof(tau), tau);
            xs.WriteFloat(xe, nameof(damping), damping);
            xs.WriteFloat(xe, nameof(proportionalRecoveryVelocity), proportionalRecoveryVelocity);
            xs.WriteFloat(xe, nameof(constantRecoveryVelocity), constantRecoveryVelocity);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbPoweredRagdollControlData);
        }

        public bool Equals(hkbPoweredRagdollControlData? other)
        {
            return other is not null &&
                   maxForce.Equals(other.maxForce) &&
                   tau.Equals(other.tau) &&
                   damping.Equals(other.damping) &&
                   proportionalRecoveryVelocity.Equals(other.proportionalRecoveryVelocity) &&
                   constantRecoveryVelocity.Equals(other.constantRecoveryVelocity) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(maxForce);
            hashcode.Add(tau);
            hashcode.Add(damping);
            hashcode.Add(proportionalRecoveryVelocity);
            hashcode.Add(constantRecoveryVelocity);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

