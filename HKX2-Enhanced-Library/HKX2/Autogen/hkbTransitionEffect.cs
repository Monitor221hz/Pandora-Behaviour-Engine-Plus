using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbTransitionEffect Signatire: 0x945da157 size: 80 flags: FLAGS_NONE

    // selfTransitionMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 72 flags: FLAGS_NONE enum: SelfTransitionMode
    // eventMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 73 flags: FLAGS_NONE enum: EventMode
    // defaultEventMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 74 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbTransitionEffect : hkbGenerator, IEquatable<hkbTransitionEffect?>
    {
        public sbyte selfTransitionMode { set; get; }
        public sbyte eventMode { set; get; }
        private sbyte defaultEventMode { set; get; }

        public override uint Signature { set; get; } = 0x945da157;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            selfTransitionMode = br.ReadSByte();
            eventMode = br.ReadSByte();
            defaultEventMode = br.ReadSByte();
            br.Position += 5;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSByte(selfTransitionMode);
            bw.WriteSByte(eventMode);
            bw.WriteSByte(defaultEventMode);
            bw.Position += 5;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            selfTransitionMode = xd.ReadFlag<SelfTransitionMode, sbyte>(xe, nameof(selfTransitionMode));
            eventMode = xd.ReadFlag<EventMode, sbyte>(xe, nameof(eventMode));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<SelfTransitionMode, sbyte>(xe, nameof(selfTransitionMode), selfTransitionMode);
            xs.WriteEnum<EventMode, sbyte>(xe, nameof(eventMode), eventMode);
            xs.WriteSerializeIgnored(xe, nameof(defaultEventMode));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbTransitionEffect);
        }

        public bool Equals(hkbTransitionEffect? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   selfTransitionMode.Equals(other.selfTransitionMode) &&
                   eventMode.Equals(other.eventMode) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(selfTransitionMode);
            hashcode.Add(eventMode);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

