using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSBoneSwitchGeneratorBoneData Signatire: 0xc1215be6 size: 64 flags: FLAGS_NONE

    // pGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: ALIGN_16|FLAGS_NONE enum: 
    // spBoneWeight class: hkbBoneWeightArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    public partial class BSBoneSwitchGeneratorBoneData : hkbBindable, IEquatable<BSBoneSwitchGeneratorBoneData?>
    {
        public hkbGenerator? pGenerator { set; get; }
        public hkbBoneWeightArray? spBoneWeight { set; get; }

        public override uint Signature { set; get; } = 0xc1215be6;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            pGenerator = des.ReadClassPointer<hkbGenerator>(br);
            spBoneWeight = des.ReadClassPointer<hkbBoneWeightArray>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, pGenerator);
            s.WriteClassPointer(bw, spBoneWeight);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(pGenerator));
            spBoneWeight = xd.ReadClassPointer<hkbBoneWeightArray>(this, xe, nameof(spBoneWeight));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(pGenerator), pGenerator);
            xs.WriteClassPointer(xe, nameof(spBoneWeight), spBoneWeight);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSBoneSwitchGeneratorBoneData);
        }

        public bool Equals(BSBoneSwitchGeneratorBoneData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((pGenerator is null && other.pGenerator is null) || (pGenerator is not null && other.pGenerator is not null && pGenerator.Equals((IHavokObject)other.pGenerator))) &&
                   ((spBoneWeight is null && other.spBoneWeight is null) || (spBoneWeight is not null && other.spBoneWeight is not null && spBoneWeight.Equals((IHavokObject)other.spBoneWeight))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pGenerator);
            hashcode.Add(spBoneWeight);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

