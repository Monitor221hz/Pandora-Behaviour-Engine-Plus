using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaDefaultAnimatedReferenceFrame Signatire: 0x6d85e445 size: 80 flags: FLAGS_NONE

    // up class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // forward class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // duration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // referenceFrameSamples class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    public partial class hkaDefaultAnimatedReferenceFrame : hkaAnimatedReferenceFrame, IEquatable<hkaDefaultAnimatedReferenceFrame?>
    {
        public Vector4 up { set; get; }
        public Vector4 forward { set; get; }
        public float duration { set; get; }
        public IList<Vector4> referenceFrameSamples { set; get; } = Array.Empty<Vector4>();

        public override uint Signature { set; get; } = 0x6d85e445;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            up = br.ReadVector4();
            forward = br.ReadVector4();
            duration = br.ReadSingle();
            br.Position += 4;
            referenceFrameSamples = des.ReadVector4Array(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(up);
            bw.WriteVector4(forward);
            bw.WriteSingle(duration);
            bw.Position += 4;
            s.WriteVector4Array(bw, referenceFrameSamples);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            up = xd.ReadVector4(xe, nameof(up));
            forward = xd.ReadVector4(xe, nameof(forward));
            duration = xd.ReadSingle(xe, nameof(duration));
            referenceFrameSamples = xd.ReadVector4Array(xe, nameof(referenceFrameSamples));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(up), up);
            xs.WriteVector4(xe, nameof(forward), forward);
            xs.WriteFloat(xe, nameof(duration), duration);
            xs.WriteVector4Array(xe, nameof(referenceFrameSamples), referenceFrameSamples);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaDefaultAnimatedReferenceFrame);
        }

        public bool Equals(hkaDefaultAnimatedReferenceFrame? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   up.Equals(other.up) &&
                   forward.Equals(other.forward) &&
                   duration.Equals(other.duration) &&
                   referenceFrameSamples.SequenceEqual(other.referenceFrameSamples) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(up);
            hashcode.Add(forward);
            hashcode.Add(duration);
            hashcode.Add(referenceFrameSamples.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

