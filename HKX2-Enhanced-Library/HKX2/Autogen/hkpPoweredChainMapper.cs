using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPoweredChainMapper Signatire: 0x7a77ef5 size: 64 flags: FLAGS_NONE

    // links class: hkpPoweredChainMapperLinkInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // targets class: hkpPoweredChainMapperTarget Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // chains class: hkpConstraintChainInstance Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpPoweredChainMapper : hkReferencedObject, IEquatable<hkpPoweredChainMapper?>
    {
        public IList<hkpPoweredChainMapperLinkInfo> links { set; get; } = Array.Empty<hkpPoweredChainMapperLinkInfo>();
        public IList<hkpPoweredChainMapperTarget> targets { set; get; } = Array.Empty<hkpPoweredChainMapperTarget>();
        public IList<hkpConstraintChainInstance> chains { set; get; } = Array.Empty<hkpConstraintChainInstance>();

        public override uint Signature { set; get; } = 0x7a77ef5;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            links = des.ReadClassArray<hkpPoweredChainMapperLinkInfo>(br);
            targets = des.ReadClassArray<hkpPoweredChainMapperTarget>(br);
            chains = des.ReadClassPointerArray<hkpConstraintChainInstance>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, links);
            s.WriteClassArray(bw, targets);
            s.WriteClassPointerArray(bw, chains);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            links = xd.ReadClassArray<hkpPoweredChainMapperLinkInfo>(xe, nameof(links));
            targets = xd.ReadClassArray<hkpPoweredChainMapperTarget>(xe, nameof(targets));
            chains = xd.ReadClassPointerArray<hkpConstraintChainInstance>(this, xe, nameof(chains));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(links), links);
            xs.WriteClassArray(xe, nameof(targets), targets);
            xs.WriteClassPointerArray(xe, nameof(chains), chains!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPoweredChainMapper);
        }

        public bool Equals(hkpPoweredChainMapper? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   links.SequenceEqual(other.links) &&
                   targets.SequenceEqual(other.targets) &&
                   chains.SequenceEqual(other.chains) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(links.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(targets.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(chains.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

