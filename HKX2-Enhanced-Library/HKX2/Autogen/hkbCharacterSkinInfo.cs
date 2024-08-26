using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterSkinInfo Signatire: 0x180d900d size: 56 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // deformableSkins class:  Type.TYPE_ARRAY Type.TYPE_UINT64 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // rigidSkins class:  Type.TYPE_ARRAY Type.TYPE_UINT64 arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    public partial class hkbCharacterSkinInfo : hkReferencedObject, IEquatable<hkbCharacterSkinInfo?>
    {
        public ulong characterId { set; get; }
        public IList<ulong> deformableSkins { set; get; } = Array.Empty<ulong>();
        public IList<ulong> rigidSkins { set; get; } = Array.Empty<ulong>();

        public override uint Signature { set; get; } = 0x180d900d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            deformableSkins = des.ReadUInt64Array(br);
            rigidSkins = des.ReadUInt64Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            s.WriteUInt64Array(bw, deformableSkins);
            s.WriteUInt64Array(bw, rigidSkins);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            deformableSkins = xd.ReadUInt64Array(xe, nameof(deformableSkins));
            rigidSkins = xd.ReadUInt64Array(xe, nameof(rigidSkins));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteNumberArray(xe, nameof(deformableSkins), deformableSkins);
            xs.WriteNumberArray(xe, nameof(rigidSkins), rigidSkins);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterSkinInfo);
        }

        public bool Equals(hkbCharacterSkinInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   deformableSkins.SequenceEqual(other.deformableSkins) &&
                   rigidSkins.SequenceEqual(other.rigidSkins) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(deformableSkins.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(rigidSkins.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

