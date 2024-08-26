using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEventRangeData Signatire: 0x6cb92c76 size: 32 flags: FLAGS_NONE

    // upperBound class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // @eventclass: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // eventMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: EventRangeMode
    public partial class hkbEventRangeData : IHavokObject, IEquatable<hkbEventRangeData?>
    {
        public float upperBound { set; get; }
        public hkbEventProperty @event{ set; get; } = new();
        public sbyte eventMode { set; get; }

        public virtual uint Signature { set; get; } = 0x6cb92c76;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            upperBound = br.ReadSingle();
            br.Position += 4;
            @event.Read(des, br);
            eventMode = br.ReadSByte();
            br.Position += 7;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(upperBound);
            bw.Position += 4;
            @event.Write(s, bw);
            bw.WriteSByte(eventMode);
            bw.Position += 7;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            upperBound = xd.ReadSingle(xe, nameof(upperBound));
            @event= xd.ReadClass<hkbEventProperty>(xe, nameof(@event));
            eventMode = xd.ReadFlag<EventRangeMode, sbyte>(xe, nameof(eventMode));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(upperBound), upperBound);
            xs.WriteClass<hkbEventProperty>(xe, nameof(@event), @event);
            xs.WriteEnum<EventRangeMode, sbyte>(xe, nameof(eventMode), eventMode);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEventRangeData);
        }

        public bool Equals(hkbEventRangeData? other)
        {
            return other is not null &&
                   upperBound.Equals(other.upperBound) &&
                   ((@event is null && other.@event is null) || (@event is not null && other.@event is not null && @event.Equals((IHavokObject)other.@event))) &&
                   eventMode.Equals(other.eventMode) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(upperBound);
            hashcode.Add(@event);
            hashcode.Add(eventMode);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

