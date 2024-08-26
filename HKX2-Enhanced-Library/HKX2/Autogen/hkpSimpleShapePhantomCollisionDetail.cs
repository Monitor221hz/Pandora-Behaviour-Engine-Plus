using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSimpleShapePhantomCollisionDetail Signatire: 0x98bfa6ce size: 8 flags: FLAGS_NOT_SERIALIZABLE

    // collidable class: hkpCollidable Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkpSimpleShapePhantomCollisionDetail : IHavokObject, IEquatable<hkpSimpleShapePhantomCollisionDetail?>
    {
        public hkpCollidable? collidable { set; get; }

        public virtual uint Signature { set; get; } = 0x98bfa6ce;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            collidable = des.ReadClassPointer<hkpCollidable>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer(bw, collidable);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            collidable = xd.ReadClassPointer<hkpCollidable>(this, xe, nameof(collidable));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointer(xe, nameof(collidable), collidable);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSimpleShapePhantomCollisionDetail);
        }

        public bool Equals(hkpSimpleShapePhantomCollisionDetail? other)
        {
            return other is not null &&
                   ((collidable is null && other.collidable is null) || (collidable is not null && other.collidable is not null && collidable.Equals((IHavokObject)other.collidable))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(collidable);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

