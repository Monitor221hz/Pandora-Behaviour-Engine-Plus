using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpVelocityConstraintMotor Signatire: 0xfca2fcc3 size: 48 flags: FLAGS_NONE

    // tau class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // velocityTarget class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // useVelocityTargetFromConstraintTargets class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    public partial class hkpVelocityConstraintMotor : hkpLimitedForceConstraintMotor, IEquatable<hkpVelocityConstraintMotor?>
    {
        public float tau { set; get; }
        public float velocityTarget { set; get; }
        public bool useVelocityTargetFromConstraintTargets { set; get; }

        public override uint Signature { set; get; } = 0xfca2fcc3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            tau = br.ReadSingle();
            velocityTarget = br.ReadSingle();
            useVelocityTargetFromConstraintTargets = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(tau);
            bw.WriteSingle(velocityTarget);
            bw.WriteBoolean(useVelocityTargetFromConstraintTargets);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            tau = xd.ReadSingle(xe, nameof(tau));
            velocityTarget = xd.ReadSingle(xe, nameof(velocityTarget));
            useVelocityTargetFromConstraintTargets = xd.ReadBoolean(xe, nameof(useVelocityTargetFromConstraintTargets));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(tau), tau);
            xs.WriteFloat(xe, nameof(velocityTarget), velocityTarget);
            xs.WriteBoolean(xe, nameof(useVelocityTargetFromConstraintTargets), useVelocityTargetFromConstraintTargets);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpVelocityConstraintMotor);
        }

        public bool Equals(hkpVelocityConstraintMotor? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   tau.Equals(other.tau) &&
                   velocityTarget.Equals(other.velocityTarget) &&
                   useVelocityTargetFromConstraintTargets.Equals(other.useVelocityTargetFromConstraintTargets) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(tau);
            hashcode.Add(velocityTarget);
            hashcode.Add(useVelocityTargetFromConstraintTargets);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

