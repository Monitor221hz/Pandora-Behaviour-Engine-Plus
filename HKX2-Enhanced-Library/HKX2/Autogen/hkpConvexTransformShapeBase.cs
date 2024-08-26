using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConvexTransformShapeBase Signatire: 0xfbd72f9 size: 64 flags: FLAGS_NONE

    // childShape class: hkpSingleShapeContainer Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // childShapeSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpConvexTransformShapeBase : hkpConvexShape, IEquatable<hkpConvexTransformShapeBase?>
    {
        public hkpSingleShapeContainer childShape { set; get; } = new();
        private int childShapeSize { set; get; }

        public override uint Signature { set; get; } = 0xfbd72f9;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            childShape.Read(des, br);
            childShapeSize = br.ReadInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            childShape.Write(s, bw);
            bw.WriteInt32(childShapeSize);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            childShape = xd.ReadClass<hkpSingleShapeContainer>(xe, nameof(childShape));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpSingleShapeContainer>(xe, nameof(childShape), childShape);
            xs.WriteSerializeIgnored(xe, nameof(childShapeSize));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConvexTransformShapeBase);
        }

        public bool Equals(hkpConvexTransformShapeBase? other)
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

