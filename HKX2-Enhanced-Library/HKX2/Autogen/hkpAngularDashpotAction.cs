using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpAngularDashpotAction Signatire: 0x35f4c487 size: 96 flags: FLAGS_NONE

    // rotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // strength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // damping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    public partial class hkpAngularDashpotAction : hkpBinaryAction, IEquatable<hkpAngularDashpotAction?>
    {
        public Quaternion rotation { set; get; }
        public float strength { set; get; }
        public float damping { set; get; }

        public override uint Signature { set; get; } = 0x35f4c487;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rotation = des.ReadQuaternion(br);
            strength = br.ReadSingle();
            damping = br.ReadSingle();
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQuaternion(bw, rotation);
            bw.WriteSingle(strength);
            bw.WriteSingle(damping);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rotation = xd.ReadQuaternion(xe, nameof(rotation));
            strength = xd.ReadSingle(xe, nameof(strength));
            damping = xd.ReadSingle(xe, nameof(damping));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQuaternion(xe, nameof(rotation), rotation);
            xs.WriteFloat(xe, nameof(strength), strength);
            xs.WriteFloat(xe, nameof(damping), damping);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpAngularDashpotAction);
        }

        public bool Equals(hkpAngularDashpotAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rotation.Equals(other.rotation) &&
                   strength.Equals(other.strength) &&
                   damping.Equals(other.damping) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rotation);
            hashcode.Add(strength);
            hashcode.Add(damping);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

