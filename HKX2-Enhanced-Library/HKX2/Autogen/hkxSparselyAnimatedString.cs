using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxSparselyAnimatedString Signatire: 0x185da6fd size: 48 flags: FLAGS_NONE

    // strings class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // times class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkxSparselyAnimatedString : hkReferencedObject, IEquatable<hkxSparselyAnimatedString?>
    {
        public IList<string> strings { set; get; } = Array.Empty<string>();
        public IList<float> times { set; get; } = Array.Empty<float>();

        public override uint Signature { set; get; } = 0x185da6fd;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            strings = des.ReadStringPointerArray(br);
            times = des.ReadSingleArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointerArray(bw, strings);
            s.WriteSingleArray(bw, times);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            strings = xd.ReadStringArray(xe, nameof(strings));
            times = xd.ReadSingleArray(xe, nameof(times));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteStringArray(xe, nameof(strings), strings);
            xs.WriteFloatArray(xe, nameof(times), times);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxSparselyAnimatedString);
        }

        public bool Equals(hkxSparselyAnimatedString? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   strings.SequenceEqual(other.strings) &&
                   times.SequenceEqual(other.times) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(strings.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(times.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

