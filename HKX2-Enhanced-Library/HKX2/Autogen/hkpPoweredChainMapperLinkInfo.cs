using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPoweredChainMapperLinkInfo Signatire: 0xcf071a1b size: 16 flags: FLAGS_NONE

    // firstTargetIdx class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // numTargets class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // limitConstraint class: hkpConstraintInstance Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpPoweredChainMapperLinkInfo : IHavokObject, IEquatable<hkpPoweredChainMapperLinkInfo?>
    {
        public int firstTargetIdx { set; get; }
        public int numTargets { set; get; }
        public hkpConstraintInstance? limitConstraint { set; get; }

        public virtual uint Signature { set; get; } = 0xcf071a1b;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            firstTargetIdx = br.ReadInt32();
            numTargets = br.ReadInt32();
            limitConstraint = des.ReadClassPointer<hkpConstraintInstance>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(firstTargetIdx);
            bw.WriteInt32(numTargets);
            s.WriteClassPointer(bw, limitConstraint);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            firstTargetIdx = xd.ReadInt32(xe, nameof(firstTargetIdx));
            numTargets = xd.ReadInt32(xe, nameof(numTargets));
            limitConstraint = xd.ReadClassPointer<hkpConstraintInstance>(this, xe, nameof(limitConstraint));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(firstTargetIdx), firstTargetIdx);
            xs.WriteNumber(xe, nameof(numTargets), numTargets);
            xs.WriteClassPointer(xe, nameof(limitConstraint), limitConstraint);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPoweredChainMapperLinkInfo);
        }

        public bool Equals(hkpPoweredChainMapperLinkInfo? other)
        {
            return other is not null &&
                   firstTargetIdx.Equals(other.firstTargetIdx) &&
                   numTargets.Equals(other.numTargets) &&
                   ((limitConstraint is null && other.limitConstraint is null) || (limitConstraint is not null && other.limitConstraint is not null && limitConstraint.Equals((IHavokObject)other.limitConstraint))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(firstTargetIdx);
            hashcode.Add(numTargets);
            hashcode.Add(limitConstraint);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

