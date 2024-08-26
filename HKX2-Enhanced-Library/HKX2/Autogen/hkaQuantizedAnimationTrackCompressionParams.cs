using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaQuantizedAnimationTrackCompressionParams Signatire: 0xf7d64649 size: 16 flags: FLAGS_NONE

    // rotationTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // translationTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // scaleTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // floatingTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkaQuantizedAnimationTrackCompressionParams : IHavokObject, IEquatable<hkaQuantizedAnimationTrackCompressionParams?>
    {
        public float rotationTolerance { set; get; }
        public float translationTolerance { set; get; }
        public float scaleTolerance { set; get; }
        public float floatingTolerance { set; get; }

        public virtual uint Signature { set; get; } = 0xf7d64649;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            rotationTolerance = br.ReadSingle();
            translationTolerance = br.ReadSingle();
            scaleTolerance = br.ReadSingle();
            floatingTolerance = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(rotationTolerance);
            bw.WriteSingle(translationTolerance);
            bw.WriteSingle(scaleTolerance);
            bw.WriteSingle(floatingTolerance);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            rotationTolerance = xd.ReadSingle(xe, nameof(rotationTolerance));
            translationTolerance = xd.ReadSingle(xe, nameof(translationTolerance));
            scaleTolerance = xd.ReadSingle(xe, nameof(scaleTolerance));
            floatingTolerance = xd.ReadSingle(xe, nameof(floatingTolerance));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(rotationTolerance), rotationTolerance);
            xs.WriteFloat(xe, nameof(translationTolerance), translationTolerance);
            xs.WriteFloat(xe, nameof(scaleTolerance), scaleTolerance);
            xs.WriteFloat(xe, nameof(floatingTolerance), floatingTolerance);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaQuantizedAnimationTrackCompressionParams);
        }

        public bool Equals(hkaQuantizedAnimationTrackCompressionParams? other)
        {
            return other is not null &&
                   rotationTolerance.Equals(other.rotationTolerance) &&
                   translationTolerance.Equals(other.translationTolerance) &&
                   scaleTolerance.Equals(other.scaleTolerance) &&
                   floatingTolerance.Equals(other.floatingTolerance) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(rotationTolerance);
            hashcode.Add(translationTolerance);
            hashcode.Add(scaleTolerance);
            hashcode.Add(floatingTolerance);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

