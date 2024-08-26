using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSInterpValueModifier Signatire: 0x29adc802 size: 104 flags: FLAGS_NONE

    // source class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // target class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // result class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // gain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // timeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSInterpValueModifier : hkbModifier, IEquatable<BSInterpValueModifier?>
    {
        public float source { set; get; }
        public float target { set; get; }
        public float result { set; get; }
        public float gain { set; get; }
        private float timeStep { set; get; }

        public override uint Signature { set; get; } = 0x29adc802;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            source = br.ReadSingle();
            target = br.ReadSingle();
            result = br.ReadSingle();
            gain = br.ReadSingle();
            timeStep = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(source);
            bw.WriteSingle(target);
            bw.WriteSingle(result);
            bw.WriteSingle(gain);
            bw.WriteSingle(timeStep);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            source = xd.ReadSingle(xe, nameof(source));
            target = xd.ReadSingle(xe, nameof(target));
            result = xd.ReadSingle(xe, nameof(result));
            gain = xd.ReadSingle(xe, nameof(gain));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(source), source);
            xs.WriteFloat(xe, nameof(target), target);
            xs.WriteFloat(xe, nameof(result), result);
            xs.WriteFloat(xe, nameof(gain), gain);
            xs.WriteSerializeIgnored(xe, nameof(timeStep));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSInterpValueModifier);
        }

        public bool Equals(BSInterpValueModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   source.Equals(other.source) &&
                   target.Equals(other.target) &&
                   result.Equals(other.result) &&
                   gain.Equals(other.gain) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(source);
            hashcode.Add(target);
            hashcode.Add(result);
            hashcode.Add(gain);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

