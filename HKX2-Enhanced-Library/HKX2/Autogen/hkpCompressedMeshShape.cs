using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCompressedMeshShape Signatire: 0xe3d1dba size: 304 flags: FLAGS_NONE

    // bitsPerIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // bitsPerWIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 52 flags: FLAGS_NONE enum: 
    // wIndexMask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // indexMask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // radius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // weldingType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 68 flags: FLAGS_NONE enum: WeldingType
    // materialType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 69 flags: FLAGS_NONE enum: MaterialType
    // materials class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // materials16 class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // materials8 class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // transforms class:  Type.TYPE_ARRAY Type.TYPE_QSTRANSFORM arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // bigVertices class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // bigTriangles class: hkpCompressedMeshShapeBigTriangle Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // chunks class: hkpCompressedMeshShapeChunk Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // convexPieces class: hkpCompressedMeshShapeConvexPiece Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // error class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 200 flags: FLAGS_NONE enum: 
    // bounds class: hkAabb Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 208 flags: FLAGS_NONE enum: 
    // defaultCollisionFilterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 240 flags: FLAGS_NONE enum: 
    // meshMaterials class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 248 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // materialStriding class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 256 flags: FLAGS_NONE enum: 
    // numMaterials class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 258 flags: FLAGS_NONE enum: 
    // namedMaterials class: hkpNamedMeshMaterial Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 264 flags: FLAGS_NONE enum: 
    // scaling class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 288 flags: FLAGS_NONE enum: 
    public partial class hkpCompressedMeshShape : hkpShapeCollection, IEquatable<hkpCompressedMeshShape?>
    {
        public int bitsPerIndex { set; get; }
        public int bitsPerWIndex { set; get; }
        public int wIndexMask { set; get; }
        public int indexMask { set; get; }
        public float radius { set; get; }
        public byte weldingType { set; get; }
        public byte materialType { set; get; }
        public IList<uint> materials { set; get; } = Array.Empty<uint>();
        public IList<ushort> materials16 { set; get; } = Array.Empty<ushort>();
        public IList<byte> materials8 { set; get; } = Array.Empty<byte>();
        public IList<Matrix4x4> transforms { set; get; } = Array.Empty<Matrix4x4>();
        public IList<Vector4> bigVertices { set; get; } = Array.Empty<Vector4>();
        public IList<hkpCompressedMeshShapeBigTriangle> bigTriangles { set; get; } = Array.Empty<hkpCompressedMeshShapeBigTriangle>();
        public IList<hkpCompressedMeshShapeChunk> chunks { set; get; } = Array.Empty<hkpCompressedMeshShapeChunk>();
        public IList<hkpCompressedMeshShapeConvexPiece> convexPieces { set; get; } = Array.Empty<hkpCompressedMeshShapeConvexPiece>();
        public float error { set; get; }
        public hkAabb bounds { set; get; } = new();
        public uint defaultCollisionFilterInfo { set; get; }
        private object? meshMaterials { set; get; }
        public ushort materialStriding { set; get; }
        public ushort numMaterials { set; get; }
        public IList<hkpNamedMeshMaterial> namedMaterials { set; get; } = Array.Empty<hkpNamedMeshMaterial>();
        public Vector4 scaling { set; get; }

        public override uint Signature { set; get; } = 0xe3d1dba;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            bitsPerIndex = br.ReadInt32();
            bitsPerWIndex = br.ReadInt32();
            wIndexMask = br.ReadInt32();
            indexMask = br.ReadInt32();
            radius = br.ReadSingle();
            weldingType = br.ReadByte();
            materialType = br.ReadByte();
            br.Position += 2;
            materials = des.ReadUInt32Array(br);
            materials16 = des.ReadUInt16Array(br);
            materials8 = des.ReadByteArray(br);
            transforms = des.ReadQSTransformArray(br);
            bigVertices = des.ReadVector4Array(br);
            bigTriangles = des.ReadClassArray<hkpCompressedMeshShapeBigTriangle>(br);
            chunks = des.ReadClassArray<hkpCompressedMeshShapeChunk>(br);
            convexPieces = des.ReadClassArray<hkpCompressedMeshShapeConvexPiece>(br);
            error = br.ReadSingle();
            br.Position += 4;
            bounds.Read(des, br);
            defaultCollisionFilterInfo = br.ReadUInt32();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            materialStriding = br.ReadUInt16();
            numMaterials = br.ReadUInt16();
            br.Position += 4;
            namedMaterials = des.ReadClassArray<hkpNamedMeshMaterial>(br);
            br.Position += 8;
            scaling = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(bitsPerIndex);
            bw.WriteInt32(bitsPerWIndex);
            bw.WriteInt32(wIndexMask);
            bw.WriteInt32(indexMask);
            bw.WriteSingle(radius);
            bw.WriteByte(weldingType);
            bw.WriteByte(materialType);
            bw.Position += 2;
            s.WriteUInt32Array(bw, materials);
            s.WriteUInt16Array(bw, materials16);
            s.WriteByteArray(bw, materials8);
            s.WriteQSTransformArray(bw, transforms);
            s.WriteVector4Array(bw, bigVertices);
            s.WriteClassArray(bw, bigTriangles);
            s.WriteClassArray(bw, chunks);
            s.WriteClassArray(bw, convexPieces);
            bw.WriteSingle(error);
            bw.Position += 4;
            bounds.Write(s, bw);
            bw.WriteUInt32(defaultCollisionFilterInfo);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            bw.WriteUInt16(materialStriding);
            bw.WriteUInt16(numMaterials);
            bw.Position += 4;
            s.WriteClassArray(bw, namedMaterials);
            bw.Position += 8;
            bw.WriteVector4(scaling);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            bitsPerIndex = xd.ReadInt32(xe, nameof(bitsPerIndex));
            bitsPerWIndex = xd.ReadInt32(xe, nameof(bitsPerWIndex));
            wIndexMask = xd.ReadInt32(xe, nameof(wIndexMask));
            indexMask = xd.ReadInt32(xe, nameof(indexMask));
            radius = xd.ReadSingle(xe, nameof(radius));
            weldingType = xd.ReadFlag<WeldingType, byte>(xe, nameof(weldingType));
            materialType = xd.ReadFlag<MaterialType, byte>(xe, nameof(materialType));
            materials = xd.ReadUInt32Array(xe, nameof(materials));
            materials16 = xd.ReadUInt16Array(xe, nameof(materials16));
            materials8 = xd.ReadByteArray(xe, nameof(materials8));
            transforms = xd.ReadQSTransformArray(xe, nameof(transforms));
            bigVertices = xd.ReadVector4Array(xe, nameof(bigVertices));
            bigTriangles = xd.ReadClassArray<hkpCompressedMeshShapeBigTriangle>(xe, nameof(bigTriangles));
            chunks = xd.ReadClassArray<hkpCompressedMeshShapeChunk>(xe, nameof(chunks));
            convexPieces = xd.ReadClassArray<hkpCompressedMeshShapeConvexPiece>(xe, nameof(convexPieces));
            error = xd.ReadSingle(xe, nameof(error));
            bounds = xd.ReadClass<hkAabb>(xe, nameof(bounds));
            defaultCollisionFilterInfo = xd.ReadUInt32(xe, nameof(defaultCollisionFilterInfo));
            materialStriding = xd.ReadUInt16(xe, nameof(materialStriding));
            numMaterials = xd.ReadUInt16(xe, nameof(numMaterials));
            namedMaterials = xd.ReadClassArray<hkpNamedMeshMaterial>(xe, nameof(namedMaterials));
            scaling = xd.ReadVector4(xe, nameof(scaling));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(bitsPerIndex), bitsPerIndex);
            xs.WriteNumber(xe, nameof(bitsPerWIndex), bitsPerWIndex);
            xs.WriteNumber(xe, nameof(wIndexMask), wIndexMask);
            xs.WriteNumber(xe, nameof(indexMask), indexMask);
            xs.WriteFloat(xe, nameof(radius), radius);
            xs.WriteEnum<WeldingType, byte>(xe, nameof(weldingType), weldingType);
            xs.WriteEnum<MaterialType, byte>(xe, nameof(materialType), materialType);
            xs.WriteNumberArray(xe, nameof(materials), materials);
            xs.WriteNumberArray(xe, nameof(materials16), materials16);
            xs.WriteNumberArray(xe, nameof(materials8), materials8);
            xs.WriteQSTransformArray(xe, nameof(transforms), transforms);
            xs.WriteVector4Array(xe, nameof(bigVertices), bigVertices);
            xs.WriteClassArray(xe, nameof(bigTriangles), bigTriangles);
            xs.WriteClassArray(xe, nameof(chunks), chunks);
            xs.WriteClassArray(xe, nameof(convexPieces), convexPieces);
            xs.WriteFloat(xe, nameof(error), error);
            xs.WriteClass<hkAabb>(xe, nameof(bounds), bounds);
            xs.WriteNumber(xe, nameof(defaultCollisionFilterInfo), defaultCollisionFilterInfo);
            xs.WriteSerializeIgnored(xe, nameof(meshMaterials));
            xs.WriteNumber(xe, nameof(materialStriding), materialStriding);
            xs.WriteNumber(xe, nameof(numMaterials), numMaterials);
            xs.WriteClassArray(xe, nameof(namedMaterials), namedMaterials);
            xs.WriteVector4(xe, nameof(scaling), scaling);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCompressedMeshShape);
        }

        public bool Equals(hkpCompressedMeshShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   bitsPerIndex.Equals(other.bitsPerIndex) &&
                   bitsPerWIndex.Equals(other.bitsPerWIndex) &&
                   wIndexMask.Equals(other.wIndexMask) &&
                   indexMask.Equals(other.indexMask) &&
                   radius.Equals(other.radius) &&
                   weldingType.Equals(other.weldingType) &&
                   materialType.Equals(other.materialType) &&
                   materials.SequenceEqual(other.materials) &&
                   materials16.SequenceEqual(other.materials16) &&
                   materials8.SequenceEqual(other.materials8) &&
                   transforms.SequenceEqual(other.transforms) &&
                   bigVertices.SequenceEqual(other.bigVertices) &&
                   bigTriangles.SequenceEqual(other.bigTriangles) &&
                   chunks.SequenceEqual(other.chunks) &&
                   convexPieces.SequenceEqual(other.convexPieces) &&
                   error.Equals(other.error) &&
                   ((bounds is null && other.bounds is null) || (bounds is not null && other.bounds is not null && bounds.Equals((IHavokObject)other.bounds))) &&
                   defaultCollisionFilterInfo.Equals(other.defaultCollisionFilterInfo) &&
                   materialStriding.Equals(other.materialStriding) &&
                   numMaterials.Equals(other.numMaterials) &&
                   namedMaterials.SequenceEqual(other.namedMaterials) &&
                   scaling.Equals(other.scaling) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(bitsPerIndex);
            hashcode.Add(bitsPerWIndex);
            hashcode.Add(wIndexMask);
            hashcode.Add(indexMask);
            hashcode.Add(radius);
            hashcode.Add(weldingType);
            hashcode.Add(materialType);
            hashcode.Add(materials.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(materials16.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(materials8.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(transforms.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(bigVertices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(bigTriangles.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(chunks.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(convexPieces.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(error);
            hashcode.Add(bounds);
            hashcode.Add(defaultCollisionFilterInfo);
            hashcode.Add(materialStriding);
            hashcode.Add(numMaterials);
            hashcode.Add(namedMaterials.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(scaling);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

