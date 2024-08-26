using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbKeyframeBonesModifier Signatire: 0x95f66629 size: 104 flags: FLAGS_NONE

    // keyframeInfo class: hkbKeyframeBonesModifierKeyframeInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // keyframedBonesList class: hkbBoneIndexArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    public partial class hkbKeyframeBonesModifier : hkbModifier, IEquatable<hkbKeyframeBonesModifier?>
    {
        public IList<hkbKeyframeBonesModifierKeyframeInfo> keyframeInfo { set; get; } = Array.Empty<hkbKeyframeBonesModifierKeyframeInfo>();
        public hkbBoneIndexArray? keyframedBonesList { set; get; }

        public override uint Signature { set; get; } = 0x95f66629;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            keyframeInfo = des.ReadClassArray<hkbKeyframeBonesModifierKeyframeInfo>(br);
            keyframedBonesList = des.ReadClassPointer<hkbBoneIndexArray>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, keyframeInfo);
            s.WriteClassPointer(bw, keyframedBonesList);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            keyframeInfo = xd.ReadClassArray<hkbKeyframeBonesModifierKeyframeInfo>(xe, nameof(keyframeInfo));
            keyframedBonesList = xd.ReadClassPointer<hkbBoneIndexArray>(this, xe, nameof(keyframedBonesList));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(keyframeInfo), keyframeInfo);
            xs.WriteClassPointer(xe, nameof(keyframedBonesList), keyframedBonesList);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbKeyframeBonesModifier);
        }

        public bool Equals(hkbKeyframeBonesModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   keyframeInfo.SequenceEqual(other.keyframeInfo) &&
                   ((keyframedBonesList is null && other.keyframedBonesList is null) || (keyframedBonesList is not null && other.keyframedBonesList is not null && keyframedBonesList.Equals((IHavokObject)other.keyframedBonesList))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(keyframeInfo.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(keyframedBonesList);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

