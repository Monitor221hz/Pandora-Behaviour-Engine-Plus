using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpShapePhantom Signatire: 0xcb22fbcd size: 416 flags: FLAGS_NONE

    // motionState class: hkMotionState Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 240 flags: FLAGS_NONE enum: 
    public partial class hkpShapePhantom : hkpPhantom, IEquatable<hkpShapePhantom?>
    {
        public hkMotionState motionState { set; get; } = new();

        public override uint Signature { set; get; } = 0xcb22fbcd;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            motionState.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            motionState.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            motionState = xd.ReadClass<hkMotionState>(xe, nameof(motionState));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkMotionState>(xe, nameof(motionState), motionState);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpShapePhantom);
        }

        public bool Equals(hkpShapePhantom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((motionState is null && other.motionState is null) || (motionState is not null && other.motionState is not null && motionState.Equals((IHavokObject)other.motionState))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(motionState);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

