using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineEventPropertyArray Signatire: 0xb07b4388 size: 32 flags: FLAGS_NONE

    // events class: hkbEventProperty Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbStateMachineEventPropertyArray : hkReferencedObject, IEquatable<hkbStateMachineEventPropertyArray?>
    {
        public IList<hkbEventProperty> events { set; get; } = Array.Empty<hkbEventProperty>();

        public override uint Signature { set; get; } = 0xb07b4388;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            events = des.ReadClassArray<hkbEventProperty>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, events);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            events = xd.ReadClassArray<hkbEventProperty>(xe, nameof(events));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(events), events);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineEventPropertyArray);
        }

        public bool Equals(hkbStateMachineEventPropertyArray? other)
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

