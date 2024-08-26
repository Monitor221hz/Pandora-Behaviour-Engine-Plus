using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEventsFromRangeModifier Signatire: 0xbc561b6e size: 112 flags: FLAGS_NONE

    // inputValue class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // lowerBound class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // eventRanges class: hkbEventRangeDataArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // wasActiveInPreviousFrame class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbEventsFromRangeModifier : hkbModifier, IEquatable<hkbEventsFromRangeModifier?>
    {
        public float inputValue { set; get; }
        public float lowerBound { set; get; }
        public hkbEventRangeDataArray? eventRanges { set; get; }
        public IList<object> wasActiveInPreviousFrame { set; get; } = Array.Empty<object>();

        public override uint Signature { set; get; } = 0xbc561b6e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            inputValue = br.ReadSingle();
            lowerBound = br.ReadSingle();
            eventRanges = des.ReadClassPointer<hkbEventRangeDataArray>(br);
            des.ReadEmptyArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(inputValue);
            bw.WriteSingle(lowerBound);
            s.WriteClassPointer(bw, eventRanges);
            s.WriteVoidArray(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            inputValue = xd.ReadSingle(xe, nameof(inputValue));
            lowerBound = xd.ReadSingle(xe, nameof(lowerBound));
            eventRanges = xd.ReadClassPointer<hkbEventRangeDataArray>(this, xe, nameof(eventRanges));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(inputValue), inputValue);
            xs.WriteFloat(xe, nameof(lowerBound), lowerBound);
            xs.WriteClassPointer(xe, nameof(eventRanges), eventRanges);
            xs.WriteSerializeIgnored(xe, nameof(wasActiveInPreviousFrame));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEventsFromRangeModifier);
        }

        public bool Equals(hkbEventsFromRangeModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   inputValue.Equals(other.inputValue) &&
                   lowerBound.Equals(other.lowerBound) &&
                   ((eventRanges is null && other.eventRanges is null) || (eventRanges is not null && other.eventRanges is not null && eventRanges.Equals((IHavokObject)other.eventRanges))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(inputValue);
            hashcode.Add(lowerBound);
            hashcode.Add(eventRanges);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

