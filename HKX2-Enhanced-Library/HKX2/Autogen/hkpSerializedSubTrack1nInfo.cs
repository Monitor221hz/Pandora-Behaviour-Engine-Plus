using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSerializedSubTrack1nInfo Signatire: 0x10155a size: 40 flags: FLAGS_NONE

    // sectorIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // offsetInSector class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    public partial class hkpSerializedSubTrack1nInfo : hkpSerializedTrack1nInfo, IEquatable<hkpSerializedSubTrack1nInfo?>
    {
        public int sectorIndex { set; get; }
        public int offsetInSector { set; get; }

        public override uint Signature { set; get; } = 0x10155a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            sectorIndex = br.ReadInt32();
            offsetInSector = br.ReadInt32();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(sectorIndex);
            bw.WriteInt32(offsetInSector);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            sectorIndex = xd.ReadInt32(xe, nameof(sectorIndex));
            offsetInSector = xd.ReadInt32(xe, nameof(offsetInSector));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(sectorIndex), sectorIndex);
            xs.WriteNumber(xe, nameof(offsetInSector), offsetInSector);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSerializedSubTrack1nInfo);
        }

        public bool Equals(hkpSerializedSubTrack1nInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   sectorIndex.Equals(other.sectorIndex) &&
                   offsetInSector.Equals(other.offsetInSector) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(sectorIndex);
            hashcode.Add(offsetInSector);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

