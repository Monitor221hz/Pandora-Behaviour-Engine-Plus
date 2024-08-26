using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkTrackerSerializableScanSnapshotBlock Signatire: 0xe7f23e6d size: 40 flags: FLAGS_NONE

    // typeIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // start class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // size class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // arraySize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // startReferenceIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // numReferences class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkTrackerSerializableScanSnapshotBlock : IHavokObject, IEquatable<hkTrackerSerializableScanSnapshotBlock?>
    {
        public int typeIndex { set; get; }
        public ulong start { set; get; }
        public ulong size { set; get; }
        public int arraySize { set; get; }
        public int startReferenceIndex { set; get; }
        public int numReferences { set; get; }

        public virtual uint Signature { set; get; } = 0xe7f23e6d;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            typeIndex = br.ReadInt32();
            br.Position += 4;
            start = br.ReadUInt64();
            size = br.ReadUInt64();
            arraySize = br.ReadInt32();
            startReferenceIndex = br.ReadInt32();
            numReferences = br.ReadInt32();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(typeIndex);
            bw.Position += 4;
            bw.WriteUInt64(start);
            bw.WriteUInt64(size);
            bw.WriteInt32(arraySize);
            bw.WriteInt32(startReferenceIndex);
            bw.WriteInt32(numReferences);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            typeIndex = xd.ReadInt32(xe, nameof(typeIndex));
            start = xd.ReadUInt64(xe, nameof(start));
            size = xd.ReadUInt64(xe, nameof(size));
            arraySize = xd.ReadInt32(xe, nameof(arraySize));
            startReferenceIndex = xd.ReadInt32(xe, nameof(startReferenceIndex));
            numReferences = xd.ReadInt32(xe, nameof(numReferences));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(typeIndex), typeIndex);
            xs.WriteNumber(xe, nameof(start), start);
            xs.WriteNumber(xe, nameof(size), size);
            xs.WriteNumber(xe, nameof(arraySize), arraySize);
            xs.WriteNumber(xe, nameof(startReferenceIndex), startReferenceIndex);
            xs.WriteNumber(xe, nameof(numReferences), numReferences);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkTrackerSerializableScanSnapshotBlock);
        }

        public bool Equals(hkTrackerSerializableScanSnapshotBlock? other)
        {
            return other is not null &&
                   typeIndex.Equals(other.typeIndex) &&
                   start.Equals(other.start) &&
                   size.Equals(other.size) &&
                   arraySize.Equals(other.arraySize) &&
                   startReferenceIndex.Equals(other.startReferenceIndex) &&
                   numReferences.Equals(other.numReferences) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(typeIndex);
            hashcode.Add(start);
            hashcode.Add(size);
            hashcode.Add(arraySize);
            hashcode.Add(startReferenceIndex);
            hashcode.Add(numReferences);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

