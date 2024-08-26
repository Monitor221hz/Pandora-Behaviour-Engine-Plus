using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBvShape Signatire: 0x286eb64c size: 56 flags: FLAGS_NONE

    // boundingVolumeShape class: hkpShape Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // childShape class: hkpSingleShapeContainer Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    public partial class hkpBvShape : hkpShape, IEquatable<hkpBvShape?>
    {
        public hkpShape? boundingVolumeShape { set; get; }
        public hkpSingleShapeContainer childShape { set; get; } = new();

        public override uint Signature { set; get; } = 0x286eb64c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            boundingVolumeShape = des.ReadClassPointer<hkpShape>(br);
            childShape.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, boundingVolumeShape);
            childShape.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            boundingVolumeShape = xd.ReadClassPointer<hkpShape>(this, xe, nameof(boundingVolumeShape));
            childShape = xd.ReadClass<hkpSingleShapeContainer>(xe, nameof(childShape));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(boundingVolumeShape), boundingVolumeShape);
            xs.WriteClass<hkpSingleShapeContainer>(xe, nameof(childShape), childShape);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBvShape);
        }

        public bool Equals(hkpBvShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((boundingVolumeShape is null && other.boundingVolumeShape is null) || (boundingVolumeShape is not null && other.boundingVolumeShape is not null && boundingVolumeShape.Equals((IHavokObject)other.boundingVolumeShape))) &&
                   ((childShape is null && other.childShape is null) || (childShape is not null && other.childShape is not null && childShape.Equals((IHavokObject)other.childShape))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(boundingVolumeShape);
            hashcode.Add(childShape);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

