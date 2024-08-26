using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSequenceStringData Signatire: 0x6a5094e3 size: 48 flags: FLAGS_NONE

    // eventNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // variableNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkbSequenceStringData : hkReferencedObject, IEquatable<hkbSequenceStringData?>
    {
        public IList<string> eventNames { set; get; } = Array.Empty<string>();
        public IList<string> variableNames { set; get; } = Array.Empty<string>();

        public override uint Signature { set; get; } = 0x6a5094e3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            eventNames = des.ReadStringPointerArray(br);
            variableNames = des.ReadStringPointerArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointerArray(bw, eventNames);
            s.WriteStringPointerArray(bw, variableNames);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            eventNames = xd.ReadStringArray(xe, nameof(eventNames));
            variableNames = xd.ReadStringArray(xe, nameof(variableNames));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteStringArray(xe, nameof(eventNames), eventNames);
            xs.WriteStringArray(xe, nameof(variableNames), variableNames);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSequenceStringData);
        }

        public bool Equals(hkbSequenceStringData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   eventNames.SequenceEqual(other.eventNames) &&
                   variableNames.SequenceEqual(other.variableNames) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(eventNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(variableNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

