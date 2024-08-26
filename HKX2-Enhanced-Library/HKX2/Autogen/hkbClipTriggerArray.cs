using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbClipTriggerArray Signatire: 0x59c23a0f size: 32 flags: FLAGS_NONE

    // triggers class: hkbClipTrigger Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbClipTriggerArray : hkReferencedObject, IEquatable<hkbClipTriggerArray?>
    {
        public IList<hkbClipTrigger> triggers { set; get; } = Array.Empty<hkbClipTrigger>();

        public override uint Signature { set; get; } = 0x59c23a0f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            triggers = des.ReadClassArray<hkbClipTrigger>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, triggers);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            triggers = xd.ReadClassArray<hkbClipTrigger>(xe, nameof(triggers));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(triggers), triggers);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbClipTriggerArray);
        }

        public bool Equals(hkbClipTriggerArray? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   triggers.SequenceEqual(other.triggers) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(triggers.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

