using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxVertexFloatDataChannel Signatire: 0xbeeb397c size: 40 flags: FLAGS_NONE

    // perVertexFloats class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // dimensions class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: VertexFloatDimensions
    public partial class hkxVertexFloatDataChannel : hkReferencedObject, IEquatable<hkxVertexFloatDataChannel?>
    {
        public IList<float> perVertexFloats { set; get; } = Array.Empty<float>();
        public byte dimensions { set; get; }

        public override uint Signature { set; get; } = 0xbeeb397c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            perVertexFloats = des.ReadSingleArray(br);
            dimensions = br.ReadByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteSingleArray(bw, perVertexFloats);
            bw.WriteByte(dimensions);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            perVertexFloats = xd.ReadSingleArray(xe, nameof(perVertexFloats));
            dimensions = xd.ReadFlag<VertexFloatDimensions, byte>(xe, nameof(dimensions));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloatArray(xe, nameof(perVertexFloats), perVertexFloats);
            xs.WriteEnum<VertexFloatDimensions, byte>(xe, nameof(dimensions), dimensions);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxVertexFloatDataChannel);
        }

        public bool Equals(hkxVertexFloatDataChannel? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   perVertexFloats.SequenceEqual(other.perVertexFloats) &&
                   dimensions.Equals(other.dimensions) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(perVertexFloats.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(dimensions);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

