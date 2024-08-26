using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaAnimation Signatire: 0xa6fa7e88 size: 56 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_INT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: AnimationType
    // duration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // numberOfTransformTracks class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // numberOfFloatTracks class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // extractedMotion class: hkaAnimatedReferenceFrame Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // annotationTracks class: hkaAnnotationTrack Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    public partial class hkaAnimation : hkReferencedObject, IEquatable<hkaAnimation?>
    {
        public int type { set; get; }
        public float duration { set; get; }
        public int numberOfTransformTracks { set; get; }
        public int numberOfFloatTracks { set; get; }
        public hkaAnimatedReferenceFrame? extractedMotion { set; get; }
        public IList<hkaAnnotationTrack> annotationTracks { set; get; } = Array.Empty<hkaAnnotationTrack>();

        public override uint Signature { set; get; } = 0xa6fa7e88;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            type = br.ReadInt32();
            duration = br.ReadSingle();
            numberOfTransformTracks = br.ReadInt32();
            numberOfFloatTracks = br.ReadInt32();
            extractedMotion = des.ReadClassPointer<hkaAnimatedReferenceFrame>(br);
            annotationTracks = des.ReadClassArray<hkaAnnotationTrack>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(type);
            bw.WriteSingle(duration);
            bw.WriteInt32(numberOfTransformTracks);
            bw.WriteInt32(numberOfFloatTracks);
            s.WriteClassPointer(bw, extractedMotion);
            s.WriteClassArray(bw, annotationTracks);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            type = xd.ReadFlag<AnimationType, int>(xe, nameof(type));
            duration = xd.ReadSingle(xe, nameof(duration));
            numberOfTransformTracks = xd.ReadInt32(xe, nameof(numberOfTransformTracks));
            numberOfFloatTracks = xd.ReadInt32(xe, nameof(numberOfFloatTracks));
            extractedMotion = xd.ReadClassPointer<hkaAnimatedReferenceFrame>(this, xe, nameof(extractedMotion));
            annotationTracks = xd.ReadClassArray<hkaAnnotationTrack>(xe, nameof(annotationTracks));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<AnimationType, int>(xe, nameof(type), type);
            xs.WriteFloat(xe, nameof(duration), duration);
            xs.WriteNumber(xe, nameof(numberOfTransformTracks), numberOfTransformTracks);
            xs.WriteNumber(xe, nameof(numberOfFloatTracks), numberOfFloatTracks);
            xs.WriteClassPointer(xe, nameof(extractedMotion), extractedMotion);
            xs.WriteClassArray(xe, nameof(annotationTracks), annotationTracks);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaAnimation);
        }

        public bool Equals(hkaAnimation? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   type.Equals(other.type) &&
                   duration.Equals(other.duration) &&
                   numberOfTransformTracks.Equals(other.numberOfTransformTracks) &&
                   numberOfFloatTracks.Equals(other.numberOfFloatTracks) &&
                   ((extractedMotion is null && other.extractedMotion is null) || (extractedMotion is not null && other.extractedMotion is not null && extractedMotion.Equals((IHavokObject)other.extractedMotion))) &&
                   annotationTracks.SequenceEqual(other.annotationTracks) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(type);
            hashcode.Add(duration);
            hashcode.Add(numberOfTransformTracks);
            hashcode.Add(numberOfFloatTracks);
            hashcode.Add(extractedMotion);
            hashcode.Add(annotationTracks.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

