using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbDelayedModifier Signatire: 0x8e101a7a size: 104 flags: FLAGS_NONE

    // delaySeconds class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // durationSeconds class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // secondsElapsed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // isActive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 100 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbDelayedModifier : hkbModifierWrapper, IEquatable<hkbDelayedModifier?>
    {
        public float delaySeconds { set; get; }
        public float durationSeconds { set; get; }
        private float secondsElapsed { set; get; }
        private bool isActive { set; get; }

        public override uint Signature { set; get; } = 0x8e101a7a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            delaySeconds = br.ReadSingle();
            durationSeconds = br.ReadSingle();
            secondsElapsed = br.ReadSingle();
            isActive = br.ReadBoolean();
            br.Position += 3;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(delaySeconds);
            bw.WriteSingle(durationSeconds);
            bw.WriteSingle(secondsElapsed);
            bw.WriteBoolean(isActive);
            bw.Position += 3;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            delaySeconds = xd.ReadSingle(xe, nameof(delaySeconds));
            durationSeconds = xd.ReadSingle(xe, nameof(durationSeconds));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(delaySeconds), delaySeconds);
            xs.WriteFloat(xe, nameof(durationSeconds), durationSeconds);
            xs.WriteSerializeIgnored(xe, nameof(secondsElapsed));
            xs.WriteSerializeIgnored(xe, nameof(isActive));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbDelayedModifier);
        }

        public bool Equals(hkbDelayedModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   delaySeconds.Equals(other.delaySeconds) &&
                   durationSeconds.Equals(other.durationSeconds) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(delaySeconds);
            hashcode.Add(durationSeconds);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

