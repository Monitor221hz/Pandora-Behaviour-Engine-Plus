using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbRaiseEventCommand Signatire: 0xa0a7bf9c size: 32 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // global class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // externalId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    public partial class hkbRaiseEventCommand : hkReferencedObject, IEquatable<hkbRaiseEventCommand?>
    {
        public ulong characterId { set; get; }
        public bool global { set; get; }
        public int externalId { set; get; }

        public override uint Signature { set; get; } = 0xa0a7bf9c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            global = br.ReadBoolean();
            br.Position += 3;
            externalId = br.ReadInt32();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            bw.WriteBoolean(global);
            bw.Position += 3;
            bw.WriteInt32(externalId);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            global = xd.ReadBoolean(xe, nameof(global));
            externalId = xd.ReadInt32(xe, nameof(externalId));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteBoolean(xe, nameof(global), global);
            xs.WriteNumber(xe, nameof(externalId), externalId);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbRaiseEventCommand);
        }

        public bool Equals(hkbRaiseEventCommand? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   global.Equals(other.global) &&
                   externalId.Equals(other.externalId) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(global);
            hashcode.Add(externalId);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

