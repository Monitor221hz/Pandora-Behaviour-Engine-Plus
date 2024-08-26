using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCachingShapePhantom Signatire: 0xcf227f58 size: 448 flags: FLAGS_NONE

    // collisionDetails class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 416 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // orderDirty class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 432 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpCachingShapePhantom : hkpShapePhantom, IEquatable<hkpCachingShapePhantom?>
    {
        public IList<object> collisionDetails { set; get; } = Array.Empty<object>();
        private bool orderDirty { set; get; }

        public override uint Signature { set; get; } = 0xcf227f58;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyArray(br);
            orderDirty = br.ReadBoolean();
            br.Position += 15;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidArray(bw);
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
            return Equals(obj as hkpCachingShapePhantom);
        }

        public bool Equals(hkpCachingShapePhantom? other)
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

