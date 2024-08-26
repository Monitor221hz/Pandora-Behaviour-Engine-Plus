using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterInfo Signatire: 0xd9709ff2 size: 32 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // @eventclass:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: Event
    // padding class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    public partial class hkbCharacterInfo : hkReferencedObject, IEquatable<hkbCharacterInfo?>
    {
        public ulong characterId { set; get; }
        public byte @event{ set; get; }
        public int padding { set; get; }

        public override uint Signature { set; get; } = 0xd9709ff2;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            @event= br.ReadByte();
            br.Position += 3;
            padding = br.ReadInt32();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            bw.WriteByte(@event);
            bw.Position += 3;
            bw.WriteInt32(padding);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            @event= xd.ReadFlag<Event, byte>(xe, nameof(@event));
            padding = xd.ReadInt32(xe, nameof(padding));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteEnum<Event, byte>(xe, nameof(@event), @event);
            xs.WriteNumber(xe, nameof(padding), padding);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterInfo);
        }

        public bool Equals(hkbCharacterInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   @event.Equals(other.@event) &&
                   padding.Equals(other.padding) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(@event);
            hashcode.Add(padding);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

