using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpListShapeChildInfo Signatire: 0x80df0f90 size: 32 flags: FLAGS_NONE

    // shape class: hkpShape Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: ALIGN_16|FLAGS_NONE enum: 
    // collisionFilterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // shapeSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // numChildShapes class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpListShapeChildInfo : IHavokObject, IEquatable<hkpListShapeChildInfo?>
    {
        public hkpShape? shape { set; get; }
        public uint collisionFilterInfo { set; get; }
        private int shapeSize { set; get; }
        private int numChildShapes { set; get; }

        public virtual uint Signature { set; get; } = 0x80df0f90;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            shape = des.ReadClassPointer<hkpShape>(br);
            collisionFilterInfo = br.ReadUInt32();
            shapeSize = br.ReadInt32();
            numChildShapes = br.ReadInt32();
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer(bw, shape);
            bw.WriteUInt32(collisionFilterInfo);
            bw.WriteInt32(shapeSize);
            bw.WriteInt32(numChildShapes);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            shape = xd.ReadClassPointer<hkpShape>(this, xe, nameof(shape));
            collisionFilterInfo = xd.ReadUInt32(xe, nameof(collisionFilterInfo));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointer(xe, nameof(shape), shape);
            xs.WriteNumber(xe, nameof(collisionFilterInfo), collisionFilterInfo);
            xs.WriteSerializeIgnored(xe, nameof(shapeSize));
            xs.WriteSerializeIgnored(xe, nameof(numChildShapes));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpListShapeChildInfo);
        }

        public bool Equals(hkpListShapeChildInfo? other)
        {
            return other is not null &&
                   ((shape is null && other.shape is null) || (shape is not null && other.shape is not null && shape.Equals((IHavokObject)other.shape))) &&
                   collisionFilterInfo.Equals(other.collisionFilterInfo) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(shape);
            hashcode.Add(collisionFilterInfo);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

