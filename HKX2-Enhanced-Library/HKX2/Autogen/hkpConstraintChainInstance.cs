using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConstraintChainInstance Signatire: 0x7a490753 size: 136 flags: FLAGS_NONE

    // chainedEntities class: hkpEntity Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // action class: hkpConstraintChainInstanceAction Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    public partial class hkpConstraintChainInstance : hkpConstraintInstance, IEquatable<hkpConstraintChainInstance?>
    {
        public IList<hkpEntity> chainedEntities { set; get; } = Array.Empty<hkpEntity>();
        public hkpConstraintChainInstanceAction? action { set; get; }

        public override uint Signature { set; get; } = 0x7a490753;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            chainedEntities = des.ReadClassPointerArray<hkpEntity>(br);
            action = des.ReadClassPointer<hkpConstraintChainInstanceAction>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, chainedEntities);
            s.WriteClassPointer(bw, action);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            chainedEntities = xd.ReadClassPointerArray<hkpEntity>(this, xe, nameof(chainedEntities));
            action = xd.ReadClassPointer<hkpConstraintChainInstanceAction>(this, xe, nameof(action));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(chainedEntities), chainedEntities!);
            xs.WriteClassPointer(xe, nameof(action), action);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConstraintChainInstance);
        }

        public bool Equals(hkpConstraintChainInstance? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   chainedEntities.SequenceEqual(other.chainedEntities) &&
                   ((action is null && other.action is null) || (action is not null && other.action is not null && action.Equals((IHavokObject)other.action))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(chainedEntities.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(action);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

