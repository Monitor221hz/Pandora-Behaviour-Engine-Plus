using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // BSRagdollContactListenerModifier Signatire: 0x8003d8ce size: 136 flags: FLAGS_NONE

    // contactEvent class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // bones class: hkbBoneIndexArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // throwEvent class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // ragdollRigidBodies class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSRagdollContactListenerModifier : hkbModifier, IEquatable<BSRagdollContactListenerModifier?>
    {
        public hkbEventProperty contactEvent { set; get; } = new();
        public hkbBoneIndexArray? bones { set; get; }
        private bool throwEvent { set; get; }
        public IList<object> ragdollRigidBodies { set; get; } = Array.Empty<object>();

        public override uint Signature { set; get; } = 0x8003d8ce;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            contactEvent.Read(des, br);
            bones = des.ReadClassPointer<hkbBoneIndexArray>(br);
            throwEvent = br.ReadBoolean();
            br.Position += 7;
            des.ReadEmptyArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            contactEvent.Write(s, bw);
            s.WriteClassPointer(bw, bones);
            bw.WriteBoolean(throwEvent);
            bw.Position += 7;
            s.WriteVoidArray(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            contactEvent = xd.ReadClass<hkbEventProperty>(xe, nameof(contactEvent));
            bones = xd.ReadClassPointer<hkbBoneIndexArray>(this, xe, nameof(bones));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbEventProperty>(xe, nameof(contactEvent), contactEvent);
            xs.WriteClassPointer(xe, nameof(bones), bones);
            xs.WriteSerializeIgnored(xe, nameof(throwEvent));
            xs.WriteSerializeIgnored(xe, nameof(ragdollRigidBodies));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSRagdollContactListenerModifier);
        }

        public bool Equals(BSRagdollContactListenerModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((contactEvent is null && other.contactEvent is null) || (contactEvent is not null && other.contactEvent is not null && contactEvent.Equals((IHavokObject)other.contactEvent))) &&
                   ((bones is null && other.bones is null) || (bones is not null && other.bones is not null && bones.Equals((IHavokObject)other.bones))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(contactEvent);
            hashcode.Add(bones);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

