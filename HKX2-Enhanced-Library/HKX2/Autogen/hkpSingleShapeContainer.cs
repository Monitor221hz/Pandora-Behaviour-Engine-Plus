using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSingleShapeContainer Signatire: 0x73aa1d38 size: 16 flags: FLAGS_NONE

    // childShape class: hkpShape Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpSingleShapeContainer : hkpShapeContainer, IEquatable<hkpSingleShapeContainer?>
    {
        public hkpShape? childShape { set; get; }

        public override uint Signature { set; get; } = 0x73aa1d38;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            childShape = des.ReadClassPointer<hkpShape>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, childShape);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            childShape = xd.ReadClassPointer<hkpShape>(this, xe, nameof(childShape));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(childShape), childShape);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSingleShapeContainer);
        }

        public bool Equals(hkpSingleShapeContainer? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((childShape is null && other.childShape is null) || (childShape is not null && other.childShape is not null && childShape.Equals((IHavokObject)other.childShape))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(childShape);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

