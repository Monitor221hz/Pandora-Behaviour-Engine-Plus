using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbRotateCharacterModifier Signatire: 0x877ebc0b size: 128 flags: FLAGS_NONE

    // degreesPerSecond class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // speedMultiplier class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // axisOfRotation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // angle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbRotateCharacterModifier : hkbModifier, IEquatable<hkbRotateCharacterModifier?>
    {
        public float degreesPerSecond { set; get; }
        public float speedMultiplier { set; get; }
        public Vector4 axisOfRotation { set; get; }
        private float angle { set; get; }

        public override uint Signature { set; get; } = 0x877ebc0b;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            degreesPerSecond = br.ReadSingle();
            speedMultiplier = br.ReadSingle();
            br.Position += 8;
            axisOfRotation = br.ReadVector4();
            angle = br.ReadSingle();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(degreesPerSecond);
            bw.WriteSingle(speedMultiplier);
            bw.Position += 8;
            bw.WriteVector4(axisOfRotation);
            bw.WriteSingle(angle);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            degreesPerSecond = xd.ReadSingle(xe, nameof(degreesPerSecond));
            speedMultiplier = xd.ReadSingle(xe, nameof(speedMultiplier));
            axisOfRotation = xd.ReadVector4(xe, nameof(axisOfRotation));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(degreesPerSecond), degreesPerSecond);
            xs.WriteFloat(xe, nameof(speedMultiplier), speedMultiplier);
            xs.WriteVector4(xe, nameof(axisOfRotation), axisOfRotation);
            xs.WriteSerializeIgnored(xe, nameof(angle));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbRotateCharacterModifier);
        }

        public bool Equals(hkbRotateCharacterModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   degreesPerSecond.Equals(other.degreesPerSecond) &&
                   speedMultiplier.Equals(other.speedMultiplier) &&
                   axisOfRotation.Equals(other.axisOfRotation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(degreesPerSecond);
            hashcode.Add(speedMultiplier);
            hashcode.Add(axisOfRotation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

