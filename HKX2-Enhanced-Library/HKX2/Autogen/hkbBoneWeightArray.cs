using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBoneWeightArray Signatire: 0xcd902b77 size: 64 flags: FLAGS_NONE

    // boneWeights class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkbBoneWeightArray : hkbBindable, IEquatable<hkbBoneWeightArray?>
    {
        public IList<float> boneWeights { set; get; } = Array.Empty<float>();

        public override uint Signature { set; get; } = 0xcd902b77;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            boneWeights = des.ReadSingleArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteSingleArray(bw, boneWeights);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            boneWeights = xd.ReadSingleArray(xe, nameof(boneWeights));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloatArray(xe, nameof(boneWeights), boneWeights);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBoneWeightArray);
        }

        public bool Equals(hkbBoneWeightArray? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   boneWeights.SequenceEqual(other.boneWeights) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(boneWeights.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

