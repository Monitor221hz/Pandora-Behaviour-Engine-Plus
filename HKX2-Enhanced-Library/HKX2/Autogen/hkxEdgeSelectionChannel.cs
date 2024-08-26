using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxEdgeSelectionChannel Signatire: 0x9ad32a5e size: 32 flags: FLAGS_NONE

    // selectedEdges class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxEdgeSelectionChannel : hkReferencedObject, IEquatable<hkxEdgeSelectionChannel?>
    {
        public IList<int> selectedEdges { set; get; } = Array.Empty<int>();

        public override uint Signature { set; get; } = 0x9ad32a5e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            selectedEdges = des.ReadInt32Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteInt32Array(bw, selectedEdges);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            selectedEdges = xd.ReadInt32Array(xe, nameof(selectedEdges));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(selectedEdges), selectedEdges);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxEdgeSelectionChannel);
        }

        public bool Equals(hkxEdgeSelectionChannel? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   selectedEdges.SequenceEqual(other.selectedEdges) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(selectedEdges.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

