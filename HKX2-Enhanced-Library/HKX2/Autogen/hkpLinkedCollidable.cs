using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpLinkedCollidable Signatire: 0xe1a81497 size: 128 flags: FLAGS_NONE

    // collisionEntries class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpLinkedCollidable : hkpCollidable, IEquatable<hkpLinkedCollidable?>
    {
        public IList<object> collisionEntries { set; get; } = Array.Empty<object>();

        public override uint Signature { set; get; } = 0xe1a81497;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidArray(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(collisionEntries));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpLinkedCollidable);
        }

        public bool Equals(hkpLinkedCollidable? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

