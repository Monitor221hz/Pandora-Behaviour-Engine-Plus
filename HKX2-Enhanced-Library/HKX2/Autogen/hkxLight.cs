using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxLight Signatire: 0x81c86d42 size: 80 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: LightType
    // position class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // direction class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // color class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // angle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    public partial class hkxLight : hkReferencedObject, IEquatable<hkxLight?>
    {
        public sbyte type { set; get; }
        public Vector4 position { set; get; }
        public Vector4 direction { set; get; }
        public uint color { set; get; }
        public float angle { set; get; }

        public override uint Signature { set; get; } = 0x81c86d42;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            type = br.ReadSByte();
            br.Position += 15;
            position = br.ReadVector4();
            direction = br.ReadVector4();
            color = br.ReadUInt32();
            angle = br.ReadSingle();
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSByte(type);
            bw.Position += 15;
            bw.WriteVector4(position);
            bw.WriteVector4(direction);
            bw.WriteUInt32(color);
            bw.WriteSingle(angle);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            type = xd.ReadFlag<LightType, sbyte>(xe, nameof(type));
            position = xd.ReadVector4(xe, nameof(position));
            direction = xd.ReadVector4(xe, nameof(direction));
            color = xd.ReadUInt32(xe, nameof(color));
            angle = xd.ReadSingle(xe, nameof(angle));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<LightType, sbyte>(xe, nameof(type), type);
            xs.WriteVector4(xe, nameof(position), position);
            xs.WriteVector4(xe, nameof(direction), direction);
            xs.WriteNumber(xe, nameof(color), color);
            xs.WriteFloat(xe, nameof(angle), angle);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxLight);
        }

        public bool Equals(hkxLight? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   type.Equals(other.type) &&
                   position.Equals(other.position) &&
                   direction.Equals(other.direction) &&
                   color.Equals(other.color) &&
                   angle.Equals(other.angle) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(type);
            hashcode.Add(position);
            hashcode.Add(direction);
            hashcode.Add(color);
            hashcode.Add(angle);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

