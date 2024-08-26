using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEventRaisedInfo Signatire: 0xc02da3 size: 48 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // eventName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // raisedBySdk class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // senderId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // padding class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    public partial class hkbEventRaisedInfo : hkReferencedObject, IEquatable<hkbEventRaisedInfo?>
    {
        public ulong characterId { set; get; }
        public string eventName { set; get; } = "";
        public bool raisedBySdk { set; get; }
        public int senderId { set; get; }
        public int padding { set; get; }

        public override uint Signature { set; get; } = 0xc02da3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            eventName = des.ReadStringPointer(br);
            raisedBySdk = br.ReadBoolean();
            br.Position += 3;
            senderId = br.ReadInt32();
            padding = br.ReadInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            s.WriteStringPointer(bw, eventName);
            bw.WriteBoolean(raisedBySdk);
            bw.Position += 3;
            bw.WriteInt32(senderId);
            bw.WriteInt32(padding);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            eventName = xd.ReadString(xe, nameof(eventName));
            raisedBySdk = xd.ReadBoolean(xe, nameof(raisedBySdk));
            senderId = xd.ReadInt32(xe, nameof(senderId));
            padding = xd.ReadInt32(xe, nameof(padding));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteString(xe, nameof(eventName), eventName);
            xs.WriteBoolean(xe, nameof(raisedBySdk), raisedBySdk);
            xs.WriteNumber(xe, nameof(senderId), senderId);
            xs.WriteNumber(xe, nameof(padding), padding);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEventRaisedInfo);
        }

        public bool Equals(hkbEventRaisedInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   (eventName is null && other.eventName is null || eventName == other.eventName || eventName is null && other.eventName == "" || eventName == "" && other.eventName is null) &&
                   raisedBySdk.Equals(other.raisedBySdk) &&
                   senderId.Equals(other.senderId) &&
                   padding.Equals(other.padding) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(eventName);
            hashcode.Add(raisedBySdk);
            hashcode.Add(senderId);
            hashcode.Add(padding);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

