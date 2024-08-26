using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCompressedSampledHeightFieldShape Signatire: 0x97b6e143 size: 144 flags: FLAGS_NONE

    // storage class:  Type.TYPE_ARRAY Type.TYPE_UINT16 arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // triangleFlip class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // offset class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 132 flags: FLAGS_NONE enum: 
    // scale class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    public partial class hkpCompressedSampledHeightFieldShape : hkpSampledHeightFieldShape, IEquatable<hkpCompressedSampledHeightFieldShape?>
    {
        public IList<ushort> storage { set; get; } = Array.Empty<ushort>();
        public bool triangleFlip { set; get; }
        public float offset { set; get; }
        public float scale { set; get; }

        public override uint Signature { set; get; } = 0x97b6e143;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            storage = des.ReadUInt16Array(br);
            triangleFlip = br.ReadBoolean();
            br.Position += 3;
            offset = br.ReadSingle();
            scale = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteUInt16Array(bw, storage);
            bw.WriteBoolean(triangleFlip);
            bw.Position += 3;
            bw.WriteSingle(offset);
            bw.WriteSingle(scale);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            storage = xd.ReadUInt16Array(xe, nameof(storage));
            triangleFlip = xd.ReadBoolean(xe, nameof(triangleFlip));
            offset = xd.ReadSingle(xe, nameof(offset));
            scale = xd.ReadSingle(xe, nameof(scale));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(storage), storage);
            xs.WriteBoolean(xe, nameof(triangleFlip), triangleFlip);
            xs.WriteFloat(xe, nameof(offset), offset);
            xs.WriteFloat(xe, nameof(scale), scale);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCompressedSampledHeightFieldShape);
        }

        public bool Equals(hkpCompressedSampledHeightFieldShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   storage.SequenceEqual(other.storage) &&
                   triangleFlip.Equals(other.triangleFlip) &&
                   offset.Equals(other.offset) &&
                   scale.Equals(other.scale) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(storage.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(triangleFlip);
            hashcode.Add(offset);
            hashcode.Add(scale);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

