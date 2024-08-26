using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSetLocalTimeOfClipGeneratorCommand Signatire: 0xfab12b45 size: 32 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // localTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // nodeId class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    public partial class hkbSetLocalTimeOfClipGeneratorCommand : hkReferencedObject, IEquatable<hkbSetLocalTimeOfClipGeneratorCommand?>
    {
        public ulong characterId { set; get; }
        public float localTime { set; get; }
        public short nodeId { set; get; }

        public override uint Signature { set; get; } = 0xfab12b45;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            localTime = br.ReadSingle();
            nodeId = br.ReadInt16();
            br.Position += 2;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            bw.WriteSingle(localTime);
            bw.WriteInt16(nodeId);
            bw.Position += 2;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            localTime = xd.ReadSingle(xe, nameof(localTime));
            nodeId = xd.ReadInt16(xe, nameof(nodeId));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteFloat(xe, nameof(localTime), localTime);
            xs.WriteNumber(xe, nameof(nodeId), nodeId);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSetLocalTimeOfClipGeneratorCommand);
        }

        public bool Equals(hkbSetLocalTimeOfClipGeneratorCommand? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   localTime.Equals(other.localTime) &&
                   nodeId.Equals(other.nodeId) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(localTime);
            hashcode.Add(nodeId);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

