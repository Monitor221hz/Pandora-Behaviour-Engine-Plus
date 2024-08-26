using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaAnimationPreviewColorContainer Signatire: 0x4bc4c3e0 size: 32 flags: FLAGS_NONE

    // previewColor class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkaAnimationPreviewColorContainer : hkReferencedObject, IEquatable<hkaAnimationPreviewColorContainer?>
    {
        public IList<uint> previewColor { set; get; } = Array.Empty<uint>();

        public override uint Signature { set; get; } = 0x4bc4c3e0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            previewColor = des.ReadUInt32Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteUInt32Array(bw, previewColor);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            previewColor = xd.ReadUInt32Array(xe, nameof(previewColor));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(previewColor), previewColor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaAnimationPreviewColorContainer);
        }

        public bool Equals(hkaAnimationPreviewColorContainer? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   previewColor.SequenceEqual(other.previewColor) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(previewColor.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

