using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCollisionFilterList Signatire: 0x2603bf04 size: 88 flags: FLAGS_NONE

    // collisionFilters class: hkpCollisionFilter Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    public partial class hkpCollisionFilterList : hkpCollisionFilter, IEquatable<hkpCollisionFilterList?>
    {
        public IList<hkpCollisionFilter> collisionFilters { set; get; } = Array.Empty<hkpCollisionFilter>();

        public override uint Signature { set; get; } = 0x2603bf04;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            collisionFilters = des.ReadClassPointerArray<hkpCollisionFilter>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, collisionFilters);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            collisionFilters = xd.ReadClassPointerArray<hkpCollisionFilter>(this, xe, nameof(collisionFilters));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(collisionFilters), collisionFilters!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCollisionFilterList);
        }

        public bool Equals(hkpCollisionFilterList? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   collisionFilters.SequenceEqual(other.collisionFilters) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(collisionFilters.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

