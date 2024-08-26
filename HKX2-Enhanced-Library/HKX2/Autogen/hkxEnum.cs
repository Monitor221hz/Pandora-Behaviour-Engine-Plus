using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxEnum Signatire: 0xc4e1211 size: 32 flags: FLAGS_NONE

    // items class: hkxEnumItem Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxEnum : hkReferencedObject, IEquatable<hkxEnum?>
    {
        public IList<hkxEnumItem> items { set; get; } = Array.Empty<hkxEnumItem>();

        public override uint Signature { set; get; } = 0xc4e1211;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            items = des.ReadClassArray<hkxEnumItem>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, items);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            items = xd.ReadClassArray<hkxEnumItem>(xe, nameof(items));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(items), items);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxEnum);
        }

        public bool Equals(hkxEnum? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   items.SequenceEqual(other.items) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(items.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

