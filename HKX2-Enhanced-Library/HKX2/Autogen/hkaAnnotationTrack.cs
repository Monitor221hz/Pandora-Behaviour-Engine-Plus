using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaAnnotationTrack Signatire: 0xd4114fdd size: 24 flags: FLAGS_NONE

    // trackName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // annotations class: hkaAnnotationTrackAnnotation Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkaAnnotationTrack : IHavokObject, IEquatable<hkaAnnotationTrack?>
    {
        public string trackName { set; get; } = "";
        public IList<hkaAnnotationTrackAnnotation> annotations { set; get; } = Array.Empty<hkaAnnotationTrackAnnotation>();

        public virtual uint Signature { set; get; } = 0xd4114fdd;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            trackName = des.ReadStringPointer(br);
            annotations = des.ReadClassArray<hkaAnnotationTrackAnnotation>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteStringPointer(bw, trackName);
            s.WriteClassArray(bw, annotations);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            trackName = xd.ReadString(xe, nameof(trackName));
            annotations = xd.ReadClassArray<hkaAnnotationTrackAnnotation>(xe, nameof(annotations));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(trackName), trackName);
            xs.WriteClassArray(xe, nameof(annotations), annotations);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaAnnotationTrack);
        }

        public bool Equals(hkaAnnotationTrack? other)
        {
            return other is not null &&
                   (trackName is null && other.trackName is null || trackName == other.trackName || trackName is null && other.trackName == "" || trackName == "" && other.trackName is null) &&
                   annotations.SequenceEqual(other.annotations) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(trackName);
            hashcode.Add(annotations.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

