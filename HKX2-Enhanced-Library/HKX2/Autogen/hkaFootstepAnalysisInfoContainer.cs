using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaFootstepAnalysisInfoContainer Signatire: 0x1d81207c size: 32 flags: FLAGS_NONE

    // previewInfo class: hkaFootstepAnalysisInfo Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkaFootstepAnalysisInfoContainer : hkReferencedObject, IEquatable<hkaFootstepAnalysisInfoContainer?>
    {
        public IList<hkaFootstepAnalysisInfo> previewInfo { set; get; } = Array.Empty<hkaFootstepAnalysisInfo>();

        public override uint Signature { set; get; } = 0x1d81207c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            previewInfo = des.ReadClassPointerArray<hkaFootstepAnalysisInfo>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, previewInfo);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            previewInfo = xd.ReadClassPointerArray<hkaFootstepAnalysisInfo>(this, xe, nameof(previewInfo));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(previewInfo), previewInfo!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaFootstepAnalysisInfoContainer);
        }

        public bool Equals(hkaFootstepAnalysisInfoContainer? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   previewInfo.SequenceEqual(other.previewInfo) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(previewInfo.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

