using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxSparselyAnimatedBool Signatire: 0x7a894596 size: 48 flags: FLAGS_NONE

    // bools class:  Type.TYPE_ARRAY Type.TYPE_BOOL arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // times class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkxSparselyAnimatedBool : hkReferencedObject, IEquatable<hkxSparselyAnimatedBool?>
    {
        public IList<bool> bools { set; get; } = Array.Empty<bool>();
        public IList<float> times { set; get; } = Array.Empty<float>();

        public override uint Signature { set; get; } = 0x7a894596;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            bools = des.ReadBooleanArray(br);
            times = des.ReadSingleArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteBooleanArray(bw, bools);
            s.WriteSingleArray(bw, times);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            bools = xd.ReadBooleanArray(xe, nameof(bools));
            times = xd.ReadSingleArray(xe, nameof(times));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBooleanArray(xe, nameof(bools), bools);
            xs.WriteFloatArray(xe, nameof(times), times);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxSparselyAnimatedBool);
        }

        public bool Equals(hkxSparselyAnimatedBool? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   bools.SequenceEqual(other.bools) &&
                   times.SequenceEqual(other.times) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(bools.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(times.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

