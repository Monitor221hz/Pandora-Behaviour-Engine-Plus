using System;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpDashpotAction Signatire: 0x50746c6e size: 128 flags: FLAGS_NONE

    // point class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 2 offset: 64 flags: FLAGS_NONE enum: 
    // strength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // damping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // impulse class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkpDashpotAction : hkpBinaryAction, IEquatable<hkpDashpotAction?>
    {
        public Vector4[] point = new Vector4[2];
        public float strength { set; get; }
        public float damping { set; get; }
        public Vector4 impulse { set; get; }

        public override uint Signature { set; get; } = 0x50746c6e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            point = des.ReadVector4CStyleArray(br, 2);
            strength = br.ReadSingle();
            damping = br.ReadSingle();
            br.Position += 8;
            impulse = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVector4CStyleArray(bw, point);
            bw.WriteSingle(strength);
            bw.WriteSingle(damping);
            bw.Position += 8;
            bw.WriteVector4(impulse);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            point = xd.ReadVector4CStyleArray(xe, nameof(point), 2);
            strength = xd.ReadSingle(xe, nameof(strength));
            damping = xd.ReadSingle(xe, nameof(damping));
            impulse = xd.ReadVector4(xe, nameof(impulse));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4Array(xe, nameof(point), point);
            xs.WriteFloat(xe, nameof(strength), strength);
            xs.WriteFloat(xe, nameof(damping), damping);
            xs.WriteVector4(xe, nameof(impulse), impulse);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpDashpotAction);
        }

        public bool Equals(hkpDashpotAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   point.SequenceEqual(other.point) &&
                   strength.Equals(other.strength) &&
                   damping.Equals(other.damping) &&
                   impulse.Equals(other.impulse) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(point.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(strength);
            hashcode.Add(damping);
            hashcode.Add(impulse);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

