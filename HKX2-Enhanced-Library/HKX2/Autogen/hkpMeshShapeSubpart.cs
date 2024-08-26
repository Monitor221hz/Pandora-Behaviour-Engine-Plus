using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMeshShapeSubpart Signatire: 0x27336e5d size: 80 flags: FLAGS_NONE

    // vertexBase class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // vertexStriding class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // numVertices class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // indexBase class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // stridingType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: MeshShapeIndexStridingType
    // materialIndexStridingType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 25 flags: FLAGS_NONE enum: MeshShapeMaterialIndexStridingType
    // indexStriding class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // flipAlternateTriangles class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // numTriangles class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // materialIndexBase class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 40 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // materialIndexStriding class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // materialBase class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // materialStriding class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // numMaterials class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // triangleOffset class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    public partial class hkpMeshShapeSubpart : IHavokObject, IEquatable<hkpMeshShapeSubpart?>
    {
        private object? vertexBase { set; get; }
        public int vertexStriding { set; get; }
        public int numVertices { set; get; }
        private object? indexBase { set; get; }
        public sbyte stridingType { set; get; }
        public sbyte materialIndexStridingType { set; get; }
        public int indexStriding { set; get; }
        public int flipAlternateTriangles { set; get; }
        public int numTriangles { set; get; }
        private object? materialIndexBase { set; get; }
        public int materialIndexStriding { set; get; }
        private object? materialBase { set; get; }
        public int materialStriding { set; get; }
        public int numMaterials { set; get; }
        public int triangleOffset { set; get; }

        public virtual uint Signature { set; get; } = 0x27336e5d;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            des.ReadEmptyPointer(br);
            vertexStriding = br.ReadInt32();
            numVertices = br.ReadInt32();
            des.ReadEmptyPointer(br);
            stridingType = br.ReadSByte();
            materialIndexStridingType = br.ReadSByte();
            br.Position += 2;
            indexStriding = br.ReadInt32();
            flipAlternateTriangles = br.ReadInt32();
            numTriangles = br.ReadInt32();
            des.ReadEmptyPointer(br);
            materialIndexStriding = br.ReadInt32();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            materialStriding = br.ReadInt32();
            numMaterials = br.ReadInt32();
            triangleOffset = br.ReadInt32();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVoidPointer(bw);
            bw.WriteInt32(vertexStriding);
            bw.WriteInt32(numVertices);
            s.WriteVoidPointer(bw);
            bw.WriteSByte(stridingType);
            bw.WriteSByte(materialIndexStridingType);
            bw.Position += 2;
            bw.WriteInt32(indexStriding);
            bw.WriteInt32(flipAlternateTriangles);
            bw.WriteInt32(numTriangles);
            s.WriteVoidPointer(bw);
            bw.WriteInt32(materialIndexStriding);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            bw.WriteInt32(materialStriding);
            bw.WriteInt32(numMaterials);
            bw.WriteInt32(triangleOffset);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            vertexStriding = xd.ReadInt32(xe, nameof(vertexStriding));
            numVertices = xd.ReadInt32(xe, nameof(numVertices));
            stridingType = xd.ReadFlag<MeshShapeIndexStridingType, sbyte>(xe, nameof(stridingType));
            materialIndexStridingType = xd.ReadFlag<MeshShapeMaterialIndexStridingType, sbyte>(xe, nameof(materialIndexStridingType));
            indexStriding = xd.ReadInt32(xe, nameof(indexStriding));
            flipAlternateTriangles = xd.ReadInt32(xe, nameof(flipAlternateTriangles));
            numTriangles = xd.ReadInt32(xe, nameof(numTriangles));
            materialIndexStriding = xd.ReadInt32(xe, nameof(materialIndexStriding));
            materialStriding = xd.ReadInt32(xe, nameof(materialStriding));
            numMaterials = xd.ReadInt32(xe, nameof(numMaterials));
            triangleOffset = xd.ReadInt32(xe, nameof(triangleOffset));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(vertexBase));
            xs.WriteNumber(xe, nameof(vertexStriding), vertexStriding);
            xs.WriteNumber(xe, nameof(numVertices), numVertices);
            xs.WriteSerializeIgnored(xe, nameof(indexBase));
            xs.WriteEnum<MeshShapeIndexStridingType, sbyte>(xe, nameof(stridingType), stridingType);
            xs.WriteEnum<MeshShapeMaterialIndexStridingType, sbyte>(xe, nameof(materialIndexStridingType), materialIndexStridingType);
            xs.WriteNumber(xe, nameof(indexStriding), indexStriding);
            xs.WriteNumber(xe, nameof(flipAlternateTriangles), flipAlternateTriangles);
            xs.WriteNumber(xe, nameof(numTriangles), numTriangles);
            xs.WriteSerializeIgnored(xe, nameof(materialIndexBase));
            xs.WriteNumber(xe, nameof(materialIndexStriding), materialIndexStriding);
            xs.WriteSerializeIgnored(xe, nameof(materialBase));
            xs.WriteNumber(xe, nameof(materialStriding), materialStriding);
            xs.WriteNumber(xe, nameof(numMaterials), numMaterials);
            xs.WriteNumber(xe, nameof(triangleOffset), triangleOffset);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMeshShapeSubpart);
        }

        public bool Equals(hkpMeshShapeSubpart? other)
        {
            return other is not null &&
                   vertexStriding.Equals(other.vertexStriding) &&
                   numVertices.Equals(other.numVertices) &&
                   stridingType.Equals(other.stridingType) &&
                   materialIndexStridingType.Equals(other.materialIndexStridingType) &&
                   indexStriding.Equals(other.indexStriding) &&
                   flipAlternateTriangles.Equals(other.flipAlternateTriangles) &&
                   numTriangles.Equals(other.numTriangles) &&
                   materialIndexStriding.Equals(other.materialIndexStriding) &&
                   materialStriding.Equals(other.materialStriding) &&
                   numMaterials.Equals(other.numMaterials) &&
                   triangleOffset.Equals(other.triangleOffset) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(vertexStriding);
            hashcode.Add(numVertices);
            hashcode.Add(stridingType);
            hashcode.Add(materialIndexStridingType);
            hashcode.Add(indexStriding);
            hashcode.Add(flipAlternateTriangles);
            hashcode.Add(numTriangles);
            hashcode.Add(materialIndexStriding);
            hashcode.Add(materialStriding);
            hashcode.Add(numMaterials);
            hashcode.Add(triangleOffset);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

