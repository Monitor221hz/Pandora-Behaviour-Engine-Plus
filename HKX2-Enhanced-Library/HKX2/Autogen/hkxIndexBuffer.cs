using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxIndexBuffer Signatire: 0xc12c8197 size: 64 flags: FLAGS_NONE

    // indexType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: IndexType
    // indices16 class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // indices32 class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // vertexBaseOffset class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // length class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    public partial class hkxIndexBuffer : hkReferencedObject, IEquatable<hkxIndexBuffer?>
    {
        public sbyte indexType { set; get; }
        public IList<ushort> indices16 { set; get; } = Array.Empty<ushort>();
        public IList<uint> indices32 { set; get; } = Array.Empty<uint>();
        public uint vertexBaseOffset { set; get; }
        public uint length { set; get; }

        public override uint Signature { set; get; } = 0xc12c8197;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            indexType = br.ReadSByte();
            br.Position += 7;
            indices16 = des.ReadUInt16Array(br);
            indices32 = des.ReadUInt32Array(br);
            vertexBaseOffset = br.ReadUInt32();
            length = br.ReadUInt32();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSByte(indexType);
            bw.Position += 7;
            s.WriteUInt16Array(bw, indices16);
            s.WriteUInt32Array(bw, indices32);
            bw.WriteUInt32(vertexBaseOffset);
            bw.WriteUInt32(length);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            indexType = xd.ReadFlag<IndexType, sbyte>(xe, nameof(indexType));
            indices16 = xd.ReadUInt16Array(xe, nameof(indices16));
            indices32 = xd.ReadUInt32Array(xe, nameof(indices32));
            vertexBaseOffset = xd.ReadUInt32(xe, nameof(vertexBaseOffset));
            length = xd.ReadUInt32(xe, nameof(length));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<IndexType, sbyte>(xe, nameof(indexType), indexType);
            xs.WriteNumberArray(xe, nameof(indices16), indices16);
            xs.WriteNumberArray(xe, nameof(indices32), indices32);
            xs.WriteNumber(xe, nameof(vertexBaseOffset), vertexBaseOffset);
            xs.WriteNumber(xe, nameof(length), length);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxIndexBuffer);
        }

        public bool Equals(hkxIndexBuffer? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   indexType.Equals(other.indexType) &&
                   indices16.SequenceEqual(other.indices16) &&
                   indices32.SequenceEqual(other.indices32) &&
                   vertexBaseOffset.Equals(other.vertexBaseOffset) &&
                   length.Equals(other.length) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(indexType);
            hashcode.Add(indices16.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(indices32.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(vertexBaseOffset);
            hashcode.Add(length);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

