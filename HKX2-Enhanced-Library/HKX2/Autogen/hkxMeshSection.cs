using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxMeshSection Signatire: 0xe2286cf8 size: 64 flags: FLAGS_NONE

    // vertexBuffer class: hkxVertexBuffer Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // indexBuffers class: hkxIndexBuffer Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // material class: hkxMaterial Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // userChannels class: hkReferencedObject Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkxMeshSection : hkReferencedObject, IEquatable<hkxMeshSection?>
    {
        public hkxVertexBuffer? vertexBuffer { set; get; }
        public IList<hkxIndexBuffer> indexBuffers { set; get; } = Array.Empty<hkxIndexBuffer>();
        public hkxMaterial? material { set; get; }
        public IList<hkReferencedObject> userChannels { set; get; } = Array.Empty<hkReferencedObject>();

        public override uint Signature { set; get; } = 0xe2286cf8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            vertexBuffer = des.ReadClassPointer<hkxVertexBuffer>(br);
            indexBuffers = des.ReadClassPointerArray<hkxIndexBuffer>(br);
            material = des.ReadClassPointer<hkxMaterial>(br);
            userChannels = des.ReadClassPointerArray<hkReferencedObject>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, vertexBuffer);
            s.WriteClassPointerArray(bw, indexBuffers);
            s.WriteClassPointer(bw, material);
            s.WriteClassPointerArray(bw, userChannels);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            vertexBuffer = xd.ReadClassPointer<hkxVertexBuffer>(this, xe, nameof(vertexBuffer));
            indexBuffers = xd.ReadClassPointerArray<hkxIndexBuffer>(this, xe, nameof(indexBuffers));
            material = xd.ReadClassPointer<hkxMaterial>(this, xe, nameof(material));
            userChannels = xd.ReadClassPointerArray<hkReferencedObject>(this, xe, nameof(userChannels));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(vertexBuffer), vertexBuffer);
            xs.WriteClassPointerArray(xe, nameof(indexBuffers), indexBuffers!);
            xs.WriteClassPointer(xe, nameof(material), material);
            xs.WriteClassPointerArray(xe, nameof(userChannels), userChannels!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxMeshSection);
        }

        public bool Equals(hkxMeshSection? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((vertexBuffer is null && other.vertexBuffer is null) || (vertexBuffer is not null && other.vertexBuffer is not null && vertexBuffer.Equals((IHavokObject)other.vertexBuffer))) &&
                   indexBuffers.SequenceEqual(other.indexBuffers) &&
                   ((material is null && other.material is null) || (material is not null && other.material is not null && material.Equals((IHavokObject)other.material))) &&
                   userChannels.SequenceEqual(other.userChannels) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(vertexBuffer);
            hashcode.Add(indexBuffers.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(material);
            hashcode.Add(userChannels.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

