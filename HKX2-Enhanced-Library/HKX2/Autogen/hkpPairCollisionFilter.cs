using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPairCollisionFilter Signatire: 0x4abc140e size: 96 flags: FLAGS_NONE

    // disabledPairs class: hkpPairCollisionFilterMapPairFilterKeyOverrideType Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 72 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // childFilter class: hkpCollisionFilter Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    public partial class hkpPairCollisionFilter : hkpCollisionFilter, IEquatable<hkpPairCollisionFilter?>
    {
        public hkpPairCollisionFilterMapPairFilterKeyOverrideType disabledPairs { set; get; } = new();
        public hkpCollisionFilter? childFilter { set; get; }

        public override uint Signature { set; get; } = 0x4abc140e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            disabledPairs.Read(des, br);
            childFilter = des.ReadClassPointer<hkpCollisionFilter>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            disabledPairs.Write(s, bw);
            s.WriteClassPointer(bw, childFilter);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            childFilter = xd.ReadClassPointer<hkpCollisionFilter>(this, xe, nameof(childFilter));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(disabledPairs));
            xs.WriteClassPointer(xe, nameof(childFilter), childFilter);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPairCollisionFilter);
        }

        public bool Equals(hkpPairCollisionFilter? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((childFilter is null && other.childFilter is null) || (childFilter is not null && other.childFilter is not null && childFilter.Equals((IHavokObject)other.childFilter))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(childFilter);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

