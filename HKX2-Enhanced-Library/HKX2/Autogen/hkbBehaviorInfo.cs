using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBehaviorInfo Signatire: 0xf7645395 size: 48 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // data class: hkbBehaviorGraphData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // idToNamePairs class: hkbBehaviorInfoIdToNamePair Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkbBehaviorInfo : hkReferencedObject, IEquatable<hkbBehaviorInfo?>
    {
        public ulong characterId { set; get; }
        public hkbBehaviorGraphData? data { set; get; }
        public IList<hkbBehaviorInfoIdToNamePair> idToNamePairs { set; get; } = Array.Empty<hkbBehaviorInfoIdToNamePair>();

        public override uint Signature { set; get; } = 0xf7645395;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            data = des.ReadClassPointer<hkbBehaviorGraphData>(br);
            idToNamePairs = des.ReadClassArray<hkbBehaviorInfoIdToNamePair>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            s.WriteClassPointer(bw, data);
            s.WriteClassArray(bw, idToNamePairs);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            data = xd.ReadClassPointer<hkbBehaviorGraphData>(this, xe, nameof(data));
            idToNamePairs = xd.ReadClassArray<hkbBehaviorInfoIdToNamePair>(xe, nameof(idToNamePairs));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteClassPointer(xe, nameof(data), data);
            xs.WriteClassArray(xe, nameof(idToNamePairs), idToNamePairs);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBehaviorInfo);
        }

        public bool Equals(hkbBehaviorInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   ((data is null && other.data is null) || (data is not null && other.data is not null && data.Equals((IHavokObject)other.data))) &&
                   idToNamePairs.SequenceEqual(other.idToNamePairs) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(data);
            hashcode.Add(idToNamePairs.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

