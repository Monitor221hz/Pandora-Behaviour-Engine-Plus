using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEventPayloadList Signatire: 0x3d2dbd34 size: 32 flags: FLAGS_NONE

    // payloads class: hkbEventPayload Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbEventPayloadList : hkbEventPayload, IEquatable<hkbEventPayloadList?>
    {
        public IList<hkbEventPayload> payloads { set; get; } = Array.Empty<hkbEventPayload>();

        public override uint Signature { set; get; } = 0x3d2dbd34;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            payloads = des.ReadClassPointerArray<hkbEventPayload>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, payloads);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            payloads = xd.ReadClassPointerArray<hkbEventPayload>(this, xe, nameof(payloads));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(payloads), payloads!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEventPayloadList);
        }

        public bool Equals(hkbEventPayloadList? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   payloads.SequenceEqual(other.payloads) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(payloads.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

