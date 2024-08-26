using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbRigidBodyRagdollControlData Signatire: 0x1e0bc068 size: 64 flags: FLAGS_NONE

    // keyFrameHierarchyControlData class: hkaKeyFrameHierarchyUtilityControlData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: ALIGN_16|FLAGS_NONE enum: 
    // durationToBlend class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkbRigidBodyRagdollControlData : IHavokObject, IEquatable<hkbRigidBodyRagdollControlData?>
    {
        public hkaKeyFrameHierarchyUtilityControlData keyFrameHierarchyControlData { set; get; } = new();
        public float durationToBlend { set; get; }

        public virtual uint Signature { set; get; } = 0x1e0bc068;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            keyFrameHierarchyControlData.Read(des, br);
            durationToBlend = br.ReadSingle();
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            keyFrameHierarchyControlData.Write(s, bw);
            bw.WriteSingle(durationToBlend);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            keyFrameHierarchyControlData = xd.ReadClass<hkaKeyFrameHierarchyUtilityControlData>(xe, nameof(keyFrameHierarchyControlData));
            durationToBlend = xd.ReadSingle(xe, nameof(durationToBlend));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkaKeyFrameHierarchyUtilityControlData>(xe, nameof(keyFrameHierarchyControlData), keyFrameHierarchyControlData);
            xs.WriteFloat(xe, nameof(durationToBlend), durationToBlend);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbRigidBodyRagdollControlData);
        }

        public bool Equals(hkbRigidBodyRagdollControlData? other)
        {
            return other is not null &&
                   ((keyFrameHierarchyControlData is null && other.keyFrameHierarchyControlData is null) || (keyFrameHierarchyControlData is not null && other.keyFrameHierarchyControlData is not null && keyFrameHierarchyControlData.Equals((IHavokObject)other.keyFrameHierarchyControlData))) &&
                   durationToBlend.Equals(other.durationToBlend) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(keyFrameHierarchyControlData);
            hashcode.Add(durationToBlend);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

