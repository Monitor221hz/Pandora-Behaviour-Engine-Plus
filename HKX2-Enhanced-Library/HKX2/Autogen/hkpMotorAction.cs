using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMotorAction Signatire: 0x8ff131d9 size: 96 flags: FLAGS_NONE

    // axis class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // spinRate class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // gain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // active class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    public partial class hkpMotorAction : hkpUnaryAction, IEquatable<hkpMotorAction?>
    {
        public Vector4 axis { set; get; }
        public float spinRate { set; get; }
        public float gain { set; get; }
        public bool active { set; get; }

        public override uint Signature { set; get; } = 0x8ff131d9;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            axis = br.ReadVector4();
            spinRate = br.ReadSingle();
            gain = br.ReadSingle();
            active = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            bw.WriteVector4(axis);
            bw.WriteSingle(spinRate);
            bw.WriteSingle(gain);
            bw.WriteBoolean(active);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            axis = xd.ReadVector4(xe, nameof(axis));
            spinRate = xd.ReadSingle(xe, nameof(spinRate));
            gain = xd.ReadSingle(xe, nameof(gain));
            active = xd.ReadBoolean(xe, nameof(active));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(axis), axis);
            xs.WriteFloat(xe, nameof(spinRate), spinRate);
            xs.WriteFloat(xe, nameof(gain), gain);
            xs.WriteBoolean(xe, nameof(active), active);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMotorAction);
        }

        public bool Equals(hkpMotorAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   axis.Equals(other.axis) &&
                   spinRate.Equals(other.spinRate) &&
                   gain.Equals(other.gain) &&
                   active.Equals(other.active) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(axis);
            hashcode.Add(spinRate);
            hashcode.Add(gain);
            hashcode.Add(active);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

