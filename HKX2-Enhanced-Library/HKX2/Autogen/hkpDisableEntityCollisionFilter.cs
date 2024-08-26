using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpDisableEntityCollisionFilter Signatire: 0xfac3351c size: 96 flags: FLAGS_NONE

    // disabledEntities class: hkpEntity Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkpDisableEntityCollisionFilter : hkpCollisionFilter, IEquatable<hkpDisableEntityCollisionFilter?>
    {
        public IList<hkpEntity> disabledEntities { set; get; } = Array.Empty<hkpEntity>();

        public override uint Signature { set; get; } = 0xfac3351c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            disabledEntities = des.ReadClassPointerArray<hkpEntity>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            s.WriteClassPointerArray(bw, disabledEntities);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            disabledEntities = xd.ReadClassPointerArray<hkpEntity>(this, xe, nameof(disabledEntities));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(disabledEntities), disabledEntities!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpDisableEntityCollisionFilter);
        }

        public bool Equals(hkpDisableEntityCollisionFilter? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   disabledEntities.SequenceEqual(other.disabledEntities) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(disabledEntities.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

