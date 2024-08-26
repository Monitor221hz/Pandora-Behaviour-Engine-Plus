using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpArrayAction Signatire: 0x674bcd2d size: 64 flags: FLAGS_NONE

    // entities class: hkpEntity Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpArrayAction : hkpAction, IEquatable<hkpArrayAction?>
    {
        public IList<hkpEntity> entities { set; get; } = Array.Empty<hkpEntity>();

        public override uint Signature { set; get; } = 0x674bcd2d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            entities = des.ReadClassPointerArray<hkpEntity>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, entities);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            entities = xd.ReadClassPointerArray<hkpEntity>(this, xe, nameof(entities));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(entities), entities!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpArrayAction);
        }

        public bool Equals(hkpArrayAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   entities.SequenceEqual(other.entities) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(entities.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

