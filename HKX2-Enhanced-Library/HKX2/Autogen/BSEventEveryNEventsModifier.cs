using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSEventEveryNEventsModifier Signatire: 0x6030970c size: 128 flags: FLAGS_NONE

    // eventToCheckFor class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // eventToSend class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // numberOfEventsBeforeSend class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // minimumNumberOfEventsBeforeSend class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 113 flags: FLAGS_NONE enum: 
    // randomizeNumberOfEvents class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 114 flags: FLAGS_NONE enum: 
    // numberOfEventsSeen class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 116 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // calculatedNumberOfEventsBeforeSend class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSEventEveryNEventsModifier : hkbModifier, IEquatable<BSEventEveryNEventsModifier?>
    {
        public hkbEventProperty eventToCheckFor { set; get; } = new();
        public hkbEventProperty eventToSend { set; get; } = new();
        public sbyte numberOfEventsBeforeSend { set; get; }
        public sbyte minimumNumberOfEventsBeforeSend { set; get; }
        public bool randomizeNumberOfEvents { set; get; }
        private int numberOfEventsSeen { set; get; }
        private sbyte calculatedNumberOfEventsBeforeSend { set; get; }

        public override uint Signature { set; get; } = 0x6030970c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            eventToCheckFor.Read(des, br);
            eventToSend.Read(des, br);
            numberOfEventsBeforeSend = br.ReadSByte();
            minimumNumberOfEventsBeforeSend = br.ReadSByte();
            randomizeNumberOfEvents = br.ReadBoolean();
            br.Position += 1;
            numberOfEventsSeen = br.ReadInt32();
            calculatedNumberOfEventsBeforeSend = br.ReadSByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            eventToCheckFor.Write(s, bw);
            eventToSend.Write(s, bw);
            bw.WriteSByte(numberOfEventsBeforeSend);
            bw.WriteSByte(minimumNumberOfEventsBeforeSend);
            bw.WriteBoolean(randomizeNumberOfEvents);
            bw.Position += 1;
            bw.WriteInt32(numberOfEventsSeen);
            bw.WriteSByte(calculatedNumberOfEventsBeforeSend);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            eventToCheckFor = xd.ReadClass<hkbEventProperty>(xe, nameof(eventToCheckFor));
            eventToSend = xd.ReadClass<hkbEventProperty>(xe, nameof(eventToSend));
            numberOfEventsBeforeSend = xd.ReadSByte(xe, nameof(numberOfEventsBeforeSend));
            minimumNumberOfEventsBeforeSend = xd.ReadSByte(xe, nameof(minimumNumberOfEventsBeforeSend));
            randomizeNumberOfEvents = xd.ReadBoolean(xe, nameof(randomizeNumberOfEvents));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbEventProperty>(xe, nameof(eventToCheckFor), eventToCheckFor);
            xs.WriteClass<hkbEventProperty>(xe, nameof(eventToSend), eventToSend);
            xs.WriteNumber(xe, nameof(numberOfEventsBeforeSend), numberOfEventsBeforeSend);
            xs.WriteNumber(xe, nameof(minimumNumberOfEventsBeforeSend), minimumNumberOfEventsBeforeSend);
            xs.WriteBoolean(xe, nameof(randomizeNumberOfEvents), randomizeNumberOfEvents);
            xs.WriteSerializeIgnored(xe, nameof(numberOfEventsSeen));
            xs.WriteSerializeIgnored(xe, nameof(calculatedNumberOfEventsBeforeSend));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSEventEveryNEventsModifier);
        }

        public bool Equals(BSEventEveryNEventsModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((eventToCheckFor is null && other.eventToCheckFor is null) || (eventToCheckFor is not null && other.eventToCheckFor is not null && eventToCheckFor.Equals((IHavokObject)other.eventToCheckFor))) &&
                   ((eventToSend is null && other.eventToSend is null) || (eventToSend is not null && other.eventToSend is not null && eventToSend.Equals((IHavokObject)other.eventToSend))) &&
                   numberOfEventsBeforeSend.Equals(other.numberOfEventsBeforeSend) &&
                   minimumNumberOfEventsBeforeSend.Equals(other.minimumNumberOfEventsBeforeSend) &&
                   randomizeNumberOfEvents.Equals(other.randomizeNumberOfEvents) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(eventToCheckFor);
            hashcode.Add(eventToSend);
            hashcode.Add(numberOfEventsBeforeSend);
            hashcode.Add(minimumNumberOfEventsBeforeSend);
            hashcode.Add(randomizeNumberOfEvents);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

