using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbHandle Signatire: 0xd8b6401c size: 48 flags: FLAGS_NONE

    // frame class: hkLocalFrame Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // rigidBody class: hkpRigidBody Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // character class: hkbCharacter Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // animationBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    public partial class hkbHandle : hkReferencedObject, IEquatable<hkbHandle?>
    {
        public hkLocalFrame? frame { set; get; }
        public hkpRigidBody? rigidBody { set; get; }
        public hkbCharacter? character { set; get; }
        public short animationBoneIndex { set; get; }

        public override uint Signature { set; get; } = 0xd8b6401c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            frame = des.ReadClassPointer<hkLocalFrame>(br);
            rigidBody = des.ReadClassPointer<hkpRigidBody>(br);
            character = des.ReadClassPointer<hkbCharacter>(br);
            animationBoneIndex = br.ReadInt16();
            br.Position += 6;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, frame);
            s.WriteClassPointer(bw, rigidBody);
            s.WriteClassPointer(bw, character);
            bw.WriteInt16(animationBoneIndex);
            bw.Position += 6;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            frame = xd.ReadClassPointer<hkLocalFrame>(this, xe, nameof(frame));
            rigidBody = xd.ReadClassPointer<hkpRigidBody>(this, xe, nameof(rigidBody));
            character = xd.ReadClassPointer<hkbCharacter>(this, xe, nameof(character));
            animationBoneIndex = xd.ReadInt16(xe, nameof(animationBoneIndex));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(frame), frame);
            xs.WriteClassPointer(xe, nameof(rigidBody), rigidBody);
            xs.WriteClassPointer(xe, nameof(character), character);
            xs.WriteNumber(xe, nameof(animationBoneIndex), animationBoneIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbHandle);
        }

        public bool Equals(hkbHandle? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((frame is null && other.frame is null) || (frame is not null && other.frame is not null && frame.Equals((IHavokObject)other.frame))) &&
                   ((rigidBody is null && other.rigidBody is null) || (rigidBody is not null && other.rigidBody is not null && rigidBody.Equals((IHavokObject)other.rigidBody))) &&
                   ((character is null && other.character is null) || (character is not null && other.character is not null && character.Equals((IHavokObject)other.character))) &&
                   animationBoneIndex.Equals(other.animationBoneIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(frame);
            hashcode.Add(rigidBody);
            hashcode.Add(character);
            hashcode.Add(animationBoneIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

