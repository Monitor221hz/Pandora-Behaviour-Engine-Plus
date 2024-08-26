using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpTransformShape Signatire: 0x787ef513 size: 144 flags: FLAGS_NONE

    // childShape class: hkpSingleShapeContainer Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // childShapeSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // rotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // transform class:  Type.TYPE_TRANSFORM Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkpTransformShape : hkpShape, IEquatable<hkpTransformShape?>
    {
        public hkpSingleShapeContainer childShape { set; get; } = new();
        private int childShapeSize { set; get; }
        public Quaternion rotation { set; get; }
        public Matrix4x4 transform { set; get; }

        public override uint Signature { set; get; } = 0x787ef513;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            childShape.Read(des, br);
            childShapeSize = br.ReadInt32();
            br.Position += 12;
            rotation = des.ReadQuaternion(br);
            transform = des.ReadTransform(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            childShape.Write(s, bw);
            bw.WriteInt32(childShapeSize);
            bw.Position += 12;
            s.WriteQuaternion(bw, rotation);
            s.WriteTransform(bw, transform);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            childShape = xd.ReadClass<hkpSingleShapeContainer>(xe, nameof(childShape));
            rotation = xd.ReadQuaternion(xe, nameof(rotation));
            transform = xd.ReadTransform(xe, nameof(transform));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpSingleShapeContainer>(xe, nameof(childShape), childShape);
            xs.WriteSerializeIgnored(xe, nameof(childShapeSize));
            xs.WriteQuaternion(xe, nameof(rotation), rotation);
            xs.WriteTransform(xe, nameof(transform), transform);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpTransformShape);
        }

        public bool Equals(hkpTransformShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((childShape is null && other.childShape is null) || (childShape is not null && other.childShape is not null && childShape.Equals((IHavokObject)other.childShape))) &&
                   rotation.Equals(other.rotation) &&
                   transform.Equals(other.transform) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(childShape);
            hashcode.Add(rotation);
            hashcode.Add(transform);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

