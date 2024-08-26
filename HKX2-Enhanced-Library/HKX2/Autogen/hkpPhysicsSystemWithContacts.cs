using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPhysicsSystemWithContacts Signatire: 0xd0fd4bbe size: 120 flags: FLAGS_NONE

    // contacts class: hkpSerializedAgentNnEntry Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    public partial class hkpPhysicsSystemWithContacts : hkpPhysicsSystem, IEquatable<hkpPhysicsSystemWithContacts?>
    {
        public IList<hkpSerializedAgentNnEntry> contacts { set; get; } = Array.Empty<hkpSerializedAgentNnEntry>();

        public override uint Signature { set; get; } = 0xd0fd4bbe;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            contacts = des.ReadClassPointerArray<hkpSerializedAgentNnEntry>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, contacts);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            contacts = xd.ReadClassPointerArray<hkpSerializedAgentNnEntry>(this, xe, nameof(contacts));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(contacts), contacts!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPhysicsSystemWithContacts);
        }

        public bool Equals(hkpPhysicsSystemWithContacts? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   contacts.SequenceEqual(other.contacts) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(contacts.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

