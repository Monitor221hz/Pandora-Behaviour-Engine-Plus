using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCdBody Signatire: 0x54a4b841 size: 32 flags: FLAGS_NONE

    // shape class: hkpShape Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // shapeKey class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // motion class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // parent class: hkpCdBody Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpCdBody : IHavokObject, IEquatable<hkpCdBody?>
    {
        public hkpShape? shape { set; get; }
        public uint shapeKey { set; get; }
        private object? motion { set; get; }
        private hkpCdBody? parent { set; get; }

        public virtual uint Signature { set; get; } = 0x54a4b841;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            shape = des.ReadClassPointer<hkpShape>(br);
            shapeKey = br.ReadUInt32();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            parent = des.ReadClassPointer<hkpCdBody>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer(bw, shape);
            bw.WriteUInt32(shapeKey);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            s.WriteClassPointer(bw, parent);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            shape = xd.ReadClassPointer<hkpShape>(this, xe, nameof(shape));
            shapeKey = xd.ReadUInt32(xe, nameof(shapeKey));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointer(xe, nameof(shape), shape);
            xs.WriteNumber(xe, nameof(shapeKey), shapeKey);
            xs.WriteSerializeIgnored(xe, nameof(motion));
            xs.WriteSerializeIgnored(xe, nameof(parent));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCdBody);
        }

        public bool Equals(hkpCdBody? other)
        {
            return other is not null &&
                   ((shape is null && other.shape is null) || (shape is not null && other.shape is not null && shape.Equals((IHavokObject)other.shape))) &&
                   shapeKey.Equals(other.shapeKey) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(shape);
            hashcode.Add(shapeKey);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

