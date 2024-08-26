using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkClassMember Signatire: 0x5c7ea4c2 size: 40 flags: FLAGS_NONE

    // name class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    //@class class: hkClass Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // @enum class: hkClassEnum Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: Type
    // subtype class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 25 flags: FLAGS_NONE enum: Type
    // cArraySize class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 26 flags: FLAGS_NONE enum: 
    // flags class:  Type.TYPE_FLAGS Type.TYPE_UINT16 arrSize: 0 offset: 28 flags: FLAGS_NONE enum: FlagValues
    // offset class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 30 flags: FLAGS_NONE enum: 
    // attributes class: hkCustomAttributes Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkClassMember : IHavokObject, IEquatable<hkClassMember?>
    {
        public string name { set; get; } = "";
        public hkClass?@class { set; get; }
        public hkClassEnum? @enum { set; get; }
        public byte type { set; get; }
        public byte subtype { set; get; }
        public short cArraySize { set; get; }
        public ushort flags { set; get; }
        public ushort offset { set; get; }
        private hkCustomAttributes? attributes { set; get; }

        public virtual uint Signature { set; get; } = 0x5c7ea4c2;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            name = des.ReadCString(br);
           @class = des.ReadClassPointer<hkClass>(br);
            @enum = des.ReadClassPointer<hkClassEnum>(br);
            type = br.ReadByte();
            subtype = br.ReadByte();
            cArraySize = br.ReadInt16();
            flags = br.ReadUInt16();
            offset = br.ReadUInt16();
            attributes = des.ReadClassPointer<hkCustomAttributes>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteCString(bw, name);
            s.WriteClassPointer(bw, @class);
            s.WriteClassPointer(bw, @enum);
            bw.WriteByte(type);
            bw.WriteByte(subtype);
            bw.WriteInt16(cArraySize);
            bw.WriteUInt16(flags);
            bw.WriteUInt16(offset);
            s.WriteClassPointer(bw, attributes);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            name = xd.ReadString(xe, nameof(name));
          @class = xd.ReadClassPointer<hkClass>(this, xe, nameof(@class));
            @enum = xd.ReadClassPointer<hkClassEnum>(this, xe, nameof(@enum));
            type = xd.ReadFlag<Type, byte>(xe, nameof(type));
            subtype = xd.ReadFlag<Type, byte>(xe, nameof(subtype));
            cArraySize = xd.ReadInt16(xe, nameof(cArraySize));
            flags = xd.ReadFlag<FlagValues, ushort>(xe, nameof(flags));
            offset = xd.ReadUInt16(xe, nameof(offset));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(name), name);
            xs.WriteClassPointer(xe, nameof(@class), @class);
            xs.WriteClassPointer(xe, nameof(@enum), @enum);
            xs.WriteEnum<Type, byte>(xe, nameof(type), type);
            xs.WriteEnum<Type, byte>(xe, nameof(subtype), subtype);
            xs.WriteNumber(xe, nameof(cArraySize), cArraySize);
            xs.WriteFlag<FlagValues, ushort>(xe, nameof(flags), flags);
            xs.WriteNumber(xe, nameof(offset), offset);
            xs.WriteSerializeIgnored(xe, nameof(attributes));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkClassMember);
        }

        public bool Equals(hkClassMember? other)
        {
            return other is not null &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   ((@class is null && other.@class is null) || (@class is not null && other.@class is not null && @class.Equals((IHavokObject)other.@class))) &&
                   ((@enum is null && other.@enum is null) || (@enum is not null && other.@enum is not null && @enum.Equals((IHavokObject)other.@enum))) &&
                   type.Equals(other.type) &&
                   subtype.Equals(other.subtype) &&
                   cArraySize.Equals(other.cArraySize) &&
                   flags.Equals(other.flags) &&
                   offset.Equals(other.offset) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(name);
            hashcode.Add(@class);
            hashcode.Add(@enum);
            hashcode.Add(type);
            hashcode.Add(subtype);
            hashcode.Add(cArraySize);
            hashcode.Add(flags);
            hashcode.Add(offset);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

