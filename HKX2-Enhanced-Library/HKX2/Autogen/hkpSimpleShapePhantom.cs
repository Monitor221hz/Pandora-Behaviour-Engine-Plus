using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSimpleShapePhantom Signatire: 0x32a2a8a8 size: 448 flags: FLAGS_NONE

    // collisionDetails class: hkpSimpleShapePhantomCollisionDetail Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 416 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // orderDirty class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 432 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpSimpleShapePhantom : hkpShapePhantom, IEquatable<hkpSimpleShapePhantom?>
    {
        public IList<hkpSimpleShapePhantomCollisionDetail> collisionDetails { set; get; } = Array.Empty<hkpSimpleShapePhantomCollisionDetail>();
        private bool orderDirty { set; get; }

        public override uint Signature { set; get; } = 0x32a2a8a8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            collisionDetails = des.ReadClassArray<hkpSimpleShapePhantomCollisionDetail>(br);
            orderDirty = br.ReadBoolean();
            br.Position += 15;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, collisionDetails);
            bw.WriteBoolean(orderDirty);
            bw.Position += 15;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(collisionDetails));
            xs.WriteSerializeIgnored(xe, nameof(orderDirty));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSimpleShapePhantom);
        }

        public bool Equals(hkpSimpleShapePhantom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

