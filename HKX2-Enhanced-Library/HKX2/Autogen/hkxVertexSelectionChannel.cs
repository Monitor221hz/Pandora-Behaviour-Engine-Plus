using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxVertexSelectionChannel Signatire: 0x866ec6d0 size: 32 flags: FLAGS_NONE

    // selectedVertices class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxVertexSelectionChannel : hkReferencedObject, IEquatable<hkxVertexSelectionChannel?>
    {
        public IList<int> selectedVertices { set; get; } = Array.Empty<int>();

        public override uint Signature { set; get; } = 0x866ec6d0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            selectedVertices = des.ReadInt32Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteInt32Array(bw, selectedVertices);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            selectedVertices = xd.ReadInt32Array(xe, nameof(selectedVertices));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(selectedVertices), selectedVertices);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxVertexSelectionChannel);
        }

        public bool Equals(hkxVertexSelectionChannel? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   selectedVertices.SequenceEqual(other.selectedVertices) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(selectedVertices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

