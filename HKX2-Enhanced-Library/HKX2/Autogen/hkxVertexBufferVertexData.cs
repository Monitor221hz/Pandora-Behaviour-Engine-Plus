using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxVertexBufferVertexData Signatire: 0xd72b6fd0 size: 104 flags: FLAGS_NONE

    // vectorData class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // floatData class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // uint32Data class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // uint16Data class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // uint8Data class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // numVerts class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // vectorStride class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // floatStride class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // uint32Stride class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // uint16Stride class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // uint8Stride class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    public partial class hkxVertexBufferVertexData : IHavokObject, IEquatable<hkxVertexBufferVertexData?>
    {
        public IList<Vector4> vectorData { set; get; } = Array.Empty<Vector4>();
        public IList<float> floatData { set; get; } = Array.Empty<float>();
        public IList<uint> uint32Data { set; get; } = Array.Empty<uint>();
        public IList<ushort> uint16Data { set; get; } = Array.Empty<ushort>();
        public IList<byte> uint8Data { set; get; } = Array.Empty<byte>();
        public uint numVerts { set; get; }
        public uint vectorStride { set; get; }
        public uint floatStride { set; get; }
        public uint uint32Stride { set; get; }
        public uint uint16Stride { set; get; }
        public uint uint8Stride { set; get; }

        public virtual uint Signature { set; get; } = 0xd72b6fd0;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            vectorData = des.ReadVector4Array(br);
            floatData = des.ReadSingleArray(br);
            uint32Data = des.ReadUInt32Array(br);
            uint16Data = des.ReadUInt16Array(br);
            uint8Data = des.ReadByteArray(br);
            numVerts = br.ReadUInt32();
            vectorStride = br.ReadUInt32();
            floatStride = br.ReadUInt32();
            uint32Stride = br.ReadUInt32();
            uint16Stride = br.ReadUInt32();
            uint8Stride = br.ReadUInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVector4Array(bw, vectorData);
            s.WriteSingleArray(bw, floatData);
            s.WriteUInt32Array(bw, uint32Data);
            s.WriteUInt16Array(bw, uint16Data);
            s.WriteByteArray(bw, uint8Data);
            bw.WriteUInt32(numVerts);
            bw.WriteUInt32(vectorStride);
            bw.WriteUInt32(floatStride);
            bw.WriteUInt32(uint32Stride);
            bw.WriteUInt32(uint16Stride);
            bw.WriteUInt32(uint8Stride);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            vectorData = xd.ReadVector4Array(xe, nameof(vectorData));
            floatData = xd.ReadSingleArray(xe, nameof(floatData));
            uint32Data = xd.ReadUInt32Array(xe, nameof(uint32Data));
            uint16Data = xd.ReadUInt16Array(xe, nameof(uint16Data));
            uint8Data = xd.ReadByteArray(xe, nameof(uint8Data));
            numVerts = xd.ReadUInt32(xe, nameof(numVerts));
            vectorStride = xd.ReadUInt32(xe, nameof(vectorStride));
            floatStride = xd.ReadUInt32(xe, nameof(floatStride));
            uint32Stride = xd.ReadUInt32(xe, nameof(uint32Stride));
            uint16Stride = xd.ReadUInt32(xe, nameof(uint16Stride));
            uint8Stride = xd.ReadUInt32(xe, nameof(uint8Stride));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4Array(xe, nameof(vectorData), vectorData);
            xs.WriteFloatArray(xe, nameof(floatData), floatData);
            xs.WriteNumberArray(xe, nameof(uint32Data), uint32Data);
            xs.WriteNumberArray(xe, nameof(uint16Data), uint16Data);
            xs.WriteNumberArray(xe, nameof(uint8Data), uint8Data);
            xs.WriteNumber(xe, nameof(numVerts), numVerts);
            xs.WriteNumber(xe, nameof(vectorStride), vectorStride);
            xs.WriteNumber(xe, nameof(floatStride), floatStride);
            xs.WriteNumber(xe, nameof(uint32Stride), uint32Stride);
            xs.WriteNumber(xe, nameof(uint16Stride), uint16Stride);
            xs.WriteNumber(xe, nameof(uint8Stride), uint8Stride);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxVertexBufferVertexData);
        }

        public bool Equals(hkxVertexBufferVertexData? other)
        {
            return other is not null &&
                   vectorData.SequenceEqual(other.vectorData) &&
                   floatData.SequenceEqual(other.floatData) &&
                   uint32Data.SequenceEqual(other.uint32Data) &&
                   uint16Data.SequenceEqual(other.uint16Data) &&
                   uint8Data.SequenceEqual(other.uint8Data) &&
                   numVerts.Equals(other.numVerts) &&
                   vectorStride.Equals(other.vectorStride) &&
                   floatStride.Equals(other.floatStride) &&
                   uint32Stride.Equals(other.uint32Stride) &&
                   uint16Stride.Equals(other.uint16Stride) &&
                   uint8Stride.Equals(other.uint8Stride) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(vectorData.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(floatData.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(uint32Data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(uint16Data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(uint8Data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(numVerts);
            hashcode.Add(vectorStride);
            hashcode.Add(floatStride);
            hashcode.Add(uint32Stride);
            hashcode.Add(uint16Stride);
            hashcode.Add(uint8Stride);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

