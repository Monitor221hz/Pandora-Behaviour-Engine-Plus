using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCompressedMeshShapeConvexPiece Signatire: 0x385bb842 size: 80 flags: FLAGS_NONE

    // offset class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // vertices class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // faceVertices class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // faceOffsets class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // reference class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // transformIndex class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 66 flags: FLAGS_NONE enum: 
    public partial class hkpCompressedMeshShapeConvexPiece : IHavokObject, IEquatable<hkpCompressedMeshShapeConvexPiece?>
    {
        public Vector4 offset { set; get; }
        public IList<ushort> vertices { set; get; } = Array.Empty<ushort>();
        public IList<byte> faceVertices { set; get; } = Array.Empty<byte>();
        public IList<ushort> faceOffsets { set; get; } = Array.Empty<ushort>();
        public ushort reference { set; get; }
        public ushort transformIndex { set; get; }

        public virtual uint Signature { set; get; } = 0x385bb842;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            offset = br.ReadVector4();
            vertices = des.ReadUInt16Array(br);
            faceVertices = des.ReadByteArray(br);
            faceOffsets = des.ReadUInt16Array(br);
            reference = br.ReadUInt16();
            transformIndex = br.ReadUInt16();
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(offset);
            s.WriteUInt16Array(bw, vertices);
            s.WriteByteArray(bw, faceVertices);
            s.WriteUInt16Array(bw, faceOffsets);
            bw.WriteUInt16(reference);
            bw.WriteUInt16(transformIndex);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            offset = xd.ReadVector4(xe, nameof(offset));
            vertices = xd.ReadUInt16Array(xe, nameof(vertices));
            faceVertices = xd.ReadByteArray(xe, nameof(faceVertices));
            faceOffsets = xd.ReadUInt16Array(xe, nameof(faceOffsets));
            reference = xd.ReadUInt16(xe, nameof(reference));
            transformIndex = xd.ReadUInt16(xe, nameof(transformIndex));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(offset), offset);
            xs.WriteNumberArray(xe, nameof(vertices), vertices);
            xs.WriteNumberArray(xe, nameof(faceVertices), faceVertices);
            xs.WriteNumberArray(xe, nameof(faceOffsets), faceOffsets);
            xs.WriteNumber(xe, nameof(reference), reference);
            xs.WriteNumber(xe, nameof(transformIndex), transformIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCompressedMeshShapeConvexPiece);
        }

        public bool Equals(hkpCompressedMeshShapeConvexPiece? other)
        {
            return other is not null &&
                   offset.Equals(other.offset) &&
                   vertices.SequenceEqual(other.vertices) &&
                   faceVertices.SequenceEqual(other.faceVertices) &&
                   faceOffsets.SequenceEqual(other.faceOffsets) &&
                   reference.Equals(other.reference) &&
                   transformIndex.Equals(other.transformIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(offset);
            hashcode.Add(vertices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(faceVertices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(faceOffsets.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(reference);
            hashcode.Add(transformIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

