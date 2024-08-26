using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkGeometry Signatire: 0x98dd8bdc size: 32 flags: FLAGS_NONE

    // vertices class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // triangles class: hkGeometryTriangle Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkGeometry : IHavokObject, IEquatable<hkGeometry?>
    {
        public IList<Vector4> vertices { set; get; } = Array.Empty<Vector4>();
        public IList<hkGeometryTriangle> triangles { set; get; } = Array.Empty<hkGeometryTriangle>();

        public virtual uint Signature { set; get; } = 0x98dd8bdc;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            vertices = des.ReadVector4Array(br);
            triangles = des.ReadClassArray<hkGeometryTriangle>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVector4Array(bw, vertices);
            s.WriteClassArray(bw, triangles);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            vertices = xd.ReadVector4Array(xe, nameof(vertices));
            triangles = xd.ReadClassArray<hkGeometryTriangle>(xe, nameof(triangles));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4Array(xe, nameof(vertices), vertices);
            xs.WriteClassArray(xe, nameof(triangles), triangles);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkGeometry);
        }

        public bool Equals(hkGeometry? other)
        {
            return other is not null &&
                   vertices.SequenceEqual(other.vertices) &&
                   triangles.SequenceEqual(other.triangles) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(vertices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(triangles.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

