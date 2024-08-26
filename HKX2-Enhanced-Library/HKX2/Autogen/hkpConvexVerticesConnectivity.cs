using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConvexVerticesConnectivity Signatire: 0x63d38e9c size: 48 flags: FLAGS_NONE

    // vertexIndices class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // numVerticesPerFace class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkpConvexVerticesConnectivity : hkReferencedObject, IEquatable<hkpConvexVerticesConnectivity?>
    {
        public IList<ushort> vertexIndices { set; get; } = Array.Empty<ushort>();
        public IList<byte> numVerticesPerFace { set; get; } = Array.Empty<byte>();

        public override uint Signature { set; get; } = 0x63d38e9c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            vertexIndices = des.ReadUInt16Array(br);
            numVerticesPerFace = des.ReadByteArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteUInt16Array(bw, vertexIndices);
            s.WriteByteArray(bw, numVerticesPerFace);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            vertexIndices = xd.ReadUInt16Array(xe, nameof(vertexIndices));
            numVerticesPerFace = xd.ReadByteArray(xe, nameof(numVerticesPerFace));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(vertexIndices), vertexIndices);
            xs.WriteNumberArray(xe, nameof(numVerticesPerFace), numVerticesPerFace);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConvexVerticesConnectivity);
        }

        public bool Equals(hkpConvexVerticesConnectivity? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   vertexIndices.SequenceEqual(other.vertexIndices) &&
                   numVerticesPerFace.SequenceEqual(other.numVerticesPerFace) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(vertexIndices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(numVerticesPerFace.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

