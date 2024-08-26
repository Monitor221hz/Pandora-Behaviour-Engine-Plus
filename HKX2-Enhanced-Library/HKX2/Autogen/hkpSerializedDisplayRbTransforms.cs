using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSerializedDisplayRbTransforms Signatire: 0xc18650ac size: 32 flags: FLAGS_NONE

    // transforms class: hkpSerializedDisplayRbTransformsDisplayTransformPair Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpSerializedDisplayRbTransforms : hkReferencedObject, IEquatable<hkpSerializedDisplayRbTransforms?>
    {
        public IList<hkpSerializedDisplayRbTransformsDisplayTransformPair> transforms { set; get; } = Array.Empty<hkpSerializedDisplayRbTransformsDisplayTransformPair>();

        public override uint Signature { set; get; } = 0xc18650ac;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            transforms = des.ReadClassArray<hkpSerializedDisplayRbTransformsDisplayTransformPair>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, transforms);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            transforms = xd.ReadClassArray<hkpSerializedDisplayRbTransformsDisplayTransformPair>(xe, nameof(transforms));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(transforms), transforms);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSerializedDisplayRbTransforms);
        }

        public bool Equals(hkpSerializedDisplayRbTransforms? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   transforms.SequenceEqual(other.transforms) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(transforms.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

