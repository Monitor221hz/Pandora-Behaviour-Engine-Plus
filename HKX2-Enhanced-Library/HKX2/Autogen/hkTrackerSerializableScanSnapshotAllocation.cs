using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkTrackerSerializableScanSnapshotAllocation Signatire: 0x9ab3a6ac size: 24 flags: FLAGS_NONE

    // start class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // size class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // traceId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkTrackerSerializableScanSnapshotAllocation : IHavokObject, IEquatable<hkTrackerSerializableScanSnapshotAllocation?>
    {
        public ulong start { set; get; }
        public ulong size { set; get; }
        public int traceId { set; get; }

        public virtual uint Signature { set; get; } = 0x9ab3a6ac;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            start = br.ReadUInt64();
            size = br.ReadUInt64();
            traceId = br.ReadInt32();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt64(start);
            bw.WriteUInt64(size);
            bw.WriteInt32(traceId);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            start = xd.ReadUInt64(xe, nameof(start));
            size = xd.ReadUInt64(xe, nameof(size));
            traceId = xd.ReadInt32(xe, nameof(traceId));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(start), start);
            xs.WriteNumber(xe, nameof(size), size);
            xs.WriteNumber(xe, nameof(traceId), traceId);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkTrackerSerializableScanSnapshotAllocation);
        }

        public bool Equals(hkTrackerSerializableScanSnapshotAllocation? other)
        {
            return other is not null &&
                   start.Equals(other.start) &&
                   size.Equals(other.size) &&
                   traceId.Equals(other.traceId) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(start);
            hashcode.Add(size);
            hashcode.Add(traceId);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

