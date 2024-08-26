using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbRigidBodyRagdollControlsModifier Signatire: 0xaa87d1eb size: 160 flags: FLAGS_NONE

    // controlData class: hkbRigidBodyRagdollControlData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // bones class: hkbBoneIndexArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    public partial class hkbRigidBodyRagdollControlsModifier : hkbModifier, IEquatable<hkbRigidBodyRagdollControlsModifier?>
    {
        public hkbRigidBodyRagdollControlData controlData { set; get; } = new();
        public hkbBoneIndexArray? bones { set; get; }

        public override uint Signature { set; get; } = 0xaa87d1eb;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            controlData.Read(des, br);
            bones = des.ReadClassPointer<hkbBoneIndexArray>(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            controlData.Write(s, bw);
            s.WriteClassPointer(bw, bones);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            controlData = xd.ReadClass<hkbRigidBodyRagdollControlData>(xe, nameof(controlData));
            bones = xd.ReadClassPointer<hkbBoneIndexArray>(this, xe, nameof(bones));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbRigidBodyRagdollControlData>(xe, nameof(controlData), controlData);
            xs.WriteClassPointer(xe, nameof(bones), bones);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbRigidBodyRagdollControlsModifier);
        }

        public bool Equals(hkbRigidBodyRagdollControlsModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((controlData is null && other.controlData is null) || (controlData is not null && other.controlData is not null && controlData.Equals((IHavokObject)other.controlData))) &&
                   ((bones is null && other.bones is null) || (bones is not null && other.bones is not null && bones.Equals((IHavokObject)other.bones))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(controlData);
            hashcode.Add(bones);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

