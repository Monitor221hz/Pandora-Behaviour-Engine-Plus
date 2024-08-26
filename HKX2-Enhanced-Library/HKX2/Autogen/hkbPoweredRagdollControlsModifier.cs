using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbPoweredRagdollControlsModifier Signatire: 0x7cb54065 size: 144 flags: FLAGS_NONE

    // controlData class: hkbPoweredRagdollControlData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // bones class: hkbBoneIndexArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // worldFromModelModeData class: hkbWorldFromModelModeData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // boneWeights class: hkbBoneWeightArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    public partial class hkbPoweredRagdollControlsModifier : hkbModifier, IEquatable<hkbPoweredRagdollControlsModifier?>
    {
        public hkbPoweredRagdollControlData controlData { set; get; } = new();
        public hkbBoneIndexArray? bones { set; get; }
        public hkbWorldFromModelModeData worldFromModelModeData { set; get; } = new();
        public hkbBoneWeightArray? boneWeights { set; get; }

        public override uint Signature { set; get; } = 0x7cb54065;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            controlData.Read(des, br);
            bones = des.ReadClassPointer<hkbBoneIndexArray>(br);
            worldFromModelModeData.Read(des, br);
            boneWeights = des.ReadClassPointer<hkbBoneWeightArray>(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            controlData.Write(s, bw);
            s.WriteClassPointer(bw, bones);
            worldFromModelModeData.Write(s, bw);
            s.WriteClassPointer(bw, boneWeights);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            controlData = xd.ReadClass<hkbPoweredRagdollControlData>(xe, nameof(controlData));
            bones = xd.ReadClassPointer<hkbBoneIndexArray>(this, xe, nameof(bones));
            worldFromModelModeData = xd.ReadClass<hkbWorldFromModelModeData>(xe, nameof(worldFromModelModeData));
            boneWeights = xd.ReadClassPointer<hkbBoneWeightArray>(this, xe, nameof(boneWeights));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbPoweredRagdollControlData>(xe, nameof(controlData), controlData);
            xs.WriteClassPointer(xe, nameof(bones), bones);
            xs.WriteClass<hkbWorldFromModelModeData>(xe, nameof(worldFromModelModeData), worldFromModelModeData);
            xs.WriteClassPointer(xe, nameof(boneWeights), boneWeights);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbPoweredRagdollControlsModifier);
        }

        public bool Equals(hkbPoweredRagdollControlsModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((controlData is null && other.controlData is null) || (controlData is not null && other.controlData is not null && controlData.Equals((IHavokObject)other.controlData))) &&
                   ((bones is null && other.bones is null) || (bones is not null && other.bones is not null && bones.Equals((IHavokObject)other.bones))) &&
                   ((worldFromModelModeData is null && other.worldFromModelModeData is null) || (worldFromModelModeData is not null && other.worldFromModelModeData is not null && worldFromModelModeData.Equals((IHavokObject)other.worldFromModelModeData))) &&
                   ((boneWeights is null && other.boneWeights is null) || (boneWeights is not null && other.boneWeights is not null && boneWeights.Equals((IHavokObject)other.boneWeights))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(controlData);
            hashcode.Add(bones);
            hashcode.Add(worldFromModelModeData);
            hashcode.Add(boneWeights);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

