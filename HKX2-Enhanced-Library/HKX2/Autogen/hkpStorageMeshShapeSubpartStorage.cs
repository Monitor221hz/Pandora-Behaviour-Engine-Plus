using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpStorageMeshShapeSubpartStorage Signatire: 0xbf27438 size: 112 flags: FLAGS_NONE

    // vertices class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // indices16 class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // indices32 class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // materialIndices class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // materials class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // materialIndices16 class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    public partial class hkpStorageMeshShapeSubpartStorage : hkReferencedObject, IEquatable<hkpStorageMeshShapeSubpartStorage?>
    {
        public IList<float> vertices { set; get; } = Array.Empty<float>();
        public IList<ushort> indices16 { set; get; } = Array.Empty<ushort>();
        public IList<uint> indices32 { set; get; } = Array.Empty<uint>();
        public IList<byte> materialIndices { set; get; } = Array.Empty<byte>();
        public IList<uint> materials { set; get; } = Array.Empty<uint>();
        public IList<ushort> materialIndices16 { set; get; } = Array.Empty<ushort>();

        public override uint Signature { set; get; } = 0xbf27438;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            vertices = des.ReadSingleArray(br);
            indices16 = des.ReadUInt16Array(br);
            indices32 = des.ReadUInt32Array(br);
            materialIndices = des.ReadByteArray(br);
            materials = des.ReadUInt32Array(br);
            materialIndices16 = des.ReadUInt16Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteSingleArray(bw, vertices);
            s.WriteUInt16Array(bw, indices16);
            s.WriteUInt32Array(bw, indices32);
            s.WriteByteArray(bw, materialIndices);
            s.WriteUInt32Array(bw, materials);
            s.WriteUInt16Array(bw, materialIndices16);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            vertices = xd.ReadSingleArray(xe, nameof(vertices));
            indices16 = xd.ReadUInt16Array(xe, nameof(indices16));
            indices32 = xd.ReadUInt32Array(xe, nameof(indices32));
            materialIndices = xd.ReadByteArray(xe, nameof(materialIndices));
            materials = xd.ReadUInt32Array(xe, nameof(materials));
            materialIndices16 = xd.ReadUInt16Array(xe, nameof(materialIndices16));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloatArray(xe, nameof(vertices), vertices);
            xs.WriteNumberArray(xe, nameof(indices16), indices16);
            xs.WriteNumberArray(xe, nameof(indices32), indices32);
            xs.WriteNumberArray(xe, nameof(materialIndices), materialIndices);
            xs.WriteNumberArray(xe, nameof(materials), materials);
            xs.WriteNumberArray(xe, nameof(materialIndices16), materialIndices16);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpStorageMeshShapeSubpartStorage);
        }

        public bool Equals(hkpStorageMeshShapeSubpartStorage? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   vertices.SequenceEqual(other.vertices) &&
                   indices16.SequenceEqual(other.indices16) &&
                   indices32.SequenceEqual(other.indices32) &&
                   materialIndices.SequenceEqual(other.materialIndices) &&
                   materials.SequenceEqual(other.materials) &&
                   materialIndices16.SequenceEqual(other.materialIndices16) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(vertices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(indices16.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(indices32.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(materialIndices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(materials.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(materialIndices16.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

