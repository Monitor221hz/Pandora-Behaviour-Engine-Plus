using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkTrackerSerializableScanSnapshot Signatire: 0x875af1d9 size: 128 flags: FLAGS_NONE

    // allocations class: hkTrackerSerializableScanSnapshotAllocation Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // blocks class: hkTrackerSerializableScanSnapshotBlock Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // refs class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // typeNames class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // traceText class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // traceAddrs class:  Type.TYPE_ARRAY Type.TYPE_UINT64 arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // traceParents class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkTrackerSerializableScanSnapshot : hkReferencedObject, IEquatable<hkTrackerSerializableScanSnapshot?>
    {
        public IList<hkTrackerSerializableScanSnapshotAllocation> allocations { set; get; } = Array.Empty<hkTrackerSerializableScanSnapshotAllocation>();
        public IList<hkTrackerSerializableScanSnapshotBlock> blocks { set; get; } = Array.Empty<hkTrackerSerializableScanSnapshotBlock>();
        public IList<int> refs { set; get; } = Array.Empty<int>();
        public IList<byte> typeNames { set; get; } = Array.Empty<byte>();
        public IList<byte> traceText { set; get; } = Array.Empty<byte>();
        public IList<ulong> traceAddrs { set; get; } = Array.Empty<ulong>();
        public IList<int> traceParents { set; get; } = Array.Empty<int>();

        public override uint Signature { set; get; } = 0x875af1d9;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            allocations = des.ReadClassArray<hkTrackerSerializableScanSnapshotAllocation>(br);
            blocks = des.ReadClassArray<hkTrackerSerializableScanSnapshotBlock>(br);
            refs = des.ReadInt32Array(br);
            typeNames = des.ReadByteArray(br);
            traceText = des.ReadByteArray(br);
            traceAddrs = des.ReadUInt64Array(br);
            traceParents = des.ReadInt32Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, allocations);
            s.WriteClassArray(bw, blocks);
            s.WriteInt32Array(bw, refs);
            s.WriteByteArray(bw, typeNames);
            s.WriteByteArray(bw, traceText);
            s.WriteUInt64Array(bw, traceAddrs);
            s.WriteInt32Array(bw, traceParents);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            allocations = xd.ReadClassArray<hkTrackerSerializableScanSnapshotAllocation>(xe, nameof(allocations));
            blocks = xd.ReadClassArray<hkTrackerSerializableScanSnapshotBlock>(xe, nameof(blocks));
            refs = xd.ReadInt32Array(xe, nameof(refs));
            typeNames = xd.ReadByteArray(xe, nameof(typeNames));
            traceText = xd.ReadByteArray(xe, nameof(traceText));
            traceAddrs = xd.ReadUInt64Array(xe, nameof(traceAddrs));
            traceParents = xd.ReadInt32Array(xe, nameof(traceParents));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(allocations), allocations);
            xs.WriteClassArray(xe, nameof(blocks), blocks);
            xs.WriteNumberArray(xe, nameof(refs), refs);
            xs.WriteNumberArray(xe, nameof(typeNames), typeNames);
            xs.WriteNumberArray(xe, nameof(traceText), traceText);
            xs.WriteNumberArray(xe, nameof(traceAddrs), traceAddrs);
            xs.WriteNumberArray(xe, nameof(traceParents), traceParents);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkTrackerSerializableScanSnapshot);
        }

        public bool Equals(hkTrackerSerializableScanSnapshot? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   allocations.SequenceEqual(other.allocations) &&
                   blocks.SequenceEqual(other.blocks) &&
                   refs.SequenceEqual(other.refs) &&
                   typeNames.SequenceEqual(other.typeNames) &&
                   traceText.SequenceEqual(other.traceText) &&
                   traceAddrs.SequenceEqual(other.traceAddrs) &&
                   traceParents.SequenceEqual(other.traceParents) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(allocations.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(blocks.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(refs.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(typeNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(traceText.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(traceAddrs.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(traceParents.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

