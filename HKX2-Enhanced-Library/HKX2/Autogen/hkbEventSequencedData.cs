using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEventSequencedData Signatire: 0x76798eb8 size: 32 flags: FLAGS_NONE

    // events class: hkbEventSequencedDataSequencedEvent Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbEventSequencedData : hkbSequencedData, IEquatable<hkbEventSequencedData?>
    {
        public IList<hkbEventSequencedDataSequencedEvent> events { set; get; } = Array.Empty<hkbEventSequencedDataSequencedEvent>();

        public override uint Signature { set; get; } = 0x76798eb8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            events = des.ReadClassArray<hkbEventSequencedDataSequencedEvent>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, events);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            events = xd.ReadClassArray<hkbEventSequencedDataSequencedEvent>(xe, nameof(events));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(events), events);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEventSequencedData);
        }

        public bool Equals(hkbEventSequencedData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   events.SequenceEqual(other.events) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(events.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

