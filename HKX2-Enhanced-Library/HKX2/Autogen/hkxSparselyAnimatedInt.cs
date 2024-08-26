using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxSparselyAnimatedInt Signatire: 0xca961951 size: 48 flags: FLAGS_NONE

    // ints class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // times class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkxSparselyAnimatedInt : hkReferencedObject, IEquatable<hkxSparselyAnimatedInt?>
    {
        public IList<int> ints { set; get; } = Array.Empty<int>();
        public IList<float> times { set; get; } = Array.Empty<float>();

        public override uint Signature { set; get; } = 0xca961951;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            ints = des.ReadInt32Array(br);
            times = des.ReadSingleArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteInt32Array(bw, ints);
            s.WriteSingleArray(bw, times);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            ints = xd.ReadInt32Array(xe, nameof(ints));
            times = xd.ReadSingleArray(xe, nameof(times));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(ints), ints);
            xs.WriteFloatArray(xe, nameof(times), times);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxSparselyAnimatedInt);
        }

        public bool Equals(hkxSparselyAnimatedInt? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ints.SequenceEqual(other.ints) &&
                   times.SequenceEqual(other.times) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(ints.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(times.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

