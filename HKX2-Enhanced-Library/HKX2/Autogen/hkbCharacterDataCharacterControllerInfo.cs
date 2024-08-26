using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterDataCharacterControllerInfo Signatire: 0xa0f415bf size: 24 flags: FLAGS_NONE

    // capsuleHeight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // capsuleRadius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // collisionFilterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // characterControllerCinfo class: hkpCharacterControllerCinfo Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbCharacterDataCharacterControllerInfo : IHavokObject, IEquatable<hkbCharacterDataCharacterControllerInfo?>
    {
        public float capsuleHeight { set; get; }
        public float capsuleRadius { set; get; }
        public uint collisionFilterInfo { set; get; }
        public hkpCharacterControllerCinfo? characterControllerCinfo { set; get; }

        public virtual uint Signature { set; get; } = 0xa0f415bf;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            capsuleHeight = br.ReadSingle();
            capsuleRadius = br.ReadSingle();
            collisionFilterInfo = br.ReadUInt32();
            br.Position += 4;
            characterControllerCinfo = des.ReadClassPointer<hkpCharacterControllerCinfo>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(capsuleHeight);
            bw.WriteSingle(capsuleRadius);
            bw.WriteUInt32(collisionFilterInfo);
            bw.Position += 4;
            s.WriteClassPointer(bw, characterControllerCinfo);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            capsuleHeight = xd.ReadSingle(xe, nameof(capsuleHeight));
            capsuleRadius = xd.ReadSingle(xe, nameof(capsuleRadius));
            collisionFilterInfo = xd.ReadUInt32(xe, nameof(collisionFilterInfo));
            characterControllerCinfo = xd.ReadClassPointer<hkpCharacterControllerCinfo>(this, xe, nameof(characterControllerCinfo));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(capsuleHeight), capsuleHeight);
            xs.WriteFloat(xe, nameof(capsuleRadius), capsuleRadius);
            xs.WriteNumber(xe, nameof(collisionFilterInfo), collisionFilterInfo);
            xs.WriteClassPointer(xe, nameof(characterControllerCinfo), characterControllerCinfo);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterDataCharacterControllerInfo);
        }

        public bool Equals(hkbCharacterDataCharacterControllerInfo? other)
        {
            return other is not null &&
                   capsuleHeight.Equals(other.capsuleHeight) &&
                   capsuleRadius.Equals(other.capsuleRadius) &&
                   collisionFilterInfo.Equals(other.collisionFilterInfo) &&
                   ((characterControllerCinfo is null && other.characterControllerCinfo is null) || (characterControllerCinfo is not null && other.characterControllerCinfo is not null && characterControllerCinfo.Equals((IHavokObject)other.characterControllerCinfo))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(capsuleHeight);
            hashcode.Add(capsuleRadius);
            hashcode.Add(collisionFilterInfo);
            hashcode.Add(characterControllerCinfo);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

