using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConstrainedSystemFilter Signatire: 0x20a447fe size: 88 flags: FLAGS_NONE

    // otherFilter class: hkpCollisionFilter Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkpConstrainedSystemFilter : hkpCollisionFilter, IEquatable<hkpConstrainedSystemFilter?>
    {
        public hkpCollisionFilter? otherFilter { set; get; }

        public override uint Signature { set; get; } = 0x20a447fe;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            otherFilter = des.ReadClassPointer<hkpCollisionFilter>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            s.WriteClassPointer(bw, otherFilter);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            otherFilter = xd.ReadClassPointer<hkpCollisionFilter>(this, xe, nameof(otherFilter));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(otherFilter), otherFilter);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConstrainedSystemFilter);
        }

        public bool Equals(hkpConstrainedSystemFilter? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((otherFilter is null && other.otherFilter is null) || (otherFilter is not null && other.otherFilter is not null && otherFilter.Equals((IHavokObject)other.otherFilter))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(otherFilter);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

