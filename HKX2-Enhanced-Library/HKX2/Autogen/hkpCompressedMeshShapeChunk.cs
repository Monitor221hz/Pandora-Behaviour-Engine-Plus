using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCompressedMeshShapeChunk Signatire: 0x5d0d67bd size: 96 flags: FLAGS_NONE

    // offset class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // vertices class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // indices class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // stripLengths class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // weldingInfo class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // materialInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // reference class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // transformIndex class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 86 flags: FLAGS_NONE enum: 
    public partial class hkpCompressedMeshShapeChunk : IHavokObject, IEquatable<hkpCompressedMeshShapeChunk?>
    {
        public Vector4 offset { set; get; }
        public IList<ushort> vertices { set; get; } = Array.Empty<ushort>();
        public IList<ushort> indices { set; get; } = Array.Empty<ushort>();
        public IList<ushort> stripLengths { set; get; } = Array.Empty<ushort>();
        public IList<ushort> weldingInfo { set; get; } = Array.Empty<ushort>();
        public uint materialInfo { set; get; }
        public ushort reference { set; get; }
        public ushort transformIndex { set; get; }

        public virtual uint Signature { set; get; } = 0x5d0d67bd;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            offset = br.ReadVector4();
            vertices = des.ReadUInt16Array(br);
            indices = des.ReadUInt16Array(br);
            stripLengths = des.ReadUInt16Array(br);
            weldingInfo = des.ReadUInt16Array(br);
            materialInfo = br.ReadUInt32();
            reference = br.ReadUInt16();
            transformIndex = br.ReadUInt16();
            br.Position += 8;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(offset);
            s.WriteUInt16Array(bw, vertices);
            s.WriteUInt16Array(bw, indices);
            s.WriteUInt16Array(bw, stripLengths);
            s.WriteUInt16Array(bw, weldingInfo);
            bw.WriteUInt32(materialInfo);
            bw.WriteUInt16(reference);
            bw.WriteUInt16(transformIndex);
            bw.Position += 8;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            offset = xd.ReadVector4(xe, nameof(offset));
            vertices = xd.ReadUInt16Array(xe, nameof(vertices));
            indices = xd.ReadUInt16Array(xe, nameof(indices));
            stripLengths = xd.ReadUInt16Array(xe, nameof(stripLengths));
            weldingInfo = xd.ReadUInt16Array(xe, nameof(weldingInfo));
            materialInfo = xd.ReadUInt32(xe, nameof(materialInfo));
            reference = xd.ReadUInt16(xe, nameof(reference));
            transformIndex = xd.ReadUInt16(xe, nameof(transformIndex));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(offset), offset);
            xs.WriteNumberArray(xe, nameof(vertices), vertices);
            xs.WriteNumberArray(xe, nameof(indices), indices);
            xs.WriteNumberArray(xe, nameof(stripLengths), stripLengths);
            xs.WriteNumberArray(xe, nameof(weldingInfo), weldingInfo);
            xs.WriteNumber(xe, nameof(materialInfo), materialInfo);
            xs.WriteNumber(xe, nameof(reference), reference);
            xs.WriteNumber(xe, nameof(transformIndex), transformIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCompressedMeshShapeChunk);
        }

        public bool Equals(hkpCompressedMeshShapeChunk? other)
        {
            return other is not null &&
                   offset.Equals(other.offset) &&
                   vertices.SequenceEqual(other.vertices) &&
                   indices.SequenceEqual(other.indices) &&
                   stripLengths.SequenceEqual(other.stripLengths) &&
                   weldingInfo.SequenceEqual(other.weldingInfo) &&
                   materialInfo.Equals(other.materialInfo) &&
                   reference.Equals(other.reference) &&
                   transformIndex.Equals(other.transformIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(offset);
            hashcode.Add(vertices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(indices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(stripLengths.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(weldingInfo.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(materialInfo);
            hashcode.Add(reference);
            hashcode.Add(transformIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

