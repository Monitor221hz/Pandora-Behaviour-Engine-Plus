using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpUnaryAction Signatire: 0x895532c0 size: 56 flags: FLAGS_NONE

    // entity class: hkpEntity Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpUnaryAction : hkpAction, IEquatable<hkpUnaryAction?>
    {
        public hkpEntity? entity { set; get; }

        public override uint Signature { set; get; } = 0x895532c0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            entity = des.ReadClassPointer<hkpEntity>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, entity);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            entity = xd.ReadClassPointer<hkpEntity>(this, xe, nameof(entity));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(entity), entity);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpUnaryAction);
        }

        public bool Equals(hkpUnaryAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((entity is null && other.entity is null) || (entity is not null && other.entity is not null && entity.Equals((IHavokObject)other.entity))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(entity);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

