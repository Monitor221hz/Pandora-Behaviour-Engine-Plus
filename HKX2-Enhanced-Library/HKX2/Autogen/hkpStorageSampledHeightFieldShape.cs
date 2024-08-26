using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpStorageSampledHeightFieldShape Signatire: 0x15ff414b size: 144 flags: FLAGS_NONE

    // storage class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // triangleFlip class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    public partial class hkpStorageSampledHeightFieldShape : hkpSampledHeightFieldShape, IEquatable<hkpStorageSampledHeightFieldShape?>
    {
        public IList<float> storage { set; get; } = Array.Empty<float>();
        public bool triangleFlip { set; get; }

        public override uint Signature { set; get; } = 0x15ff414b;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            storage = des.ReadSingleArray(br);
            triangleFlip = br.ReadBoolean();
            br.Position += 15;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteSingleArray(bw, storage);
            bw.WriteBoolean(triangleFlip);
            bw.Position += 15;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            storage = xd.ReadSingleArray(xe, nameof(storage));
            triangleFlip = xd.ReadBoolean(xe, nameof(triangleFlip));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloatArray(xe, nameof(storage), storage);
            xs.WriteBoolean(xe, nameof(triangleFlip), triangleFlip);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpStorageSampledHeightFieldShape);
        }

        public bool Equals(hkpStorageSampledHeightFieldShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   storage.SequenceEqual(other.storage) &&
                   triangleFlip.Equals(other.triangleFlip) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(storage.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(triangleFlip);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

