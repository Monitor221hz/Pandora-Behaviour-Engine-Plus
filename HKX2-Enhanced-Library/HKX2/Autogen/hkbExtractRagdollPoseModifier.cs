using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbExtractRagdollPoseModifier Signatire: 0x804dcbab size: 88 flags: FLAGS_NONE

    // poseMatchingBone0 class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // poseMatchingBone1 class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 82 flags: FLAGS_NONE enum: 
    // poseMatchingBone2 class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // enableComputeWorldFromModel class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 86 flags: FLAGS_NONE enum: 
    public partial class hkbExtractRagdollPoseModifier : hkbModifier, IEquatable<hkbExtractRagdollPoseModifier?>
    {
        public short poseMatchingBone0 { set; get; }
        public short poseMatchingBone1 { set; get; }
        public short poseMatchingBone2 { set; get; }
        public bool enableComputeWorldFromModel { set; get; }

        public override uint Signature { set; get; } = 0x804dcbab;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            poseMatchingBone0 = br.ReadInt16();
            poseMatchingBone1 = br.ReadInt16();
            poseMatchingBone2 = br.ReadInt16();
            enableComputeWorldFromModel = br.ReadBoolean();
            br.Position += 1;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt16(poseMatchingBone0);
            bw.WriteInt16(poseMatchingBone1);
            bw.WriteInt16(poseMatchingBone2);
            bw.WriteBoolean(enableComputeWorldFromModel);
            bw.Position += 1;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            poseMatchingBone0 = xd.ReadInt16(xe, nameof(poseMatchingBone0));
            poseMatchingBone1 = xd.ReadInt16(xe, nameof(poseMatchingBone1));
            poseMatchingBone2 = xd.ReadInt16(xe, nameof(poseMatchingBone2));
            enableComputeWorldFromModel = xd.ReadBoolean(xe, nameof(enableComputeWorldFromModel));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(poseMatchingBone0), poseMatchingBone0);
            xs.WriteNumber(xe, nameof(poseMatchingBone1), poseMatchingBone1);
            xs.WriteNumber(xe, nameof(poseMatchingBone2), poseMatchingBone2);
            xs.WriteBoolean(xe, nameof(enableComputeWorldFromModel), enableComputeWorldFromModel);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbExtractRagdollPoseModifier);
        }

        public bool Equals(hkbExtractRagdollPoseModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   poseMatchingBone0.Equals(other.poseMatchingBone0) &&
                   poseMatchingBone1.Equals(other.poseMatchingBone1) &&
                   poseMatchingBone2.Equals(other.poseMatchingBone2) &&
                   enableComputeWorldFromModel.Equals(other.enableComputeWorldFromModel) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(poseMatchingBone0);
            hashcode.Add(poseMatchingBone1);
            hashcode.Add(poseMatchingBone2);
            hashcode.Add(enableComputeWorldFromModel);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

