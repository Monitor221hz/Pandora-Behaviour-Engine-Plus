using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaInterleavedUncompressedAnimation Signatire: 0x930af031 size: 88 flags: FLAGS_NONE

    // transforms class:  Type.TYPE_ARRAY Type.TYPE_QSTRANSFORM arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // floats class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    public partial class hkaInterleavedUncompressedAnimation : hkaAnimation, IEquatable<hkaInterleavedUncompressedAnimation?>
    {
        public IList<Matrix4x4> transforms { set; get; } = Array.Empty<Matrix4x4>();
        public IList<float> floats { set; get; } = Array.Empty<float>();

        public override uint Signature { set; get; } = 0x930af031;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            transforms = des.ReadQSTransformArray(br);
            floats = des.ReadSingleArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQSTransformArray(bw, transforms);
            s.WriteSingleArray(bw, floats);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            transforms = xd.ReadQSTransformArray(xe, nameof(transforms));
            floats = xd.ReadSingleArray(xe, nameof(floats));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQSTransformArray(xe, nameof(transforms), transforms);
            xs.WriteFloatArray(xe, nameof(floats), floats);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaInterleavedUncompressedAnimation);
        }

        public bool Equals(hkaInterleavedUncompressedAnimation? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   transforms.SequenceEqual(other.transforms) &&
                   floats.SequenceEqual(other.floats) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(transforms.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(floats.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

