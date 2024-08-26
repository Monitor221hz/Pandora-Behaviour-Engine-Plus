using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxTriangleSelectionChannel Signatire: 0xa02cfca9 size: 32 flags: FLAGS_NONE

    // selectedTriangles class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxTriangleSelectionChannel : hkReferencedObject, IEquatable<hkxTriangleSelectionChannel?>
    {
        public IList<int> selectedTriangles { set; get; } = Array.Empty<int>();

        public override uint Signature { set; get; } = 0xa02cfca9;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            selectedTriangles = des.ReadInt32Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteInt32Array(bw, selectedTriangles);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            selectedTriangles = xd.ReadInt32Array(xe, nameof(selectedTriangles));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(selectedTriangles), selectedTriangles);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxTriangleSelectionChannel);
        }

        public bool Equals(hkxTriangleSelectionChannel? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   selectedTriangles.SequenceEqual(other.selectedTriangles) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(selectedTriangles.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

