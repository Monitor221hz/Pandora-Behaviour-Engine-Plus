using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkClassEnum Signatire: 0x8a3609cf size: 40 flags: FLAGS_NONE

    // name class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // items class: hkClassEnumItem Type.TYPE_SIMPLEARRAY Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // attributes class: hkCustomAttributes Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // flags class:  Type.TYPE_FLAGS Type.TYPE_UINT32 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: FlagValues
    public partial class hkClassEnum : IHavokObject, IEquatable<hkClassEnum?>
    {
        public string name { set; get; } = "";
        public object? items { set; get; }
        private hkCustomAttributes? attributes { set; get; }
        public uint flags { set; get; }

        public virtual uint Signature { set; get; } = 0x8a3609cf;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            name = des.ReadCString(br);
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            attributes = des.ReadClassPointer<hkCustomAttributes>(br);
            flags = br.ReadUInt32();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteCString(bw, name);
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            //s.WriteClassPointer(bw, attributes);
            //bw.WriteUInt32(flags);
            //bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            name = xd.ReadString(xe, nameof(name));
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            //flags = xd.ReadFlag<FlagValues, uint>(xe, nameof(flags));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(name), name);
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            //xs.WriteSerializeIgnored(xe, nameof(attributes));
            //xs.WriteFlag<FlagValues, uint>(xe, nameof(flags), flags);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkClassEnum);
        }

        public bool Equals(hkClassEnum? other)
        {
            return other is not null &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   items!.Equals(other.items) &&
                   flags.Equals(other.flags) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(name);
            hashcode.Add(items);
            hashcode.Add(flags);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

