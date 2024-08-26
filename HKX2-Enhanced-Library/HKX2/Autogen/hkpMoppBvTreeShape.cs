using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMoppBvTreeShape Signatire: 0x90b29d39 size: 112 flags: FLAGS_NONE

    // child class: hkpSingleShapeContainer Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // childSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpMoppBvTreeShape : hkMoppBvTreeShapeBase, IEquatable<hkpMoppBvTreeShape?>
    {
        public hkpSingleShapeContainer child { set; get; } = new();
        private int childSize { set; get; }

        public override uint Signature { set; get; } = 0x90b29d39;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            child.Read(des, br);
            childSize = br.ReadInt32();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            child.Write(s, bw);
            bw.WriteInt32(childSize);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            child = xd.ReadClass<hkpSingleShapeContainer>(xe, nameof(child));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpSingleShapeContainer>(xe, nameof(child), child);
            xs.WriteSerializeIgnored(xe, nameof(childSize));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMoppBvTreeShape);
        }

        public bool Equals(hkpMoppBvTreeShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((child is null && other.child is null) || (child is not null && other.child is not null && child.Equals((IHavokObject)other.child))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(child);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

