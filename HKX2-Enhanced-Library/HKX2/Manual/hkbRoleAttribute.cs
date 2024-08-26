using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbRoleAttribute Signatire: 0x3eb2e082 size: 4 flags: FLAGS_NONE

    // role class:  Type.TYPE_ENUM Type.TYPE_INT16 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: Role
    // flags class:  Type.TYPE_FLAGS Type.TYPE_INT16 arrSize: 0 offset: 2 flags: FLAGS_NONE enum: RoleFlags
    public partial class hkbRoleAttribute : IHavokObject, IEquatable<hkbRoleAttribute?>
    {
        public short role { set; get; }
        public short flags { set; get; }

        public virtual uint Signature { set; get; } = 0x3eb2e082;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            role = br.ReadInt16();
            flags = br.ReadInt16();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt16(role);
            bw.WriteInt16(flags);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            role = xd.ReadFlag<Role, short>(xe, nameof(role));
            flags = xd.ReadFlag<RoleFlags, short>(xe, nameof(flags));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteEnum<Role, short>(xe, nameof(role), role);
            xs.WriteFlag<RoleFlags, short>(xe, nameof(flags), flags);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbRoleAttribute);
        }

        public bool Equals(hkbRoleAttribute? other)
        {
            // FIXME: flag type overflow https://github.com/ret2end/HKX2Library/issues/4
            return other is not null &&
                   role.Equals(other.role) &&
                   //flags.Equals(other.flags) &&
                   Signature == other.Signature;
        }


        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(role);
            //hashcode.Add(flags);
            hashcode.Add(Signature);
            return hashcode.ToHashCode(); ;
        }
    }
}

