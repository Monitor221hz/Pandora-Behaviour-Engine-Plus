using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxVertexIntDataChannel Signatire: 0x5a50e673 size: 32 flags: FLAGS_NONE

    // perVertexInts class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxVertexIntDataChannel : hkReferencedObject, IEquatable<hkxVertexIntDataChannel?>
    {
        public IList<int> perVertexInts { set; get; } = Array.Empty<int>();

        public override uint Signature { set; get; } = 0x5a50e673;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            perVertexInts = des.ReadInt32Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteInt32Array(bw, perVertexInts);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            perVertexInts = xd.ReadInt32Array(xe, nameof(perVertexInts));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(perVertexInts), perVertexInts);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxVertexIntDataChannel);
        }

        public bool Equals(hkxVertexIntDataChannel? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   perVertexInts.SequenceEqual(other.perVertexInts) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(perVertexInts.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

