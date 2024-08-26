using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSerializedDisplayRbTransformsDisplayTransformPair Signatire: 0x94ac5bec size: 80 flags: FLAGS_NONE

    // rb class: hkpRigidBody Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // localToDisplay class:  Type.TYPE_TRANSFORM Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpSerializedDisplayRbTransformsDisplayTransformPair : IHavokObject, IEquatable<hkpSerializedDisplayRbTransformsDisplayTransformPair?>
    {
        public hkpRigidBody? rb { set; get; }
        public Matrix4x4 localToDisplay { set; get; }

        public virtual uint Signature { set; get; } = 0x94ac5bec;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            rb = des.ReadClassPointer<hkpRigidBody>(br);
            br.Position += 8;
            localToDisplay = des.ReadTransform(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer(bw, rb);
            bw.Position += 8;
            s.WriteTransform(bw, localToDisplay);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            rb = xd.ReadClassPointer<hkpRigidBody>(this, xe, nameof(rb));
            localToDisplay = xd.ReadTransform(xe, nameof(localToDisplay));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointer(xe, nameof(rb), rb);
            xs.WriteTransform(xe, nameof(localToDisplay), localToDisplay);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSerializedDisplayRbTransformsDisplayTransformPair);
        }

        public bool Equals(hkpSerializedDisplayRbTransformsDisplayTransformPair? other)
        {
            return other is not null &&
                   ((rb is null && other.rb is null) || (rb is not null && other.rb is not null && rb.Equals((IHavokObject)other.rb))) &&
                   localToDisplay.Equals(other.localToDisplay) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(rb);
            hashcode.Add(localToDisplay);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

