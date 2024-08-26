using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpAabbPhantom Signatire: 0x2c5189dd size: 304 flags: FLAGS_NONE

    // aabb class: hkAabb Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 240 flags: FLAGS_NONE enum: 
    // overlappingCollidables class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 272 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // orderDirty class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 288 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpAabbPhantom : hkpPhantom, IEquatable<hkpAabbPhantom?>
    {
        public hkAabb aabb { set; get; } = new();
        public IList<object> overlappingCollidables { set; get; } = Array.Empty<object>();
        private bool orderDirty { set; get; }

        public override uint Signature { set; get; } = 0x2c5189dd;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            aabb.Read(des, br);
            des.ReadEmptyArray(br);
            orderDirty = br.ReadBoolean();
            br.Position += 15;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            aabb.Write(s, bw);
            s.WriteVoidArray(bw);
            bw.WriteBoolean(orderDirty);
            bw.Position += 15;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            aabb = xd.ReadClass<hkAabb>(xe, nameof(aabb));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkAabb>(xe, nameof(aabb), aabb);
            xs.WriteSerializeIgnored(xe, nameof(overlappingCollidables));
            xs.WriteSerializeIgnored(xe, nameof(orderDirty));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpAabbPhantom);
        }

        public bool Equals(hkpAabbPhantom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((aabb is null && other.aabb is null) || (aabb is not null && other.aabb is not null && aabb.Equals((IHavokObject)other.aabb))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(aabb);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

