using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConvexPieceStreamData Signatire: 0xa5bd1d6e size: 64 flags: FLAGS_NONE

    // convexPieceStream class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // convexPieceOffsets class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // convexPieceSingleTriangles class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpConvexPieceStreamData : hkReferencedObject, IEquatable<hkpConvexPieceStreamData?>
    {
        public IList<uint> convexPieceStream { set; get; } = Array.Empty<uint>();
        public IList<uint> convexPieceOffsets { set; get; } = Array.Empty<uint>();
        public IList<uint> convexPieceSingleTriangles { set; get; } = Array.Empty<uint>();

        public override uint Signature { set; get; } = 0xa5bd1d6e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            convexPieceStream = des.ReadUInt32Array(br);
            convexPieceOffsets = des.ReadUInt32Array(br);
            convexPieceSingleTriangles = des.ReadUInt32Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteUInt32Array(bw, convexPieceStream);
            s.WriteUInt32Array(bw, convexPieceOffsets);
            s.WriteUInt32Array(bw, convexPieceSingleTriangles);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            convexPieceStream = xd.ReadUInt32Array(xe, nameof(convexPieceStream));
            convexPieceOffsets = xd.ReadUInt32Array(xe, nameof(convexPieceOffsets));
            convexPieceSingleTriangles = xd.ReadUInt32Array(xe, nameof(convexPieceSingleTriangles));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(convexPieceStream), convexPieceStream);
            xs.WriteNumberArray(xe, nameof(convexPieceOffsets), convexPieceOffsets);
            xs.WriteNumberArray(xe, nameof(convexPieceSingleTriangles), convexPieceSingleTriangles);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConvexPieceStreamData);
        }

        public bool Equals(hkpConvexPieceStreamData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   convexPieceStream.SequenceEqual(other.convexPieceStream) &&
                   convexPieceOffsets.SequenceEqual(other.convexPieceOffsets) &&
                   convexPieceSingleTriangles.SequenceEqual(other.convexPieceSingleTriangles) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(convexPieceStream.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(convexPieceOffsets.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(convexPieceSingleTriangles.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

