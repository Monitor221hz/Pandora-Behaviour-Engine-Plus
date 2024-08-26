using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbVariableInfo Signatire: 0x9e746ba2 size: 6 flags: FLAGS_NONE

    // role class: hkbRoleAttribute Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 4 flags: FLAGS_NONE enum: VariableType
    public partial class hkbVariableInfo : IHavokObject, IEquatable<hkbVariableInfo?>
    {
        public hkbRoleAttribute role { set; get; } = new();
        public sbyte type { set; get; } = default;

        public virtual uint Signature { set; get; } = 0x9e746ba2;
        public hkbVariableInfo()
        {
            
        }
        public hkbVariableInfo(hkbVariableInfo other)
        {
            role = other.role;
            type = other.type;
        }
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            role = new hkbRoleAttribute();
            role.Read(des, br);
            type = br.ReadSByte();
            br.Position += 1;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            role.Write(s, bw);
            s.WriteSByte(bw, type);
            bw.Position += 1;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            role = xd.ReadClass<hkbRoleAttribute>(xe, nameof(role));
            // XXX: inconsistent type, it just work.
            type = (sbyte)xd.ReadFlag<VariableType, uint>(xe, nameof(type));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass(xe, nameof(role), role);
            xs.WriteEnum<VariableType, sbyte>(xe, nameof(type), type);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbVariableInfo);
        }

        public bool Equals(hkbVariableInfo? other)
        {
            return other is not null &&
                   ((role is null && other.role is null) || (role is not null && role.Equals((IHavokObject)other.role))) &&
                   type == other.type &&
                   Signature == other.Signature;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(role);
            hashcode.Add(type);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

