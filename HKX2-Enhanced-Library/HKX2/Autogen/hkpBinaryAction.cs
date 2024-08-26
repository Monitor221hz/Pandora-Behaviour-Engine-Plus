using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBinaryAction Signatire: 0xc00f3403 size: 64 flags: FLAGS_NONE

    // entityA class: hkpEntity Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // entityB class: hkpEntity Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    public partial class hkpBinaryAction : hkpAction, IEquatable<hkpBinaryAction?>
    {
        public hkpEntity? entityA { set; get; }
        public hkpEntity? entityB { set; get; }

        public override uint Signature { set; get; } = 0xc00f3403;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            entityA = des.ReadClassPointer<hkpEntity>(br);
            entityB = des.ReadClassPointer<hkpEntity>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, entityA);
            s.WriteClassPointer(bw, entityB);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            entityA = xd.ReadClassPointer<hkpEntity>(this, xe, nameof(entityA));
            entityB = xd.ReadClassPointer<hkpEntity>(this, xe, nameof(entityB));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(entityA), entityA);
            xs.WriteClassPointer(xe, nameof(entityB), entityB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBinaryAction);
        }

        public bool Equals(hkpBinaryAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((entityA is null && other.entityA is null) || (entityA is not null && other.entityA is not null && entityA.Equals((IHavokObject)other.entityA))) &&
                   ((entityB is null && other.entityB is null) || (entityB is not null && other.entityB is not null && entityB.Equals((IHavokObject)other.entityB))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(entityA);
            hashcode.Add(entityB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

