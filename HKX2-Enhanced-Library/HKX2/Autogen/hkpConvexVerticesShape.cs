using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConvexVerticesShape Signatire: 0x28726ad8 size: 144 flags: FLAGS_NONE

    // aabbHalfExtents class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // aabbCenter class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // rotatedVertices class: hkpConvexVerticesShapeFourVectors Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // numVertices class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // externalObject class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // getFaceNormals class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // planeEquations class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // connectivity class: hkpConvexVerticesConnectivity Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    public partial class hkpConvexVerticesShape : hkpConvexShape, IEquatable<hkpConvexVerticesShape?>
    {
        public Vector4 aabbHalfExtents { set; get; }
        public Vector4 aabbCenter { set; get; }
        public IList<hkpConvexVerticesShapeFourVectors> rotatedVertices { set; get; } = Array.Empty<hkpConvexVerticesShapeFourVectors>();
        public int numVertices { set; get; }
        private object? externalObject { set; get; }
        private object? getFaceNormals { set; get; }
        public IList<Vector4> planeEquations { set; get; } = Array.Empty<Vector4>();
        public hkpConvexVerticesConnectivity? connectivity { set; get; }

        public override uint Signature { set; get; } = 0x28726ad8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            aabbHalfExtents = br.ReadVector4();
            aabbCenter = br.ReadVector4();
            rotatedVertices = des.ReadClassArray<hkpConvexVerticesShapeFourVectors>(br);
            numVertices = br.ReadInt32();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            planeEquations = des.ReadVector4Array(br);
            connectivity = des.ReadClassPointer<hkpConvexVerticesConnectivity>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            bw.WriteVector4(aabbHalfExtents);
            bw.WriteVector4(aabbCenter);
            s.WriteClassArray(bw, rotatedVertices);
            bw.WriteInt32(numVertices);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVector4Array(bw, planeEquations);
            s.WriteClassPointer(bw, connectivity);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            aabbHalfExtents = xd.ReadVector4(xe, nameof(aabbHalfExtents));
            aabbCenter = xd.ReadVector4(xe, nameof(aabbCenter));
            rotatedVertices = xd.ReadClassArray<hkpConvexVerticesShapeFourVectors>(xe, nameof(rotatedVertices));
            numVertices = xd.ReadInt32(xe, nameof(numVertices));
            planeEquations = xd.ReadVector4Array(xe, nameof(planeEquations));
            connectivity = xd.ReadClassPointer<hkpConvexVerticesConnectivity>(this, xe, nameof(connectivity));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(aabbHalfExtents), aabbHalfExtents);
            xs.WriteVector4(xe, nameof(aabbCenter), aabbCenter);
            xs.WriteClassArray(xe, nameof(rotatedVertices), rotatedVertices);
            xs.WriteNumber(xe, nameof(numVertices), numVertices);
            xs.WriteSerializeIgnored(xe, nameof(externalObject));
            xs.WriteSerializeIgnored(xe, nameof(getFaceNormals));
            xs.WriteVector4Array(xe, nameof(planeEquations), planeEquations);
            xs.WriteClassPointer(xe, nameof(connectivity), connectivity);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConvexVerticesShape);
        }

        public bool Equals(hkpConvexVerticesShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   aabbHalfExtents.Equals(other.aabbHalfExtents) &&
                   aabbCenter.Equals(other.aabbCenter) &&
                   rotatedVertices.SequenceEqual(other.rotatedVertices) &&
                   numVertices.Equals(other.numVertices) &&
                   planeEquations.SequenceEqual(other.planeEquations) &&
                   ((connectivity is null && other.connectivity is null) || (connectivity is not null && other.connectivity is not null && connectivity.Equals((IHavokObject)other.connectivity))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(aabbHalfExtents);
            hashcode.Add(aabbCenter);
            hashcode.Add(rotatedVertices.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(numVertices);
            hashcode.Add(planeEquations.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(connectivity);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

